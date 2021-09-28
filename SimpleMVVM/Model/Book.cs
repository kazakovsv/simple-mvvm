using System;
using System.ComponentModel;

namespace SimpleMVVM.Model
{
    public class Book : IDataErrorInfo
    {
        #region Constructor

        protected Book()
        {
        }

        #endregion // Constructor

        #region Creation

        public static Book CreateNewBook()
        {
            return CreateBook(
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                DateTime.Now);
        }

        public static Book CreateBook(
            string title,
            string isbn,
            string author,
            string publisher,
            DateTime publishedYear)
        {
            return new Book
            {
                Title = title,
                Isbn = isbn,
                Author = author,
                Publisher = publisher,
                PublishedYear = publishedYear
            };
        }

        #endregion // Creation

        #region Properties

        public string Title { get; set; }

        public string Isbn { get; set; }

        public string Author { get; set; }

        public string Publisher { get; set; }

        public DateTime PublishedYear { get; set; }

        #endregion // Properties

        #region IDataErrorInfo Members

        public string Error => null;

        public string this[string propertyName] => Validate(propertyName);

        #endregion // IDataErrorInfo Members

        #region Validation

        private const string StringMissingError = "Значение не может быть пустым";

        private static readonly string[] m_validatedProperties =
        {
            nameof(Title),
            nameof(Isbn),
            nameof(Author),
            nameof(Publisher)
        };

        public bool IsValid
        {
            get
            {
                foreach (string propertyName in m_validatedProperties)
                {
                    if (Validate(propertyName) != null)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private string Validate(string propertyName)
        {
            if (Array.IndexOf(m_validatedProperties, propertyName) < 0)
            {
                return null;
            }

            string error = propertyName switch
            {
                nameof(Title) => ValidateTitle(),
                nameof(Isbn) => ValidateIsbn(),
                nameof(Author) => ValidateAuthor(),
                nameof(Publisher) => ValidatePublisher(),
                _ => throw new ArgumentOutOfRangeException(propertyName),
            };

            return error;
        }

        private string ValidateTitle()
        {
            return IsStringMissing(Title) ? StringMissingError : null;
        }

        private string ValidateIsbn()
        {
            return IsStringMissing(Isbn) ? StringMissingError : null;
        }

        private string ValidateAuthor()
        {
            return IsStringMissing(Author) ? StringMissingError : null;
        }

        private string ValidatePublisher()
        {
            return IsStringMissing(Publisher) ? StringMissingError : null;
        }

        private static bool IsStringMissing(string value)
        {
            return string.IsNullOrEmpty(value) || value.Trim() == string.Empty;
        }

        #endregion // Validation
    }
}
