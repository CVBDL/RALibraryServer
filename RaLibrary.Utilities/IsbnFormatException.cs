using System;

namespace RaLibrary.Utilities
{
    public class IsbnFormatException : Exception
    {
        private static readonly string s_defaultMessage = "Invalid ISBN format.";

        public IsbnFormatException() : base(s_defaultMessage) { }

        public IsbnFormatException(string message) : base(message) { }
    }
}
