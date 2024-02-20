using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STOBDataLayer.Models.BoilerPlateDefaults;
using STOBEntities.BoilerPlateDefaults;

namespace STOBDataLayer.Repositories.BoilerPlateDefaults
{
    public class ErrorRepository : GenericRepository<Contexts.BoilerPlateDefaults, ERR, Error>
    {
        public override Error ConvertToModel(ERR entity)
        {
            Error error = new Error()
            {
                LogGuid = entity.LOG_GLBL_UNIQUE_IDENTIFIER,
                ErrorDateTime = entity.ERR_DTM,
                UserName = entity.USER_NM,
                ErrorType = entity.ERR_TYP,
                ErrorSourceText = entity.ERR_SRC_TXT,
                ErrorMessageText = entity.ERR_MSG_TXT,
                StackTraceText = entity.STACK_TRACE_TXT,
                ServerName = entity.SRVR_NM
            };

            return error;
        }

        public override ERR ConvertToDbObject(Error entity)
        {
            ERR error = new ERR()
            {
                LOG_GLBL_UNIQUE_IDENTIFIER = entity.LogGuid,
                ERR_DTM = entity.ErrorDateTime,
                USER_NM = entity.UserName,
                ERR_TYP = entity.ErrorType,
                ERR_SRC_TXT = entity.ErrorSourceText,
                ERR_MSG_TXT = entity.ErrorMessageText,
                STACK_TRACE_TXT = entity.StackTraceText,
                SRVR_NM = entity.ServerName
            };

            return error;
        }
    }
}
