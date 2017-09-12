using System.ComponentModel.DataAnnotations;

namespace RaLibrary.Utils
{
    public class IsbnTenAttribute : ValidationAttribute
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

                return isbn.Type == IsbnType.Ten;
            }
            catch (IsbnFormatException)
            {
                return false;
            }
        }
    }
}
