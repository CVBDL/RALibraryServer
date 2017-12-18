using System;

namespace RaLibrary.Data.Models
{
    public class BookStateDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public BookStatus Status { get; set; }

        public string Borrower { get; set; }

        public string BorrowedDate { get; set; }

        public string ExpectedReturnDate { get; set; }
    }

    public enum BookStatus
    {
        Available,
        Borrowed,
    }
}
