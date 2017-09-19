using System;

namespace RaLibrary.Data.Exceptions
{
    public class DbOperationException : Exception
    {
        private static readonly string s_defaultMessage = "An error occurred when operating database.";

        public DbOperationException() : base(s_defaultMessage) { }

        public DbOperationException(string message) : base(message) { }
    }
}
