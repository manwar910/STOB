using System;
using STOBDataLayer.Repositories.BoilerPlateDefaults;
using STOBBusinessLayer.Services.Interfaces;
using STOBEntities.BoilerPlateDefaults;

namespace STOBBusinessLayer.Services
{
    public class ErrorService : IErrorService
    {
        ErrorRepository _errRepo;

        public ErrorService()
        {
        }

        public ErrorService(ErrorRepository errRepo)
        {
            this._errRepo = errRepo;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Log(Exception ex, Guid logGuid, string username)
        {
            Error e = new Error();

            e.LogGuid = logGuid;
            e.ErrorDateTime = DateTime.Now;
            e.ServerName = System.Environment.MachineName;
            e.UserName = username;
            e.ErrorType = ex.GetType().ToString();
            e.ErrorSourceText = ex.Source;
            e.ErrorMessageText = ex.InnerException != null ? ex.Message + ex.InnerException : ex.Message;
            e.StackTraceText = ex.StackTrace;

            _errRepo.AddModel(ref e);
        }
    }
}
