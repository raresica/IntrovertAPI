/* 
 * Author: Smailovic Alen
 * Date:   01.09.2019
*/

namespace IntrovertAuthenticationAPI.Settings
{
    public class IdentityDatabase
    {
        public string ConnectionString { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AdminDatabase { get; set; }
        public bool UseSSL { get; set; }
    }
}
