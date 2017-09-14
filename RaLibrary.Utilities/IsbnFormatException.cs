using System;

namespace RaLibrary.Utilities
{
    public class IsbnFormatException : Exception
    {
        public IsbnFormatException() { }

        public IsbnFormatException(string message) : base(message) { }
    }
}
