using System;

namespace RaLibrary.Data.Exceptions
{
    public class DbRecordNotFoundException : Exception
    {
        private static readonly string s_defaultMessage = "Record not found from database.";

        public DbRecordNotFoundException() : base(s_defaultMessage) { }

        public DbRecordNotFoundException(string message) : base(message) { }
    }
}
