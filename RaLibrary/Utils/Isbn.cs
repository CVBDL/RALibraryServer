namespace RaLibrary.Utils
{
    public class Isbn
    {
        #region Fields

        private string _isbn;

        private string _normalizedIsbn;

        #endregion Fields

        #region Constructors

        public Isbn(string isbn)
        {
            if (IsValidIsbnTen(isbn) || IsValidIsbnThirteen(isbn))
            {
                _isbn = isbn;
            }
            else
            {
                throw new IsbnFormatException();
            }
        }

        #endregion Constructors

        #region Properties

        public string NormalizedIsbn
        {
            get
            {
                if (_normalizedIsbn == null && !string.IsNullOrWhiteSpace(_isbn))
                {
                    _normalizedIsbn = _isbn.ToUpper();
                }

                return _normalizedIsbn;
            }
        }

        #endregion Properties

        public static bool IsValidIsbn(string isbn)
        {
            return IsValidIsbnTen(isbn) || IsValidIsbnThirteen(isbn);
        }

        public static bool IsValidIsbnTen(string isbn)
        {
            // We'll ignore the null or empty isbn validation.
            if (string.IsNullOrEmpty(isbn))
            {
                return true;
            }
            else
            {
                int length = isbn.Length - 1;
                int counter = 10;
                int sum = 0;
                if (isbn[length] == 'x' || isbn[length] == 'X')
                {
                    length -= 1;
                    sum = 10;
                }

                for (int i = 0; i <= length; i++)
                {
                    if (isbn[i] < '0' || isbn[i] > '9')
                    {
                        return false;
                    }
                    sum += (isbn[i] - '0') * counter;
                    counter -= 1;
                }

                return sum % 11 == 0;
            }
        }

        public static bool IsValidIsbnThirteen(string isbn)
        {
            // We'll ignore the null or empty isbn validation.
            if (string.IsNullOrEmpty(isbn))
            {
                return true;
            }
            else
            {
                int length = isbn.Length - 1;
                int counter = 10;
                int sum = 0;

                const int one = 1, three = 3;
                counter = one;
                for (int i = 0; i <= length; i++)
                {
                    if (isbn[i] < '0' || isbn[i] > '9')
                    {
                        return false;
                    }
                    sum += (isbn[i] - '0') * counter;
                    counter = (counter == one) ? three : one;
                }

                return sum % 10 == 0; // Divisible by 10.
            }
        }
    }
}
