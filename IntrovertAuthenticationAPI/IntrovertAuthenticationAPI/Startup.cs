using AutoMapper;
using IntrovertAuthenticationAPI.Processors;
using IntrovertAuthenticationAPI.Services;
using IntrovertAuthenticationAPI.Settings;
using IntrovertAuthenticationAPI.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

namespace IntrovertAuthenticationAPI
{
    /// <summary>
    /// The startup.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            // Register the Swagger generator
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Introvert Authentication API",
                    Description = "Identity service for Introvert application.",
                    Contact = new Contact
                    {
                        Name = "Alen Smailovic",
                        Email = "alen.smailovic@gmail.com"
                    }
                });
            });

            // configure MongoDB connection
            services.Configure<IdentityDatabase>(Configuration.GetSection("IdentityDatabase"));
            services.AddSingleton(sp =>
                sp.GetRequiredService<IOptions<IdentityDatabase>>().Value);

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("IdentitySettings");
            services.Configure<IdentitySettings>(appSettingsSection);

            // configure JWT authentication
            var appSettings = appSettingsSection.Get<IdentitySettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        var userId = context.Principal.Identity.Name;
                        var user = userService.GetUser(userId);
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddTransient<IUserProcessor, UserProcessor>();
            services.AddTransient<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui, specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Introvert Authorization API");
            });

            // Based on the following link, the maximum number for Access-Control-Max-Age in Chromium is 600 (10 minutes) Having this, in order
            // to ensure that we don't have a great value and to be ignored, we set the value to 5 minutes https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Access-Control-Max-Age
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowCredentials()
                .SetPreflightMaxAge(TimeSpan.FromMinutes(5))
                .WithExposedHeaders("X-Total-Count", "Content-Disposition")
            );

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
