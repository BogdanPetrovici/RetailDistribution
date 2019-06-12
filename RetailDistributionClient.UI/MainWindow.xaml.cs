using RetailDistribution.Client.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace RetailDistribution.Client.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.IsEnabled)
            {
                btn.IsEnabled = false;
                await (DataContext as MainViewModel).GetDistrictsAsync(ServicePaths.DistrictsEndpoint);
                btn.IsEnabled = true;
            }

        }

        private async void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            if (listBox.IsEnabled)
            {
                listBox.IsEnabled = false;
                await (DataContext as MainViewModel).GetVendorsAsync(ServicePaths.VendorsEndpoint);
                await (DataContext as MainViewModel).GetShopsAsync(ServicePaths.ShopsEndpoint);
                listBox.IsEnabled = true;
            }
        }
    }
}
