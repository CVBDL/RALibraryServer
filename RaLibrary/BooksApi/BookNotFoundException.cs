using System;

namespace RaLibrary.BooksApi
{
    public class BookNotFoundException : Exception
    {
        public BookNotFoundException() { }

        public BookNotFoundException(string message) : base(message) { }
    }
}
