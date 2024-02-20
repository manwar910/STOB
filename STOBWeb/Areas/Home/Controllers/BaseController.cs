using STOBBusinessLayer.Services;
using STOBBusinessLayer.Utility;
using STOBEntities.Models;
using STOBWeb.Authorization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;


namespace STOBWeb.Areas.Home.Controllers
{
    public class BaseController : Controller
    {
        public string UserName { get; set; }
        public string NominatorFullName { get; set; }
        public string NominatorEmail { get; set; }

        public BaseController()
        {
            UserName = new UserInformationModel().ReturnUserName();
            using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
            {
                UserPrincipal principal = UserPrincipal.FindByIdentity(context, UserName);
                var firstName = principal.GivenName;
                var lastName = principal.Surname;
                NominatorEmail = principal.EmailAddress;
                var _UserData = new UserDataService();
                NominatorFullName = string.Format("{0}, {1}", firstName, lastName);
            }
        }
        //dons comment
        public string GetCurrentVersion()
        {
            return new ReleaseInformation().ReturnCurrentVersion();
        }

        public string GetlastUpdatedDate()
        {
            return new ReleaseInformation().ReturnlastUpdatedDate();
        }

        public string GetUserName()
        {
            return new UserInformationModel().ReturnUserName();
        }
        public List<string> GetUserRoles(string userName)
        {
            List<string> roles = new Three60RoleProvider().GetRolesForUser(userName).ToList();
            roles.Add("AdminRole");
            return roles;
        }
        public string GetImagePath()
        {
            string urlPath = Url.Content("~/Content/images/curve.png");
            return urlPath;
        }
        public string GetlogoImagePath()
        {
            string urlPath = Url.Content("~/Content/Assets/abc-logo-white.png");
            return urlPath;
        }
        public bool CheckUserHasStobRoles(List<string> roles)
        {
            bool hasStobTole = false;

            roles.ForEach(x => {
                string stob = x.Split('-')[1];
                if (string.Equals(stob, "stob", System.StringComparison.OrdinalIgnoreCase))
                {
                    hasStobTole = true;
                }
            });

            return hasStobTole;
        }
        protected PartialViewResult ErrorPartialViewResult()
        {
            return PartialView("~/Areas/Errors/Views/Shared/ErrorMessage.cshtml");
        }

        protected ActionResult RedirectError()
        {
            return RedirectToAction("Error", new { area = "Error", controller = "Error" });
        }

        protected ActionResult RedirectNotFound()
        {
            return RedirectToAction("NotFound", new { area = "Error", controller = "Error" });
        }

        protected ActionResult JsonError()
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return Json("Error");
        }
        public static void SendMail(string body, string MailTo, string Path)
        {
            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["SendMail"]))
                return;

            var smtpServer = ConfigurationManager.AppSettings["SendMailSMPTServer"];
            SmtpClient client = new SmtpClient(smtpServer) { UseDefaultCredentials = true };
           
            MailMessage email = new MailMessage();

            email.To.Add(MailTo);           
            
            email.From = new MailAddress(ConfigurationManager.AppSettings["SendMailFrom"]);            
            email.IsBodyHtml = true;
            email.Body = body.ToString();
            email.Subject = ConfigurationManager.AppSettings["SendMailSubject"];
            //client.Send(email);
            email.Dispose();
        }


    }
}