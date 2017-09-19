using RaLibrary.Data.Context;
using RaLibrary.Data.Entities;
using RaLibrary.Data.Exceptions;
using RaLibrary.Data.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace RaLibrary.Data.Managers
{
    public class BookManager
    {
        private RaLibraryContext _db = new RaLibraryContext();

        public IQueryable<Book> List()
        {
            return _db.Books;
        }

        public async Task<Book> GetAsync(int id)
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

        public async Task UpdateAsync(int id, BookDto bookDto)
        {
            if (!BookExists(id))
            {
                throw new DbRecordNotFoundException();
            }

            Book book = await GetAsync(id);

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
            book.Borrower = bookDto.Borrower;
            book.RowVersion = bookDto.RowVersion;

            _db.Entry(book).State = EntityState.Modified;

            await SaveChangesAsync();
        }

        public async Task UpdateBorrowerAsync(BookDto bookDto)
        {
            if (!BookExists(bookDto.Id))
            {
                throw new DbRecordNotFoundException();
            }

            Book book = await GetAsync(bookDto.Id);

            book.Borrower = bookDto.Borrower;
            book.RowVersion = bookDto.RowVersion;

            _db.Entry(book).State = EntityState.Modified;

            await SaveChangesAsync();
        }

        public async Task<Book> CreateAsync(BookDto bookDto)
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
                Borrower = bookDto.Borrower,
                CreatedDate = DateTime.UtcNow
            };

            _db.Books.Add(book);

            await SaveChangesAsync();

            return await GetAsync(book.Id);
        }

        public async Task DeleteAsync(int id)
        {
            Book book = await GetAsync(id);
            if (book == null)
            {
                throw new DbRecordNotFoundException();
            }

            _db.Books.Remove(book);

            await SaveChangesAsync();
        }

        public BookDto ToDto(Book book)
        {
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
                Borrower = book.Borrower,
                RowVersion = book.RowVersion
            };
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        private bool BookExists(int id)
        {
            return _db.Books.Count(book => book.Id == id) > 0;
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
    }
}
