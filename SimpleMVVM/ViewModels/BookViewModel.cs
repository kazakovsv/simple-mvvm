using System;
using System.Windows.Input;
using SimpleMVVM.Commands;
using SimpleMVVM.DataAccess;
using SimpleMVVM.Model;

namespace SimpleMVVM.ViewModels
{
    public class BookViewModel : WorkspaceViewModel
    {
        private readonly Book m_book;
        private readonly BookRepository m_bookRepository;

        public BookViewModel(Book book, BookRepository bookRepository)
        {
            m_book = book
                ?? throw new ArgumentNullException(nameof(book));
            m_bookRepository = bookRepository
                ?? throw new ArgumentNullException(nameof(bookRepository));
        }

        public string Title
        {
            get => m_book.Title;
            set
            {
                if (value == m_book.Title)
                {
                    return;
                }

                m_book.Title = value;
                OnPropertyChanged();
            }
        }

        public string Isbn
        {
            get => m_book.Isbn;
            set
            {
                if (value == m_book.Isbn)
                {
                    return;
                }

                m_book.Isbn = value;
                OnPropertyChanged();
            }
        }

        public string Author
        {
            get => m_book.Author;
            set
            {
                if (value == m_book.Author)
                {
                    return;
                }

                m_book.Author = value;
                OnPropertyChanged();
            }
        }

        public string Publisher
        {
            get => m_book.Publisher;
            set
            {
                if (value == m_book.Publisher)
                {
                    return;
                }

                m_book.Publisher = value;
                OnPropertyChanged();
            }
        }

        public override string DisplayName => Title;

        public bool IsSelected
        {
            get => m_isSelected;
            set
            {
                if (value == m_isSelected)
                {
                    return;
                }

                m_isSelected = value;
                OnPropertyChanged();
            }
        }
        private bool m_isSelected = false;

        public bool IsNewBook => !m_bookRepository.ContainsBook(m_book);

        public ICommand SaveCommand
        {
            get
            {
                if (m_saveCommand == null)
                {
                    m_saveCommand = new DelegateCommand(
                        parameter => OnSaveCommandExecuted(),
                        parameter => CanSaveCommandExecute());
                }

                return m_saveCommand;
            }
        }
        private DelegateCommand m_saveCommand;

        private bool CanSaveCommandExecute()
        {
            return m_book.IsValid;
        }

        private void OnSaveCommandExecuted()
        {
            SaveBook();
        }

        public void SaveBook()
        {
            if (!m_book.IsValid)
            {
                throw new InvalidOperationException("Книга не может быть сохранена");
            }

            if (IsNewBook)
            {
                m_bookRepository.AddBook(m_book);
            }

            OnPropertyChanged(nameof(DisplayName));
        }
    }
}
