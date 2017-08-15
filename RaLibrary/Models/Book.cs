using System.ComponentModel.DataAnnotations;

namespace RaLibrary.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(50)]
        public string Borrower { get; set; }
    }
}
