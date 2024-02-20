using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using STOBWeb.Three60AuthorizationService;

namespace STOBWeb.Authorization
{
    public class Three60RoleProvider : RoleProvider
    {
        public IThree60AuthService Three60AuthClient { get; set; }

        public override string ApplicationName { get; set; }

        public Three60RoleProvider()
        {
            Three60AuthClient = new Three60AuthService();
        }


        public override bool IsUserInRole(string username, string roleName)
        {
            var uid = username.Contains("\\") ? username.Split('\\')[1] : username;
            try
            {
                return Three60AuthClient.IsInGroup(uid, roleName, true);
            }
            catch (Exception)
            {
                //Consider logging 360 exception
                return false;
            }
        }

        public override string[] GetRolesForUser(string username)
        {
            var uid = username.Contains("\\") ? username.Split('\\')[1] : username;
            Group[] groups;

            try
            {
                groups = Three60AuthClient.GetGroupsForUser(uid, true);
            }
            catch (Exception)
            {
                //Consider logging 360 exception
                groups = new Group[] { };
            }
            var roles = new List<string>();
            roles.AddRange(groups.Select(g => g.Cn));
            return roles.ToArray();
        }

        #region Not Implemented Methods

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        #endregion

    }

    #region Three60AuthService Wrapper

    public interface IThree60AuthService
    {
        bool IsInGroup(string accountName, string group, bool searcSubGroups);
        Group[] GetGroupsForUser(string accountName, bool searcSubGroups);
    }

    public class Three60AuthService : IThree60AuthService
    {
        private readonly AuthorizationServiceSoapClient _client;

        public Three60AuthService()
        {
            _client = new AuthorizationServiceSoapClient("BasicHttpBinding_AuthorizationServiceSoap");
        }

        public bool IsInGroup(string accountName, string group, bool searcSubGroups)
        {
            var uid = accountName.Contains("\\") ? accountName.Split('\\')[1] : accountName;
            return _client.IsInGroup(uid, group, searcSubGroups);
        }

        public Group[] GetGroupsForUser(string accountName, bool searcSubGroups)
        {
            return _client.GetGroupsForUser(accountName, searcSubGroups);

        }
    }
    #endregion
}