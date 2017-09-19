using System;

namespace RaLibrary.BookApiProxy.Exceptions
{
    public class BookNotFoundException : Exception
    {
        private static readonly string s_defaultMessage = "Book not found.";

        public BookNotFoundException() : base(s_defaultMessage) { }

        public BookNotFoundException(string message) : base(message) { }
    }
}
