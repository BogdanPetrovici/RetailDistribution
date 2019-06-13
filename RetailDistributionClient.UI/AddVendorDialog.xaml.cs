using RetailDistribution.Client.UI.ViewModels;
using System.Windows;

namespace RetailDistribution.Client.UI
{
	/// <summary>
	/// Interaction logic for AddVendorDialog.xaml
	/// </summary>
	public partial class AddVendorDialog : Window
	{
		public AddVendorDialog()
		{
			InitializeComponent();
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			await (DataContext as AddVendorViewModel).AddVendor(ServicePaths.VendorsEndpoint);
			Close();
		}

		private async void Window_Loaded(object sender, RoutedEventArgs e)
		{
			var viewModel = DataContext as AddVendorViewModel;
			if (viewModel != null)
			{
				await viewModel.GetUnusedVendorsAsync(ServicePaths.DistrictsEndpoint, viewModel.DistrictId);
			}
		}
	}
}
