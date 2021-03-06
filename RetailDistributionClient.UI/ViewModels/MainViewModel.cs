﻿using MvvmDialogs;
using RetailDistribution.Client.UI.Model;
using RetailDistribution.Client.UI.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace RetailDistribution.Client.UI.ViewModels
{
	public class MainViewModel : INotifyPropertyChanged
	{
		private readonly HttpClient client;
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IDialogService dialogService;

		public MainViewModel(IDialogService dialogService, HttpClient client)
		{
			this.dialogService = dialogService;
			this.client = client;
		}

		#region proxy calls

		/// <summary>
		/// Calls the service endpoint and gets all the available vendors for a particular district
		/// </summary>
		/// <param name="path">The service endpoint path</param>
		/// <returns></returns>
		public async Task<IEnumerable<Vendor>> GetVendorsAsync(string path)
		{
			try
			{
				if (SelectedDistrict != null)
				{
					var districtId = SelectedDistrict.DistrictId;
					var parametrizedPath = $"{path}/getvendors/{districtId}";
					HttpResponseMessage response = await client.GetAsync(parametrizedPath).ConfigureAwait(false);
					if (response.IsSuccessStatusCode)
					{
						Vendors = await response.Content.ReadAsAsync<IList<Vendor>>();
					}
				}

				return Vendors;
			}
			catch (Exception e)
			{
				log.Error(e.ToString());
				return null;
			}
		}

		/// <summary>
		/// Calls the service endpoint and gets all the available districts
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public async Task<IEnumerable<District>> GetDistrictsAsync(string path)
		{
			try
			{
				HttpResponseMessage response = await client.GetAsync(path).ConfigureAwait(false);
				if (response.IsSuccessStatusCode)
				{
					Districts = await response.Content.ReadAsAsync<IList<District>>();
				}

				return Districts;
			}
			catch (Exception e)
			{
				log.Error(e.ToString());
				return null;
			}
		}

		public async Task RefreshVendors()
		{
			await GetVendorsAsync(ServicePaths.DistrictsEndpoint);
			SelectedVendor = null;
		}

		/// <summary>
		/// Refreshes the list of districts and resets the dependent control (vendors, shops)
		/// </summary>
		public async Task RefreshDistricts()
		{
			await GetDistrictsAsync(ServicePaths.DistrictsEndpoint);
			SelectedVendor = null;
			SelectedDistrict = null;
			Vendors = null;
			Shops = null;
		}

		public async Task AddVendor()
		{
			if (SelectedDistrict != null)
			{
				var addVendorViewModel = new AddVendorViewModel(client);
				addVendorViewModel.DistrictId = SelectedDistrict.DistrictId;
				dialogService.ShowDialog<AddVendor>(this, addVendorViewModel);

				await RefreshVendors();
			}
		}

		/// <summary>
		/// Calls the service endpoint and gets all the available districts
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public async Task<bool> SetPrimaryVendorAsync(string path)
		{
			try
			{
				ProcessingSetPrimaryVendor = true;
				if (SelectedDistrict != null && SelectedVendor != null)
				{
					// Get the right object reference (the one bound to the Vendor listbox)
					var originalVendor = Vendors.FirstOrDefault(v => v.VendorId == SelectedDistrict.PrimaryVendor.VendorId);
					SelectedDistrict.PrimaryVendor = SelectedVendor;
					HttpResponseMessage response = await client.PutAsJsonAsync(path, SelectedDistrict).ConfigureAwait(false);
					if (response.IsSuccessStatusCode)
					{
						await RefreshVendors();
						// Operation completed successfully
						return true;
					}
					else
					{
						//If operation failed, set the original district back as primary
						SelectedDistrict.PrimaryVendor = originalVendor;
					}
				}

				return false;
			}
			catch (Exception e)
			{
				log.Error(e.ToString());
				return false;
			}
			finally
			{
				ProcessingSetPrimaryVendor = false;
			}
		}

		/// <summary>
		/// Removes the selected vendor
		/// </summary>
		/// <param name="path">the partial path of the endpoint</param>
		/// <returns>True if successful, false otherwise</returns>
		public async Task<bool> RemoveVendor(string path)
		{
			try
			{
				ProcessingRemoveVendor = true;
				if (SelectedDistrict != null && SelectedVendor != null)
				{
					var parametrizedPath = $"{path}/removeVendor/{SelectedDistrict.DistrictId}/{SelectedVendor.VendorId}";
					HttpResponseMessage response = await client.DeleteAsync(parametrizedPath).ConfigureAwait(false);
					if (response.IsSuccessStatusCode)
					{
						var success = await response.Content.ReadAsAsync<bool>();
						await RefreshVendors();
						return true;
					}
					//Ideally, I would use a different set of error codes for passing error messages to clients
					// but due to lack of time, I'm piggybacking on HTTP's status codes
					else if (response.StatusCode == HttpStatusCode.BadRequest)
					{
						throw new InvalidOperationException("Cannot remove primary vendor. Set another vendor as primary and retry.");
					}
				}

				return false;
			}
			//This one is sent further to the UI
			catch (InvalidOperationException)
			{
				throw;
			}
			catch (Exception e)
			{
				log.Error(e.ToString());
				return false;
			}
			finally
			{
				ProcessingRemoveVendor = false;
			}
		}

		/// <summary>
		/// Calls the service endpoint and gets all the available shops for a particular district
		/// </summary>
		/// <param name="path">The service endpoint path</param>
		/// <returns></returns>
		public async Task<IEnumerable<Shop>> GetShopsAsync(string path)
		{
			try
			{
				if (SelectedDistrict != null)
				{
					var districtId = SelectedDistrict.DistrictId;
					var parametrizedPath = $"{path}/{districtId}";
					HttpResponseMessage response = await client.GetAsync(parametrizedPath).ConfigureAwait(false);
					if (response.IsSuccessStatusCode)
					{
						Shops = await response.Content.ReadAsAsync<IEnumerable<Shop>>();
					}
				}

				return Shops;
			}
			catch (Exception e)
			{
				log.Error(e.ToString());
				return null;
			}
		}

		#endregion proxy calls

		#region INotifyPropertyChanged members

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion INotifyPropertyChanged members

		public District selectedDistrict;
		public District SelectedDistrict
		{
			get { return selectedDistrict; }
			set
			{
				selectedDistrict = value;
				OnPropertyChanged("IsAddVendorEnabled");
				OnPropertyChanged("IsSetPrimaryVendorEnabled");
				OnPropertyChanged("IsRemoveVendorEnabled");
			}
		}

		public Vendor selectedVendor;
		public Vendor SelectedVendor
		{
			get
			{
				return selectedVendor;
			}
			set
			{
				selectedVendor = value;
				OnPropertyChanged("IsRemoveVendorEnabled");
				OnPropertyChanged("IsSetPrimaryVendorEnabled");
			}
		}

		private IList<District> districts;
		public IList<District> Districts
		{
			get
			{
				return districts;
			}
			set
			{
				districts = value;
				OnPropertyChanged("Districts");
			}
		}

		private IList<Vendor> vendors;
		public IList<Vendor> Vendors
		{
			get
			{
				return vendors;
			}
			set
			{
				vendors = value;
				OnPropertyChanged("Vendors");
			}
		}

		private IEnumerable<Shop> shops;
		public IEnumerable<Shop> Shops
		{
			get
			{
				return shops;
			}
			set
			{
				shops = value;
				OnPropertyChanged("Shops");
			}
		}

		public bool IsAddVendorEnabled
		{
			get
			{
				return SelectedDistrict != null;
			}
		}

		public bool processingSetPrimaryVendor;
		public bool ProcessingSetPrimaryVendor
		{
			get
			{
				return processingSetPrimaryVendor;
			}
			set
			{
				processingSetPrimaryVendor = value;
				OnPropertyChanged("IsSetPrimaryVendorEnabled");
			}
		}

		public bool IsSetPrimaryVendorEnabled
		{
			get
			{
				return SelectedDistrict != null && SelectedVendor != null && !ProcessingRemoveVendor;
			}
		}

		public bool processingRemoveVendor;
		public bool ProcessingRemoveVendor
		{
			get
			{
				return processingRemoveVendor;
			}
			set
			{
				processingRemoveVendor = value;
				OnPropertyChanged("IsRemoveVendorEnabled");
			}
		}

		public bool IsRemoveVendorEnabled
		{
			get
			{
				return SelectedDistrict != null && SelectedVendor != null && !ProcessingRemoveVendor;
			}
		}
	}
}
