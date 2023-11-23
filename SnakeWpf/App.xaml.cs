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
            this.MainWindow = new MainWindow();
            this.MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
