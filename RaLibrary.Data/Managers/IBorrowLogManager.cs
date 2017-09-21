using RaLibrary.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RaLibrary.Data.Managers
{
    public interface IBorrowLogManager : IDisposable
    {
        Task CloseAsync(BorrowLogDto logDto);
        Task<BorrowLogDto> CreateAsync(BorrowLogDto logDto);
        BorrowLogDto GetActive(int bookId);
        Task<BorrowLogDto> GetAsync(int id);
        IQueryable<BorrowLogDto> List();
    }
}