using RaLibrary.Utils;
using System.Threading.Tasks;

namespace RaLibrary.BooksApi
{
    public interface IBooksOpenApi
    {
        Task<BookDetails> QueryIsbnAsync(Isbn isbn);
    }
}
