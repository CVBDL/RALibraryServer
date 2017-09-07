using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace RaLibrary.BooksApi
{
    public class DoubanBooksOpenApi : BooksOpenApi
    {
        public static string baseRequestUri = ConfigurationManager.AppSettings.Get("DoubanBooksApiEndpoint");
        public static HttpClient httpClient = new HttpClient();

        public async Task<BookDetails> QueryIsbnAsync(string isbn)
        {
            string requestUri = baseRequestUri + isbn;
            HttpResponseMessage response = await httpClient.GetAsync(requestUri);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                throw new BookNotFoundException();
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            DoubanBook book = JsonConvert.DeserializeObject<DoubanBook>(responseBody);

            string authors = string.Empty;
            if (!string.IsNullOrWhiteSpace(authors))
            {
                authors = string.Join(",", book.author);
            }

            ushort? pageCount = null;
            if (!string.IsNullOrWhiteSpace(book.pages))
            {
                pageCount = ushort.Parse(book.pages);
            }

            return new BookDetails()
            {
                ISBN10 = book.isbn10,
                ISBN13 = book.isbn13,
                Title = book.title,
                Subtitle = book.subtitle,
                Authors = string.Join(",", book.author),
                Publisher = book.publisher,
                PublishedDate = book.pubdate,
                Description = book.summary,
                ThumbnailLink = book.images.medium,
                PageCount = ushort.Parse(book.pages)
            };
        }
    }
}
