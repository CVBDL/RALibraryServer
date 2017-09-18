using System;

namespace RaLibrary.Utilities
{
    public class IsbnFormatException : Exception
    {
        private const string msg = "Invalid ISBN format.";

        public IsbnFormatException() : base(msg) { }

        public IsbnFormatException(string message) : base(message) { }
    }
}
