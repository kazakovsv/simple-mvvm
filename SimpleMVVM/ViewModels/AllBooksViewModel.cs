using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SimpleMVVM.DataAccess;

namespace SimpleMVVM.ViewModels
{
    public class AllBooksViewModel : WorkspaceViewModel
    {
        private readonly BookRepository m_bookRepository;

        public AllBooksViewModel(BookRepository bookRepository)
        {
            m_bookRepository = bookRepository
                ?? throw new ArgumentNullException(nameof(bookRepository));
            m_bookRepository.BookAdded += OnBookAdded;

            DisplayName = "Все книги";
            CreateAllBooks();
        }

        public ObservableCollection<BookViewModel> AllBooks { get; private set; }

        private void CreateAllBooks()
        {
            List<BookViewModel> books = m_bookRepository.GetBooks()
                .Select(book => new BookViewModel(book, m_bookRepository))
                .ToList();

            AllBooks = new ObservableCollection<BookViewModel>(books);
        }

        private void OnBookAdded(object sender, BookAddedEventArgs e)
        {
            BookViewModel bookViewModel = new BookViewModel(e.NewBook, m_bookRepository);
            AllBooks.Add(bookViewModel);
        }
    }
}
