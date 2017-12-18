using System.ComponentModel.DataAnnotations;

namespace RaLibrary.Data.Entities
{
    public class ServiceAccount
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
