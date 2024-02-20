using System.Configuration;

namespace STOBWeb.Authorization.Configuration
{
    public class AuthRoleConfig : ConfigurationElement
    {
        [ConfigurationProperty("appRole", IsRequired = true, IsKey = true)]
        public string AppRole
        {
            get { return (string)this["appRole"]; }
            set { this["appRole"] = value; }
        }

        [ConfigurationProperty("authRole", IsRequired = true)]
        public string AuthRole
        {
            get { return (string)this["authRole"]; }
            set { this["authRole"] = value; }
        }
    }
}
