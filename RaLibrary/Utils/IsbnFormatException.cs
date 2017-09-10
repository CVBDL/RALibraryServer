using System;

namespace RaLibrary.Utils
{
    public class IsbnFormatException : Exception
    {
        public IsbnFormatException() { }

        public IsbnFormatException(string message) : base(message) { }
    }
}
