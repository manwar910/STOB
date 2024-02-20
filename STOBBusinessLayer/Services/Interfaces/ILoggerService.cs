using STOBBusinessLayer.Utility;

namespace STOBBusinessLayer.Services.Interfaces
{
    public interface ILoggerService : IService
    {
        void InsertLog(Logger logger);
    }
}
