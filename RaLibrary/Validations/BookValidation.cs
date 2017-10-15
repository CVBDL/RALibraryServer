using RaLibrary.Data.Managers;
using RaLibrary.Data.Models;
using RaLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaLibrary.Validations
{
    public class BookValidation
    {
        private IBookManager _bookMgr;

        public BookValidation()
        {
            //if (bookMgr == null)
            //    throw new ArgumentNullException("book manager shouldn't be null");

            _bookMgr = new BookManager();
        }

        public bool ValidateCreate(BookDto book, out string message)
        {
            message = string.Empty;
            if (!ValidateCode(book, out message))
                return false;

            if (!ValidateIsbn(book, out message))
                return false;

            if (!ValidateTitle(book, out message))
                return false;

            return true;
        }

        public bool ValidateUpdate(BookDto book, out string message)
        {
            message = string.Empty;

            if (!ValidateIsbn(book, out message))
                return false;

            if (!ValidateTitle(book, out message))
                return false;

            return true;
        }

        public bool ValidateCode(BookDto book, out string message)
        {
            var result = true;
            message = string.Empty;
            if (book == null)
            {
                message = "book data should not be null";
                return false;
            }

            if (string.IsNullOrWhiteSpace(book.Code))
            {
                message = "Book code can't be empty";
                return false;
            }

            var existed = _bookMgr.List().Any(i => i.Code == book.Code);
            if (existed)
            {
                result = false;
                message = string.Format("The book code {0} is already existed", book.Code);
            }
            return result;
        }

        public bool ValidateIsbn(BookDto book, out string message)
        {
            var result = false;
            message = string.Empty;

            if ((!string.IsNullOrWhiteSpace(book.ISBN10) && Isbn.IsValidIsbnTen(book.ISBN10)) ||
                (string.IsNullOrWhiteSpace(book.ISBN10) && Isbn.IsValidIsbnThirteen(book.ISBN13)))
            {
                result = true;
            }
            else
            {
                message = "ISBN is invalid";
            }
            return result;
        }

        public bool ValidateTitle(BookDto book, out string message)
        {
            var result = true;
            message = string.Empty;

            if (string.IsNullOrWhiteSpace(book.Title))
            {
                result = false;
                message = "Book title can't be empty";
            }
            return result;
        }
    }
}