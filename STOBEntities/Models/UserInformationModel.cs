using System.Web;

namespace STOBEntities.Models
{
    public class UserInformationModel
    {
        public string ReturnUserName()
        {
            string username = HttpContext.Current != null
                ? HttpContext.Current.User.Identity.Name
                : System.Threading.Thread.CurrentPrincipal.Identity.Name;

            return username;
        }
    }
}
