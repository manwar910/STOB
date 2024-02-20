using System.Web;
using System.Web.Mvc;
using System.Linq;
using System;
using STOBEntities.Models;
using System.DirectoryServices.AccountManagement;
using STOBBusinessLayer.Services;
using System.Web.Routing;

namespace STOBWeb.Authorization
{
    public class AuthFilter : ActionFilterAttribute, IActionFilter
    {
        public string[] groups { get; set; }
        public string[] users { get; set; }
        private Three60AuthService _authClient;
        private Three60RoleProvider _roleProvider;

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            _authClient = new Three60AuthService();
            _roleProvider = new Three60RoleProvider();

            //Check if user is in passed group for authorization to view/edit page. 
            var getUserName = new UserInformationModel();
            var username = getUserName.ReturnUserName();

            //Get Directory about user 
            using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
            {
                UserPrincipal principal = UserPrincipal.FindByIdentity(context, HttpContext.Current.User.Identity.Name);
                var firstName = principal.GivenName;
                var lastName = principal.Surname;
                var emailAddress = principal.EmailAddress;
                var phoneNumber = principal.VoiceTelephoneNumber;

                //Send off data about user to update or insert into the database
                var _UserData = new UserDataService();
                //_UserData.CheckForUser(username, firstName, lastName, emailAddress, phoneNumber);
            }

            bool authorized = groups == null && users == null;

            if (groups != null)
            {
                foreach (var group in groups)
                {
                    try
                    {
                        //if (_authClient.IsInGroup(username, group, true))
                        //if (_roleProvider.IsUserInRole(username, "AdminRole"))
                        //if(_roleProvider.GetRolesForUser(username).Contains("AdminRole"))                           
                        //{
                        authorized = _roleProvider.GetRolesForUser(username).Contains("AdminRole") ||
                                     _roleProvider.GetRolesForUser(username).Contains("WSF-Stob-Role-NominationCoordinator") ||
                                     _roleProvider.GetRolesForUser(username).Contains("WSF-Stob-Role-BusinessUnit") ||
                                     _roleProvider.GetRolesForUser(username).Contains("WSF-Stob-Role-Committee");
                        //}
                    }
                    catch(Exception)
                    {
                        //assume the user is not in the group
                        authorized = false;
                    }
                    
                }
            }

            //they aren't in one of the groups so we'll see if a specified user
            var dedomainedUsername = username.Substring(username.LastIndexOf(@"\") + 1);
            if (!authorized && users != null && users.Where(u => u == dedomainedUsername).FirstOrDefault() != null)
            {
                authorized = true;
            }

            if (!authorized)
            {
                authorized = true;
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Nomination", action = "Add" }));
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);

            }

        }
    }
}