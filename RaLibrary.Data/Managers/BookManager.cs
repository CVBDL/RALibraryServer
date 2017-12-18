using RaLibrary.Data.Context;
using RaLibrary.Data.Entities;
using RaLibrary.Data.Exceptions;
using RaLibrary.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace RaLibrary.Data.Managers
{
    public class BookManager : IBookManager
    {
        #region Fields

        private RaLibraryContext _db = new RaLibraryContext();

        #endregion Fields

        /// <summary>
        /// List all books from data store.
        /// </summary>
        /// <returns></returns>
        public IQueryable<BookDto> List()
        {
            List<Book> books = _db.Books.ToList();

            var result = new List<BookDto>();
            foreach (Book book in books)
            {
                result.Add(ToDto(book));
            }

            return result.AsQueryable();
        }

        /// <summary>
        /// List all books from data store.
        /// </summary>
        /// <returns></returns>
        public async Task<IQueryable<BookDto>> ListAsync()
        {
            List<Book> books = await _db.Books.ToListAsync();

            var result = new List<BookDto>();
            foreach (Book book in books)
            {
                result.Add(ToDto(book));
            }

            return result.AsQueryable();
        }

        /// <summary>
        /// Get a single book from data store.
        /// </summary>
        /// <param name="id">The book id.</param>
        /// <returns></returns>
        public async Task<BookDto> GetAsync(int id)
        {
            Book book = await FindAsync(id);

            return ToDto(book);
        }

        /// <summary>
        /// Update an existing book.
        /// </summary>
        /// <param name="bookDto"></param>
        /// <returns></returns>
        public async Task<BookDto> UpdateAsync(BookDto bookDto)
        {
            Book book = await FindAsync(bookDto.Id);

            book.Code = bookDto.Code;
            book.ISBN10 = bookDto.ISBN10;
            book.ISBN13 = bookDto.ISBN13;
            book.Title = bookDto.Title;
            book.Subtitle = bookDto.Subtitle;
            book.Authors = bookDto.Authors;
            book.Publisher = bookDto.Publisher;
            book.PublishedDate = bookDto.PublishedDate;
            book.Description = bookDto.Description;
            book.PageCount = bookDto.PageCount;
            book.ThumbnailLink = bookDto.ThumbnailLink;

            ModifyDbEntityEntry(book, bookDto.RowVersion);

            await SaveChangesAsync();

            return await GetAsync(book.Id);
        }

        /// <summary>
        /// Create a book.
        /// </summary>
        /// <param name="bookDto"></param>
        /// <returns></returns>
        public async Task<BookDto> CreateAsync(BookDto bookDto)
        {
            Book book = new Book()
            {
                Code = bookDto.Code,
                ISBN10 = bookDto.ISBN10,
                ISBN13 = bookDto.ISBN13,
                Title = bookDto.Title,
                Subtitle = bookDto.Subtitle,
                Authors = bookDto.Authors,
                Publisher = bookDto.Publisher,
                PublishedDate = bookDto.PublishedDate,
                Description = bookDto.Description,
                PageCount = bookDto.PageCount,
                ThumbnailLink = bookDto.ThumbnailLink,
                CreatedDate = DateTime.UtcNow
            };

            _db.Books.Add(book);

            await SaveChangesAsync();

            return await GetAsync(book.Id);
        }

        /// <summary>
        /// Delete an existing book.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            Book book = await FindAsync(id);
            if (book == null)
            {
                throw new DbRecordNotFoundException();
            }

            _db.Books.Remove(book);

            await SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        private async Task<Book> FindAsync(int id)
        {
            Book book = await _db.Books.FindAsync(id);
            if (book == null)
            {
                throw new DbRecordNotFoundException();
            }
            else
            {
                return book;
            }
        }

        public async Task<IQueryable<BookDto>> QueryAsync(IEnumerable<string> keywords)
        {
            return await Task.Run(() =>
            {
                var result = new List<BookDto>();
                var books = _db.Books.ToList();
                foreach (var keyword in keywords)
                {
                    books = Filter(books, keyword);
                }
                books.ForEach(i => result.Add(ToDto(i)));
                return result.AsQueryable();
            });
        }

        private List<Book> Filter(List<Book> books, string keyword)
        {
            var re = books.Where(i =>
                (!string.IsNullOrWhiteSpace(i.Code) && i.Code.ToLower().Contains(keyword)) ||
                (!string.IsNullOrWhiteSpace(i.ISBN10) && i.ISBN10.ToLower().Contains(keyword)) ||
                (!string.IsNullOrWhiteSpace(i.ISBN13) && i.ISBN13.ToLower().Contains(keyword)) ||
                (!string.IsNullOrWhiteSpace(i.Title) && i.Title.ToLower().Contains(keyword)) ||
                (!string.IsNullOrWhiteSpace(i.Subtitle) && i.Subtitle.ToLower().Contains(keyword)) ||
                (!string.IsNullOrWhiteSpace(i.Authors) && i.Authors.ToLower().Contains(keyword)) ||
                (!string.IsNullOrWhiteSpace(i.Publisher) && i.Publisher.ToLower().Contains(keyword)) ||
                (!string.IsNullOrWhiteSpace(i.Description) && i.Description.ToLower().Contains(keyword))
            ).ToList();
            return re;
        }

        private void ModifyDbEntityEntry(Book book)
        {
            _db.Entry(book).State = EntityState.Modified;
        }

        private void ModifyDbEntityEntry(Book book, byte[] rowVersion)
        {
            DbEntityEntry<Book> dbEntry = _db.Entry(book);

            // Enable database update concurrency checking.
            dbEntry.Property(b => b.RowVersion).OriginalValue = rowVersion;
            dbEntry.Property(b => b.RowVersion).IsModified = true;

            dbEntry.State = EntityState.Modified;
        }

        private async Task SaveChangesAsync()
        {
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbOperationException("Concurrency updating conflicts detected.");
            }
            catch (DbEntityValidationException)
            {
                throw new DbOperationException("Validation of database property values failed.");
            }
            catch
            {
                throw new DbOperationException();
            }
        }

        private BookDto ToDto(Book book)
        {
            bool isBorrowed = _db.Borrows
                .Count(b => b.FK_Book_Id == book.Id) > 0;

            return new BookDto()
            {
                Id = book.Id,
                Code = book.Code,
                ISBN10 = book.ISBN10,
                ISBN13 = book.ISBN13,
                Title = book.Title,
                Subtitle = book.Subtitle,
                Authors = book.Authors,
                Publisher = book.Publisher,
                PublishedDate = book.PublishedDate,
                Description = book.Description,
                PageCount = book.PageCount,
                ThumbnailLink = book.ThumbnailLink,
                CreatedDate = book.CreatedDate,
                IsBorrowed = isBorrowed,
                RowVersion = book.RowVersion
            };
        }
    }
}
