using System;
using System.ComponentModel.DataAnnotations;

namespace RaLibrary.Data.Entities
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string Code { get; set; }
        [MaxLength(10), MinLength(10)]
        public string ISBN10 { get; set; }
        [MaxLength(13), MinLength(13)]
        public string ISBN13 { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [MaxLength(200)]
        public string Subtitle { get; set; }
        [MaxLength(100)]
        public string Authors { get; set; }
        [MaxLength(100)]
        public string Publisher { get; set; }
        [MaxLength(30)]
        public string PublishedDate { get; set; }
        public string Description { get; set; }
        public int PageCount { get; set; }
        [MaxLength(200)]
        public string ThumbnailLink { get; set; }
        public DateTime CreatedDate { get; set; }
        [MaxLength(50)]
        public string Borrower { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
