using System;

namespace RaLibrary.Data.Managers
{
    public interface IAdministratorManager : IDisposable
    {
        bool IsAdministrator(string email);
    }
}
