using RaLibrary.Data.Context;
using RaLibrary.Data.Entities;
using RaLibrary.Data.Exceptions;
using RaLibrary.Data.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace RaLibrary.Data.Managers
{
    public class BookManager
    {
        private RaLibraryContext db = new RaLibraryContext();

        public IQueryable<Book> List()
        {
            return db.Books;
        }

        public async Task<Book> GetAsync(int id)
        {
            Book book = await db.Books.FindAsync(id);
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

            db.Entry(book).State = EntityState.Modified;

            await db.SaveChangesAsync();
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

            db.Entry(book).State = EntityState.Modified;

            await db.SaveChangesAsync();
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

            db.Books.Add(book);

            await db.SaveChangesAsync();

            return await GetAsync(book.Id);
        }

        public async Task DeleteAsync(int id)
        {
            Book book = await GetAsync(id);
            if (book == null)
            {
                throw new DbRecordNotFoundException();
            }

            db.Books.Remove(book);

            await db.SaveChangesAsync();
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

        private bool BookExists(int id)
        {
            return db.Books.Count(book => book.Id == id) > 0;
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
