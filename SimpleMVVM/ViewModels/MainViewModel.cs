using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using SimpleMVVM.Commands;
using SimpleMVVM.DataAccess;
using SimpleMVVM.Model;

namespace SimpleMVVM.ViewModels
{
    public class MainViewModel : WorkspaceViewModel
    {
        private readonly BookRepository m_bookRepository;
        private ReadOnlyCollection<CommandViewModel> m_commands;
        private ObservableCollection<WorkspaceViewModel> m_workspaces;

        public MainViewModel(string bookDataFile)
        {
            m_bookRepository = new BookRepository(bookDataFile);
            DisplayName = "Книжный клуб";
        }

        public ReadOnlyCollection<CommandViewModel> Commands
        {
            get
            {
                if (m_commands == null)
                {
                    List<CommandViewModel> commands = CreateCommands();
                    m_commands = new ReadOnlyCollection<CommandViewModel>(commands);
                }

                return m_commands;
            }
        }

        private List<CommandViewModel> CreateCommands()
        {
            return new List<CommandViewModel>
            {
                new CommandViewModel(
                    "Все книги",
                    new DelegateCommand(
                        parameter => ShowAllBooks())),
                new CommandViewModel(
                    "Добавить книгу",
                    new DelegateCommand(
                        parameter => CreateNewBook())),
            };
        }

        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (m_workspaces == null)
                {
                    m_workspaces = new ObservableCollection<WorkspaceViewModel>();
                    m_workspaces.CollectionChanged += OnWorkspacesCollectionChanged;
                }

                return m_workspaces;
            }
        }

        private void OnWorkspacesCollectionChanged(object sender,
            NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
            {
                foreach (WorkspaceViewModel workspace in e.NewItems)
                {
                    workspace.RequestClose += OnWorkspaceRequestClose;
                }
            }

            if (e.OldItems != null && e.OldItems.Count != 0)
            {
                foreach (WorkspaceViewModel workspace in e.OldItems)
                {
                    workspace.RequestClose -= OnWorkspaceRequestClose;
                }
            }
        }

        private void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel workspace = sender as WorkspaceViewModel;
            _ = Workspaces.Remove(workspace);
        }

        private void ShowAllBooks()
        {
            if (!(Workspaces.FirstOrDefault(vm => vm is AllBooksViewModel)
                is AllBooksViewModel workspace))
            {
                workspace = new AllBooksViewModel(m_bookRepository);
                Workspaces.Add(workspace);
            }

            SetActiveWorkspace(workspace);
        }

        private void CreateNewBook()
        {
            Book book = Book.CreateNewBook();
            BookViewModel bookViewModel = new BookViewModel(book, m_bookRepository);
            Workspaces.Add(bookViewModel);
            SetActiveWorkspace(bookViewModel);
        }

        private void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(Workspaces);

            if (collectionView != null)
            {
                _ = collectionView.MoveCurrentTo(workspace);
            }
        }
    }
}
