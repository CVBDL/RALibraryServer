using RaLibrary.Utilities;
using System;
using System.ComponentModel.DataAnnotations;

namespace RaLibrary.Data.Models
{
    public class BookDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string Code { get; set; }
        [IsbnTen]
        public string ISBN10 { get; set; }
        [IsbnThirteen]
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
        public byte[] RowVersion { get; set; }
    }
}
