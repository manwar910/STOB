using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using STOBWeb.Authorization;

namespace STOBWeb.Authorization
{
    public enum AppRoles
    {
        SampleAppRoleView,
        SampleAppRoleUpdate
    }

    public static class SecurityPrincipalExtensions
    {
        /// <summary>
        /// Determines whether the current principal belongs to the specified app role
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="appRole"></param>
        /// <returns></returns>
        public static bool IsInRole(this IPrincipal principal, AppRoles appRole)
        {
            return principal.IsInRole(AuthRoles.GetAuthRole(appRole.ToString()));
        }
    }

    /// <summary>
    /// Extends the Authorize attribute to accept one or more AppRoles enum values and to display 
    /// the NotAuthorized view for unauthorized access
    /// </summary>
    public class AppAuthorizeAttribute : AuthorizeAttribute
    {
        public AppAuthorizeAttribute(params AppRoles[] appRoles)
        {
            var roles = appRoles.Select(appRole => AuthRoles.GetAuthRole(appRole.ToString())).ToList();
            Roles = string.Join(",", roles);
        }

        /// <summary>
        /// To disable prompt for credentials and display "NotAuthorized" view instead
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new ViewResult { ViewName = "NotAuthorized" };
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}