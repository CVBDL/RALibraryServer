using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaLibrary.Data.Exceptions
{
    class BookRecordNotFoundException : Exception
    {
        public BookRecordNotFoundException() { }

        public BookRecordNotFoundException(string message) : base(message) { }
    }
}
