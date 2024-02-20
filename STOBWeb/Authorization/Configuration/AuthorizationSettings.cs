using System.Configuration;

namespace STOBWeb.Authorization.Configuration
{
    public class AuthorizationSettings : ConfigurationSection
    {
        private static readonly AuthorizationSettings _settings = (AuthorizationSettings)ConfigurationManager.GetSection("authorizationSettings");

        public static AuthorizationSettings Settings
        {
            get { return _settings; }
        }

        [ConfigurationProperty("authRoles", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(AuthRoleConfigCollection))]
        public AuthRoleConfigCollection AuthRoles
        {
            get { return (AuthRoleConfigCollection)this["authRoles"]; }
        }
    }
}
