using RaLibrary.Data.Entities;
using System;

namespace RaLibrary.Data.Models
{
    public class BorrowDto
    {
        public string Borrower { get; set; }
        public DateTime BorrowTime { get; set; }
        public DateTime ExpectedReturnTime { get; set; }
        public Book Book { get; set; }
    }
}
