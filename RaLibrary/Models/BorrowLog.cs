using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RaLibrary.Models
{
    public class BorrowLog
    {
        public int Id { get; set; }

        [ForeignKey("Book")]
        public int F_BookID { get; set; }

        public virtual Book Book { get; set; }

        [Required]
        [MaxLength(50)]
        public string Borrower { get; set; }

        [Required]
        public DateTime BorrowTime { get; set; }

        public DateTime? ReturnTime { get; set; }
    }
}