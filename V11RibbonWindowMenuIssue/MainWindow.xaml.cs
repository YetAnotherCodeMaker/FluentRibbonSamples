using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace RibbonWindowMenuShowcase;

#if USE_FLUENT_RIBBON

public class WindowBase : Fluent.RibbonWindow
{
    public WindowBase() =>
        Resources.MergedDictionaries.Add(
            new ResourceDictionary()
            {
                Source =
                    new(
                        "pack://application:,,,/Fluent;Component/Themes/Generic.xaml",
                        UriKind.RelativeOrAbsolute)
            });
}

#else

public class WindowBase : Window
{
}

#endif

static partial class NativeMethods
{
    [LibraryImport("User32.dll")]
    internal static partial nint GetSystemMenu(nint window, [MarshalAs(UnmanagedType.Bool)] bool revert);
}

public partial class MainWindow : WindowBase
{
    public static readonly nint Zero = nint.Zero;

    public static readonly DependencyProperty WindowMenuHandleProperty =
        DependencyProperty.Register(
            nameof(WindowMenuHandle),
            typeof(nint),
            typeof(MainWindow));

    public nint WindowMenuHandle
    {
        get => (nint)GetValue(WindowMenuHandleProperty);
        set => SetValue(WindowMenuHandleProperty, value);
    }

    [STAThread]
    public static void Main() =>
        new Application().Run(new MainWindow());

    public MainWindow()
    {
        InitializeComponent();
        Loaded += (sender, e) => UpdateWindowMenuHandle();
    }

    void OnRebuildWindowMenuButtonClick(object sender, RoutedEventArgs e) =>
        RebuildWindowMenu();

    void RebuildWindowMenu()
    {
        var handle = new WindowInteropHelper(this).EnsureHandle();
        _ = NativeMethods.GetSystemMenu(handle, true);

        UpdateWindowMenuHandle();
    }

    void UpdateWindowMenuHandle()
    {
        var window = new WindowInteropHelper(this).EnsureHandle();
        var menu = NativeMethods.GetSystemMenu(window, false);
        Debug.WriteLine($"window handle: {window}");
        Debug.WriteLine($"menu handle: {menu}");
        WindowMenuHandle = menu;
    }
}
