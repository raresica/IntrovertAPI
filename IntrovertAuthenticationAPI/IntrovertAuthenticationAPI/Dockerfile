FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["IntrovertAuthenticationAPI/IntrovertAuthenticationAPI.csproj", "IntrovertAuthenticationAPI/"]
RUN dotnet restore "IntrovertAuthenticationAPI/IntrovertAuthenticationAPI.csproj"
COPY . .
WORKDIR "/src/IntrovertAuthenticationAPI"
RUN dotnet build "IntrovertAuthenticationAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "IntrovertAuthenticationAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "IntrovertAuthenticationAPI.dll"]