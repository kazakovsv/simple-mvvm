using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Resources;
using System.Xml;
using System.Xml.Linq;
using SimpleMVVM.Model;

namespace SimpleMVVM.DataAccess
{
    public class BookRepository
    {
        #region Fields

        private readonly List<Book> m_books;

        #endregion // Fields

        #region Constructor

        public BookRepository(string bookDataFile)
        {
            m_books = LoadBooks(bookDataFile);
        }

        #endregion // Constructor

        #region Public Interface

        public event EventHandler<BookAddedEventArgs> BookAdded;

        private void RaiseBookAddedEvent(Book newBook)
        {
            BookAdded?.Invoke(this, new BookAddedEventArgs(newBook));
        }

        public void AddBook(Book book)
        {
            if (book is null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            if (!ContainsBook(book))
            {
                m_books.Add(book);
                RaiseBookAddedEvent(book);
            }
        }

        public bool ContainsBook(Book book)
        {
            return book is null
                ? throw new ArgumentNullException(nameof(book))
                : m_books.Contains(book);
        }

        public List<Book> GetBooks()
        {
            return new List<Book>(m_books);
        }

        #endregion // Public Interface

        #region Private Helpers

        private static List<Book> LoadBooks(string bookDataFile)
        {
            using Stream stream = GetResourceStream(bookDataFile);
            using XmlReader reader = new XmlTextReader(stream);

            return (from bookElement in XDocument.Load(reader)
                    .Element("books")
                    .Elements("book")
                    select Book.CreateBook(
                        (string)bookElement.Attribute("title"),
                        (string)bookElement.Attribute("isbn"),
                        (string)bookElement.Attribute("author"),
                        (string)bookElement.Attribute("publisher"),
                        (DateTime)bookElement.Attribute("published_year")
                        )).ToList();
        }

        private static Stream GetResourceStream(string resourceFile)
        {
            Uri uri = new Uri(resourceFile, UriKind.RelativeOrAbsolute);
            StreamResourceInfo info = Application.GetResourceStream(uri);

            return info == null || info.Stream == null
                ? throw new ApplicationException("Отсутствует файл ресурсов: " + resourceFile)
                : info.Stream;
        }

        #endregion // Private Helpers
    }
}
