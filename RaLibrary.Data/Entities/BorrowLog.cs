using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaLibrary.Data.Entities
{
    public class BorrowLog
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Book")]
        public int F_BookID { get; set; }
        public virtual Book Book { get; set; }
        [Required]
        [MaxLength(50)]
        public string Borrower { get; set; }
        [Required]
        public DateTime BorrowTime { get; set; }
        public DateTime ExpectedReturnTime { get; set; }
        public DateTime? ReturnTime { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
