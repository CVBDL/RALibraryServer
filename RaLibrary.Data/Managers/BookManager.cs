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

        public async Task<Book> Get(int id)
        {
            return await db.Books.FindAsync(id);
        }

        public async Task Update(int id, BookDTO bookDto)
        {
            if (!BookExists(id))
            {
                throw new BookRecordNotFoundException();
            }

            Book book = await Get(id);

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

            db.Entry(book).State = EntityState.Modified;

            await db.SaveChangesAsync();
        }

        private bool BookExists(int id)
        {
            return db.Books.Count(book => book.Id == id) > 0;
        }
    }
}
