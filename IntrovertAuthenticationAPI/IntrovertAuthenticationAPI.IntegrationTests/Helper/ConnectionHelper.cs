/* 
 * Author: Smailovic Alen
 * Date:   01.09.2019
*/

using IntrovertAuthenticationAPI.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace IntrovertAuthenticationAPI.IntegrationTests.Helper
{
    class ConnectionHelper
    {
        public static string jsonsPath = "../../../../IntrovertAuthenticationAPI/appsettings.Testing.json";
        public static string databaseSection = "IdentityDatabase";

        public static IdentityDatabase Create()
        {
            IdentityDatabase connection = null;
            using (StreamReader file = new StreamReader(jsonsPath))
            {
                JObject json = JObject.Parse(file.ReadToEnd());

                if (json.TryGetValue(databaseSection, StringComparison.CurrentCulture, out JToken jToken))
                {
                    if (jToken != null)
                    {
                        connection = JsonConvert.DeserializeObject<IdentityDatabase>(jToken.ToString());
                    }
                }
            }

            return connection;
        }
    }
}
