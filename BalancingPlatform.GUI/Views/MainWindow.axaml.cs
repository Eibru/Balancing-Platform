using Avalonia.Controls;
using BalancingPlatform.GUI.ViewModels;

namespace BalancingPlatform.GUI;
public partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();
    }

    private void XamlLineSeries_ActualThemeVariantChanged(object? sender, System.EventArgs e) {
    }

    private void Image_Tapped(object? sender, Avalonia.Input.TappedEventArgs e) {
        var vm = (MainWindowViewModel)this.DataContext;

        var pos = e.GetPosition(sender as Image);

        var image = sender as Image;
        var imgWidth = (sender as Image).DesiredSize.Width;
        var imgHeight = (sender as Image).DesiredSize.Height;

        var x = (pos.X * 100) / imgWidth;
        var y = (pos.Y * 100) / imgHeight;

        vm.ChangeSetpointAbsoluteCommand.Execute((x, y));
    }
}