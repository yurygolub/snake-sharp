using System;
using System.Windows;
using System.Windows.Input;

namespace SnakeWpf;

/// <summary>
/// Interaction logic for MainWindow.xaml.
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        this.ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        this.InitializeComponent();
    }

    public MainViewModel ViewModel { get; }

    private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
    {
        this.Close();
    }
}
