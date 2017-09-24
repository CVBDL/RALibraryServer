using RaLibrary.Data.Entities;
using RaLibrary.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RaLibrary.Data.Managers
{
    public interface IBorrowManager : IDisposable
    {
        Task<Borrow> CreateAsync(Borrow borrow);
        Task DeleteAsync(int id);
        Task DeleteByBookIdAsync(int bookId);
        Task<Borrow> GetAsync(int id);
        Borrow GetByBookId(int id);
        IQueryable<BorrowDto> List();
        IQueryable<BorrowDto> List(string borrower);
    }
}