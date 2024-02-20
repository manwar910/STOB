using System;

namespace STOBEntities.BoilerPlateDefaults
{
    [Serializable]
    public class Error
    {
        public int ErrorId { get; set; }
        public Guid? LogGuid { get; set; }
        public DateTime ErrorDateTime { get; set; }
        public string UserName { get; set; }
        public string ErrorType { get; set; }
        public string ErrorSourceText { get; set; }
        public string ErrorMessageText { get; set; }
        public string StackTraceText { get; set; }
        public string ServerName { get; set; }
    }
}
