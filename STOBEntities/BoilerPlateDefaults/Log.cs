using System;

namespace STOBEntities.BoilerPlateDefaults
{
    [Serializable]
    public class Log
    {
        public int LogId { get; set; }
        public Guid? LogGuid { get; set; }
        public string UserName { get; set; }
        public string SessionId { get; set; }
        public string TitleText { get; set; }
        public string ClassName { get; set; }
        public string MethodText { get; set; }
        public string InputText { get; set; }
        public DateTime? LogDateTime { get; set; }
        public decimal DurationTime { get; set; }
        public string ServerName { get; set; }
        public int ProcId { get; set; }
        public int ThreadId { get; set; }
        public string UserDefinedText1 { get; set; }
        public string UserDefinedText2 { get; set; }

    }
}
