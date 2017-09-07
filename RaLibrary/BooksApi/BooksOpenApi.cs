using System.Threading.Tasks;

namespace RaLibrary.BooksApi
{
    public interface BooksOpenApi
    {
        Task<BookDetails> QueryIsbnAsync(string isbn);
    }
}
