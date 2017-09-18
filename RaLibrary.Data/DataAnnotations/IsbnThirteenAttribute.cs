using RaLibrary.Utilities;
using System.ComponentModel.DataAnnotations;

namespace RaLibrary.Data.DataAnnotations
{
    public class IsbnThirteenAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            string strIsbn = value as string;
            if (strIsbn == null)
            {
                return false;
            }

            try
            {
                Isbn isbn = new Isbn(strIsbn);

                return isbn.Type == IsbnType.Thirteen;
            }
            catch (IsbnFormatException)
            {
                return false;
            }
        }
    }
}
