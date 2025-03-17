using System.Windows;

namespace RibbonToggleButtonsShowcase;

public enum Options { A, B, C, D }

public partial class MainWindow
{
    [STAThread]
    public static void Main() =>
        new Application().Run(new MainWindow());

    public MainWindow() =>
        InitializeComponent();
}
