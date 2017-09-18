using System;

namespace RaLibrary.Data.Exceptions
{
    public class DbRecordNotFoundException : Exception
    {
        private const string msg = "Record not found from database.";

        public DbRecordNotFoundException() : base(msg) { }

        public DbRecordNotFoundException(string message) : base(message) { }
    }
}
