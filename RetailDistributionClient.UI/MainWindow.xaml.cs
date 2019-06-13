using RetailDistribution.Client.UI.ViewModels;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RetailDistribution.Client.UI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private HttpClient client;
		public MainWindow()
		{
			InitializeComponent();
			client = new HttpClient();
			// Update port # in the following line.
			client.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["localServerAddress"]);
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(
				new MediaTypeWithQualityHeaderValue("application/json"));
		}

		private async Task RefreshDistricts(Button btn)
		{
			if (btn.IsEnabled)
			{
				btn.IsEnabled = false;
				var mainViewModel = DataContext as MainViewModel;
				await mainViewModel.RefreshDistricts();
				btn.IsEnabled = true;
			}
		}

		private async void Window_Loaded(object sender, RoutedEventArgs e)
		{
			MainViewModel.Client = client;
			await RefreshDistricts(RefreshDistrictsButton);
		}

		private async void RefreshDistricts_Button_Click(object sender, RoutedEventArgs e)
		{
			Button btn = (Button)sender;
			await RefreshDistricts(btn);
		}

		private async void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ListBox listBox = (ListBox)sender;
			if (listBox.IsEnabled)
			{
				listBox.IsEnabled = false;
				await (DataContext as MainViewModel).GetVendorsAsync(ServicePaths.DistrictsEndpoint);
				await (DataContext as MainViewModel).GetShopsAsync(ServicePaths.ShopsEndpoint);
				listBox.IsEnabled = true;
			}
		}

		private async void SetPrimaryVendor_Button_Click(object sender, RoutedEventArgs e)
		{
			Button btn = (Button)sender;
			if (btn.IsEnabled)
			{
				await (DataContext as MainViewModel).SetPrimaryVendorAsync(ServicePaths.DistrictsEndpoint);
			}
		}

		private void AddVendor_Button_Click(object sender, RoutedEventArgs e)
		{
			var mainViewModel = DataContext as MainViewModel;
			if (mainViewModel.SelectedDistrict != null)
			{
				AddVendorViewModel addVendorViewModel = new AddVendorViewModel(client, mainViewModel.SelectedDistrict.DistrictId);
				AddVendorDialog addVendorDialog = new AddVendorDialog();
				addVendorDialog.Owner = this;
				addVendorDialog.DataContext = addVendorViewModel;
				addVendorDialog.ShowDialog();

				mainViewModel.Vendors.Add(addVendorViewModel.SelectedVendor);
			}
		}

		private void RemoveVendor_Button_Click(object sender, RoutedEventArgs e)
		{

		}


	}
}
