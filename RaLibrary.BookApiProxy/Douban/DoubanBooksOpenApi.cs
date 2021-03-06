﻿using Newtonsoft.Json;
using RaLibrary.BookApiProxy.Exceptions;
using RaLibrary.BookApiProxy.Models;
using RaLibrary.Utilities;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace RaLibrary.BookApiProxy.Douban
{
    public class DoubanBooksOpenApi : IBooksOpenApi
    {
        #region Fields

        private static readonly string s_baseRequestUri = ConfigurationManager.AppSettings.Get("DoubanBooksApiEndpoint");
        private static readonly HttpClient s_httpClient = new HttpClient();

        #endregion Fields

        public async Task<BookDetailsDto> QueryIsbnAsync(Isbn isbn)
        {
            string requestUri = s_baseRequestUri + isbn.NormalizedValue;
            HttpResponseMessage response = await s_httpClient.GetAsync(requestUri);

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
            if (book.author!=null && book.author.Length > 0)
            {
                authors = string.Join(",", book.author);
            }

            ushort? pageCount = null;
            if (!string.IsNullOrWhiteSpace(book.pages))
            {
                pageCount = ushort.Parse(book.pages);
            }

            return new BookDetailsDto()
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
