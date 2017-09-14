using System;
using System.ComponentModel.DataAnnotations;

namespace RaLibrary.Data.Entities
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Code { get; set; }
        [MaxLength(10)]
        public string ISBN10 { get; set; }
        [MaxLength(13)]
        public string ISBN13 { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Authors { get; set; }
        public string Publisher { get; set; }
        public string PublishedDate { get; set; }
        public string Description { get; set; }
        public int PageCount { get; set; }
        public string ThumbnailLink { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [MaxLength(50)]
        public string Borrower { get; set; }
    }
}
