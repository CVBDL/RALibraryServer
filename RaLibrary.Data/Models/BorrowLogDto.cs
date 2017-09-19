using System;
using System.ComponentModel.DataAnnotations;

namespace RaLibrary.Data.Models
{
    public class BorrowLogDto
    {
        public int Id { get; set; }
        public int F_BookID { get; set; }
        [Required]
        [MaxLength(50)]
        public string Borrower { get; set; }
        [Required]
        public DateTime BorrowTime { get; set; }
        public DateTime ExpectedReturnTime { get; set; }
        public DateTime? ReturnTime { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
