using System.ComponentModel.DataAnnotations;

namespace RaLibrary.Models
{
    public class Administrator
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
    }
}
