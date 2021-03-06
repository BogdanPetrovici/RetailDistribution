﻿using RetailDistribution.Client.UI.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RetailDistribution.Client.UI.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class Main : Window
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public Main(MainViewModel viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;
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
			var mainViewModel = DataContext as MainViewModel;
			if (mainViewModel != null && btn.IsEnabled)
			{
				await mainViewModel.SetPrimaryVendorAsync(ServicePaths.DistrictsEndpoint);
			}
		}

		private async void AddVendor_Button_Click(object sender, RoutedEventArgs e)
		{
			var mainViewModel = DataContext as MainViewModel;
			await mainViewModel.AddVendor();
		}

		private async void RemoveVendor_Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Button btn = (Button)sender;
				var mainViewModel = DataContext as MainViewModel;
				if (mainViewModel != null && btn.IsEnabled)
				{
					await mainViewModel.RemoveVendor(ServicePaths.DistrictsEndpoint);
				}
			}
			catch (InvalidOperationException ex)
			{
				log.Error(ex.ToString());
				MessageBox.Show(ex.Message, "Cannot remove vendor", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
