using RaLibrary.Utils;
using System.ComponentModel.DataAnnotations;

namespace RaLibrary.Models
{
    public class Book
    {
        private string _isbn10;

        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Code { get; set; }
        [IsbnTen]
        public string ISBN10
        {
            get
            {
                return _isbn10;
            }
            set
            {
                if (value != null)
                {
                    _isbn10 = value.ToUpper();
                }
                else
                {
                    _isbn10 = null;
                }
            }
        }
        [IsbnThirteen]
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
        [MaxLength(50)]
        public string Borrower { get; set; }
    }
}
