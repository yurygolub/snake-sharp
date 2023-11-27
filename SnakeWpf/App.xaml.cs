using System.Windows;

namespace SnakeWpf
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var viewModel = new MainViewModel();
            this.MainWindow = new MainWindow(viewModel);
            this.MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
