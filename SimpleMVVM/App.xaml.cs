using System.Windows;
using SimpleMVVM.ViewModels;
using SimpleMVVM.Views;

namespace SimpleMVVM
{
    public partial class App : Application
    {
        private readonly string m_path = "Data/books.xml";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainView mainView = new MainView();
            MainViewModel mainViewModel = new MainViewModel(m_path);
            mainView.DataContext = mainViewModel;
            mainViewModel.RequestClose += (sender, e) => mainView.Close();
            mainView.Show();
        }
    }
}
