using System.Windows;
using WPFUserInterface.Helpers;
using WPFUserInterface.ViewModels;

namespace WPFUserInterface
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            NavigationStore navigationStore = new NavigationStore();

            //navigationStore.CurrentViewModel = LoginViewModel(navigationStore);

            MainWindow = new MainWindow()
            {
                DataContext = new ApplicationViewModel(navigationStore)
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
