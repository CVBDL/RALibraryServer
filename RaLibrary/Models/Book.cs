using System.ComponentModel.DataAnnotations;

namespace RaLibrary.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string CustomId { get; set; }

        [Required]
        public string Title { get; set; }
    }
}
