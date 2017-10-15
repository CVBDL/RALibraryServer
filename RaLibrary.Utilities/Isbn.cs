namespace RaLibrary.Utilities
{
    public class Isbn
    {
        #region Fields

        private string _isbn;

        private string _normalizedValue;
        private IsbnType _type;

        #endregion Fields

        #region Constructors

        public Isbn(string isbn)
        {
            _isbn = isbn;


            if (IsValidIsbn(_isbn))
            {
                throw new IsbnFormatException();
            }
        }

        #endregion Constructors

        #region Properties

        public string NormalizedValue
        {
            get
            {
                if (_normalizedValue == null && !string.IsNullOrWhiteSpace(_isbn))
                {
                    _normalizedValue = _isbn.ToUpper();
                }

                return _normalizedValue;
            }
        }

        public IsbnType Type
        {
            get
            {
                if (NormalizedValue == null)
                {
                    _type = IsbnType.None;
                    return _type;
                }

                switch (NormalizedValue.Length)
                {
                    case 10:
                        _type = IsbnType.Ten;
                        break;
                    case 13:
                        _type = IsbnType.Thirteen;
                        break;
                    default:
                        _type = IsbnType.None;
                        break;
                }

                return _type;
            }
        }

        #endregion Properties

        public static bool IsValidIsbn(string isbn)
        {
            return IsValidIsbnTen(isbn) && IsValidIsbnThirteen(isbn);
        }

        public static bool IsValidIsbnTen(string isbn)
        {
            // We'll ignore ISBN validation if given null
            if (isbn == null)
            {
                return true;
            }
            else
            {
                int length = isbn.Length - 1;
                if (length < 0)
                {
                    return false;
                }

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
            // We'll ignore ISBN validation if given null
            if (isbn == null)
            {
                return true;
            }
            else
            {
                int length = isbn.Length - 1;
                if (length < 0)
                {
                    return false;
                }

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
