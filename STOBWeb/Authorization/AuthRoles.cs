using System.Collections.Generic;
using STOBWeb.Authorization.Configuration;

namespace STOBWeb.Authorization
{
    public static class AuthRoles
    {
        private static readonly Dictionary<string, string> _authRoles = new Dictionary<string, string>();

        static AuthRoles()
        {
            foreach (AuthRoleConfig arc in AuthorizationSettings.Settings.AuthRoles)
            {
                _authRoles.Add(arc.AppRole, arc.AuthRole);
            }

        }

        public static string GetAuthRole(string appRole)
        {
            return _authRoles[appRole];
        }
    }
}