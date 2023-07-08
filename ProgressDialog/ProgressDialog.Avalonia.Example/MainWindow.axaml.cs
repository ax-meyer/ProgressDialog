using Avalonia.Controls;
using ProgressDialog.Avalonia.Example;

namespace ProgressDialog.Avalonia.Example;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        DataContext = new ProgressDialogExampleViewModel(this);
        InitializeComponent();
    }
}