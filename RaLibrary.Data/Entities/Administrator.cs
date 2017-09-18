using System.ComponentModel.DataAnnotations;

namespace RaLibrary.Data.Entities
{
    public class Administrator
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
    }
}
