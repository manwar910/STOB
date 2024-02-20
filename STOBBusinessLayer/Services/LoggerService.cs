using STOBBusinessLayer.Services.Interfaces;
using STOBBusinessLayer.Utility;
using STOBDataLayer.Contexts;
using STOBDataLayer.Models.BoilerPlateDefaults;

namespace STOBBusinessLayer.Services
{
    public class LoggerService : ILoggerService
    {
        BoilerPlateDefaults db = new BoilerPlateDefaults();

        public void Dispose()
        {
            db.Dispose();
        }

        public void InsertLog(Logger logger)
        {
            db.LOG.Add(ConvertLogger(logger));
            db.SaveChanges();
        }

        private LOG ConvertLogger(Logger logger)
        {
            LOG log = new LOG
            {
                LOG_GLBL_UNIQUE_IDENTIFIER = logger.LogGuid,
                USER_NM = logger.UserName,
                SESN_ID = logger.SessionId,
                TITLE_TXT = logger.Title,
                CLASS_NM = logger.Class,
                MTHD_TXT = logger.Method,
                INP_TXT = (logger.Input.Length > 8000) ? logger.Input.Substring(0, 7999) : logger.Input,
                LOG_DTM = logger.LogDate,
                DUR_TM_QTY = logger.Duration,
                SRVR_NM = logger.ServerName,
                PROC_ID = logger.ProcessId,
                THREAD_ID = logger.ThreadId,
                USER_DEF_TXT_1 = logger.UserDefined1,
                USER_DEF_TXT_2 = logger.UserDefined2
            };

            return log;
        }
    }
}
