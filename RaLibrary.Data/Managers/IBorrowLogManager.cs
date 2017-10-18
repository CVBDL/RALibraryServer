using RaLibrary.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RaLibrary.Data.Managers
{
    public interface IBorrowLogManager : IDisposable
    {
        Task<BorrowLogDto> CreateAsync(BorrowLogDto logDto);
        Task<BorrowLogDto> GetAsync(int id);
        Task<IQueryable<BorrowLogDto>> ListAsync();
    }
}
