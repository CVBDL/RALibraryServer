using System;
using System.Runtime.Serialization;

namespace RaLibrary.Controllers
{
    [Serializable]
    internal class BookRecordNotFoundException : Exception
    {
        public BookRecordNotFoundException()
        {
        }

        public BookRecordNotFoundException(string message) : base(message)
        {
        }

        public BookRecordNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BookRecordNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}