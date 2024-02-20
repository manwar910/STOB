using System;

namespace STOBBusinessLayer.Services.Interfaces
{
    public interface IErrorService : IService
    {
        void Log(Exception ex, Guid logGuid, string username);
    }
}