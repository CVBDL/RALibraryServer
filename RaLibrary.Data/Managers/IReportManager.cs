using RaLibrary.Data.Models;
using System;
using System.Collections.Generic;

namespace RaLibrary.Data.Managers
{
    public interface IReportManager: IDisposable
    {
        IEnumerable<BookStateDto> GetAllBooksStatusReport();
    }
}
