using System.Collections.Generic;

namespace RaLibrary.Utils
{
    public class Isbn
    {
        private string isbn;

        public Isbn(string isbn)
        {
            if (!IsValid(isbn))
            {
                throw new IsbnFormatException();
            }
            else
            {
                this.isbn = isbn;
            }
        }

        public string NormalizedValue
        {
            get
            {
                return Normalize(isbn);
            }
        }

        private string Normalize(string isbn)
        {
            List<char> digits = new List<char>();

            if (string.IsNullOrWhiteSpace(isbn))
            {
                return string.Empty;
            }
            else
            {
                for (int i = 0; i < isbn.Length; i++)
                {
                    if ((isbn[i] >= '0' && isbn[i] <= '9')
                        || isbn[i] == 'x'
                        || isbn[i] == 'X')
                    {
                        digits.Add(isbn[i]);
                    }
                }

                return string.Join("", digits).ToUpper();
            }
        }

        private bool IsValid(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            {
                return false;
            }

            string normalizedIsbn = Normalize(isbn);
            int length = normalizedIsbn.Length - 1;
            int counter = 0;
            int sum = 0;

            switch (normalizedIsbn.Length)
            {
                case 10:
                    {
                        if (normalizedIsbn[length] == 'x' || normalizedIsbn[length] == 'X')
                        {
                            length -= 1;
                            sum = 10;
                        }

                        counter = 10;
                        for (int i = 0; i <= length; i++)
                        {
                            if (normalizedIsbn[i] < '0' || normalizedIsbn[i] > '9')
                            {
                                continue;
                            }
                            sum += (normalizedIsbn[i] - '0') * counter;
                            counter -= 1;
                        }
                        return sum % 11 == 0; // Divisible by 11.
                    }
                case 13:
                    {
                        const int one = 1, three = 3;
                        counter = one;
                        for (int i = 0; i <= length; i++)
                        {
                            if (normalizedIsbn[i] < '0' || normalizedIsbn[i] > '9')
                            {
                                continue;
                            }
                            sum += (normalizedIsbn[i] - '0') * counter;
                            counter = (counter == one) ? three : one;
                        }
                        return sum % 10 == 0; // Divisible by 10.
                    }
            }

            return false;
        }
    }
}
