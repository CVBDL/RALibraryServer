namespace RaLibrary.BookApiProxy.Douban
{
    internal class DoubanBook
    {
        public Rating rating { get; set; }
        public string subtitle { get; set; }
        public string[] author { get; set; }
        public string pubdate { get; set; }
        public Tag[] tags { get; set; }
        public string origin_title { get; set; }
        public string image { get; set; }
        public string binding { get; set; }
        public string[] translator { get; set; }
        public string catalog { get; set; }
        public string pages { get; set; }
        public Images images { get; set; }
        public string alt { get; set; }
        public string id { get; set; }
        public string publisher { get; set; }
        public string isbn10 { get; set; }
        public string isbn13 { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string alt_title { get; set; }
        public string author_intro { get; set; }
        public string summary { get; set; }
        public string price { get; set; }
    }

    internal class Rating
    {
        public int max { get; set; }
        public int numRaters { get; set; }
        public string average { get; set; }
        public int min { get; set; }
    }

    internal class Images
    {
        public string small { get; set; }
        public string large { get; set; }
        public string medium { get; set; }
    }

    internal class Tag
    {
        public int count { get; set; }
        public string name { get; set; }
        public string title { get; set; }
    }
}
