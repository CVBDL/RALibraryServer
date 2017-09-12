using Newtonsoft.Json;
using RaLibrary.Utils;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace RaLibrary.BooksApi
{
    public class DoubanBooksOpenApi : IBooksOpenApi
    {
        public static string baseRequestUri = ConfigurationManager.AppSettings.Get("DoubanBooksApiEndpoint");
        public static HttpClient httpClient = new HttpClient();

        public async Task<BookDetails> QueryIsbnAsync(Isbn isbn)
        {
            string requestUri = baseRequestUri + isbn.NormalizedValue;
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
                Authors = authors,
                Publisher = book.publisher,
                PublishedDate = book.pubdate,
                Description = book.summary,
                ThumbnailLink = book.images.medium,
                PageCount = pageCount
            };
        }
    }
}
