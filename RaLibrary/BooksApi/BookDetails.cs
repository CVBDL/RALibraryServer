namespace RaLibrary.BooksApi
{
    public class BookDetails
    {
        public string ISBN10 { get; set; }
        public string ISBN13 { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Authors { get; set; }
        public string Publisher { get; set; }
        public string PublishedDate { get; set; }
        public string Description { get; set; }
        public string ThumbnailLink { get; set; }
        public ushort? PageCount { get; set; }
    }
}
