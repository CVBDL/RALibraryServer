using RaLibrary.BookApiProxy.Models;
using RaLibrary.Utilities;
using System.Threading.Tasks;

namespace RaLibrary.BookApiProxy
{
    public interface IBooksOpenApi
    {
        Task<BookDetailsDto> QueryIsbnAsync(Isbn isbn);
    }
}
