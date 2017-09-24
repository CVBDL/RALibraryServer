using RaLibrary.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RaLibrary.Data.Managers
{
    public interface IBookManager : IDisposable
    {
        Task<BookDto> CreateAsync(BookDto bookDto);
        Task DeleteAsync(int id);
        Task<BookDto> GetAsync(int id);
        IQueryable<BookDto> List();
        Task<BookDto> UpdateAsync(BookDto bookDto);
    }
}
