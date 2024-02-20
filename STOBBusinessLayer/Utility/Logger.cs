using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using STOBBusinessLayer.Services;
using STOBBusinessLayer.Services.Interfaces;
using System.Runtime.Serialization;

namespace STOBBusinessLayer.Utility
{
    public class Logger : IDisposable
    {
        private ILoggerService _loggerService;

        public int LogId { get; set; }
        public System.Guid LogGuid { get; set; }
        public string UserName { get; set; }
        public string SessionId { get; set; }
        public string Title { get; set; }
        public string Class { get; set; }
        public string Method { get; set; }
        public string Input { get; set; }
        public DateTime LogDate { get; set; }
        public Decimal Duration { get; set; }
        public string ServerName { get; set; }
        public int ProcessId { get; set; }
        public int ThreadId { get; set; }
        public string UserDefined1 { get; set; }
        public string UserDefined2 { get; set; }
        public MethodBase MethodBase { get; set; }

        public Logger(object parameters, string userDef1, string userDef2)
            : this(parameters, userDef1)
        {
            _loggerService = new LoggerService();
            SetMethod();
            this.UserDefined2 = userDef2;
        }

        public Logger(object parameters, string userDef1)
            : this(parameters)
        {
            _loggerService = new LoggerService();
            SetMethod();
            this.UserDefined1 = userDef1;
        }

        public Logger(object parameters)
            : this()
        {
            _loggerService = new LoggerService();
            SetMethod();
            this.Input = Serialize(parameters);
        }

        public Logger(MethodBase methodBase, object parameters = null)
            : this()
        {
            _loggerService = new LoggerService();
            MethodBase = methodBase;
            SetMethod();

            if (parameters != null)
                this.Input = Serialize(parameters);
        }

        public Logger()
        {
            _loggerService = new LoggerService();
            SetMethod();
            this.UserName = HttpContext.Current != null
                ? HttpContext.Current.User.Identity.Name
                : System.Threading.Thread.CurrentPrincipal.Identity.Name;
            this.LogGuid = Guid.NewGuid();
            this.SessionId = HttpContext.Current != null
                ? HttpContext.Current.Session.SessionID
                : "No Session Id Available";
            this.LogDate = DateTime.Now;
            this.Input = "null";
            this.ServerName = System.Environment.MachineName;
            this.ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;
            this.ThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
        }

        private void SetMethod()
        {
            MethodBase method;

            if (MethodBase == null)
                method = new System.Diagnostics.StackFrame(2).GetMethod();
            else
                method = MethodBase;

            this.Title = string.Format("{0}.{1}", method.ReflectedType.FullName, method.Name);
            this.Class = method.ReflectedType.FullName;
            this.Method = method.Name;
        }

        public void Dispose()
        {
            this.Duration =
                Convert.ToDecimal(new TimeSpan(DateTime.Now.Ticks - this.LogDate.Ticks).TotalMilliseconds / 1000);
            this.InsertLog();
        }

        private void InsertLog()
        {
            _loggerService.InsertLog(this);
        }

        private string Serialize(object t)
        {
            try
            {
                if (t == null)
                {
                    return "null";
                }
                else if (t.GetType().IsSerializable)
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(t.GetType(), GetTypes(t));
                    StringWriter textWriter = new StringWriter();

                    xmlSerializer.Serialize(textWriter, t);
                    return textWriter.ToString();
                }
                else if (t.GetType() == typeof(HttpPostedFileWrapper))
                {
                    return "";
                }
                else
                {
                    // http://stackoverflow.com/questions/7502658/how-to-serialize-property-of-type-icollectiont-while-using-entity-framework
                    using (var stream = new MemoryStream())
                    {
                        var serializer = new DataContractSerializer(t.GetType());
                        serializer.WriteObject(stream, t);
                        return Encoding.UTF8.GetString(stream.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                return string.Format("Data Serialization Error Occurred: \r\n\r\n{0}", GetExecptionMessages(ex));
            }
        }

        private static Type[] GetTypes(object t)
        {
            List<Type> types = new List<Type>();

            if (t.GetType().IsArray)
            {
                object[] objectArray = (object[])t;
                foreach (object o in objectArray)
                {
                    if (o != null)
                        types.Add(o.GetType());
                }
            }
            else
            {
                types.Add(t.GetType());
            }

            return types.ToArray();
        }

        private static string GetExecptionMessages(Exception ex)
        {
            string innerException = "";
            if (ex.InnerException != null)
                innerException = GetExecptionMessages(ex.InnerException);

            string returnString = ex.Message;
            if (!string.IsNullOrEmpty(innerException))
                returnString += string.Format("\r\nInnerException: {0}", innerException);

            return returnString;
        }
    }
}