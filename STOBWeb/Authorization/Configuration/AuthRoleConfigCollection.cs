using System.Configuration;

namespace STOBWeb.Authorization.Configuration
{
    [ConfigurationCollection(typeof(AuthRoleConfig))]
    public class AuthRoleConfigCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AuthRoleConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var obj = (AuthRoleConfig)element;
            return obj.AppRole;
        }
    }
}
