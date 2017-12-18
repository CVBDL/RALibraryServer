using RaLibrary.Data.Context;
using RaLibrary.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace RaLibrary.Data.Managers
{
    public class ReportManager : IReportManager
    {
        private RaLibraryContext _db = new RaLibraryContext();

        public IEnumerable<BookStateDto> GetAllBooksStatusReport()
        {
            var books = new List<BookStateDto>();
            foreach (var book in _db.Books)
            {
                var borrow = _db.Borrows.FirstOrDefault(i => i.FK_Book_Id == book.Id);
                var bookInfo = new BookStateDto
                {
                    Code = book.Code,
                    Name = book.Title,
                    Status = borrow == null ? BookStatus.Available : BookStatus.Borrowed,
                    Borrower = borrow == null ? string.Empty : borrow.Borrower,
                    BorrowedDate = borrow == null ? string.Empty : borrow.BorrowTime.ToShortDateString(),
                    ExpectedReturnDate = borrow == null ? string.Empty : borrow.BorrowTime.AddMonths(3).ToShortDateString()
                };
                books.Add(bookInfo);
            }
            return books;
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
