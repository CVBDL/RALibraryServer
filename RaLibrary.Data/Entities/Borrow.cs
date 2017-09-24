using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaLibrary.Data.Entities
{
    public class Borrow
    {
        [Key]
        public int Id { get; set; }
        [Index(IsUnique = true)]
        [ForeignKey("Book")]
        public int FK_Book_Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Borrower { get; set; }
        [Required]
        public DateTime BorrowTime { get; set; }
        [Required]
        public DateTime ExpectedReturnTime { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Book Book { get; set; }
    }
}
