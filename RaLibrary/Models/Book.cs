using System.ComponentModel.DataAnnotations;

namespace RaLibrary.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Title { get; set; }

        public string Borrower { get; set; }
    }
}
