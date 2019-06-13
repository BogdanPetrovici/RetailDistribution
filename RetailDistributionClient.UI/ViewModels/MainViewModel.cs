using RetailDistribution.Client.UI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace RetailDistribution.Client.UI.ViewModels
{
	public class MainViewModel : INotifyPropertyChanged
	{
		public static HttpClient Client { get; set; }

		public MainViewModel()
		{

		}

		/// <summary>
		/// Calls the service endpoint and gets all the available vendors for a particular district
		/// </summary>
		/// <param name="path">The service endpoint path</param>
		/// <returns></returns>
		public async Task<IEnumerable<Vendor>> GetVendorsAsync(string path)
		{
			try
			{
				var districtId = SelectedDistrict?.DistrictId;
				var parametrizedPath = $"{path}/getvendors/{districtId}";
				HttpResponseMessage response = await Client.GetAsync(parametrizedPath).ConfigureAwait(false);
				if (response.IsSuccessStatusCode)
				{
					Vendors = await response.Content.ReadAsAsync<IList<Vendor>>();
				}

				return Vendors;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
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
				HttpResponseMessage response = await Client.GetAsync(path).ConfigureAwait(false);
				if (response.IsSuccessStatusCode)
				{
					Districts = await response.Content.ReadAsAsync<IList<District>>();
				}

				return Districts;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return null;
			}
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

		/// <summary>
		/// Calls the service endpoint and gets all the available districts
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public async Task<bool> SetPrimaryVendorAsync(string path)
		{
			try
			{
				if (SelectedDistrict != null && SelectedVendor != null)
				{
					// Get the right object reference (the one bound to the Vendor listbox)
					var originalVendor = Vendors.FirstOrDefault(v => v.VendorId == SelectedDistrict.PrimaryVendor.VendorId);
					SelectedDistrict.PrimaryVendor = SelectedVendor;
					HttpResponseMessage response = await Client.PutAsJsonAsync(path, SelectedDistrict).ConfigureAwait(false);
					if (response.IsSuccessStatusCode)
					{
						originalVendor.IsPrimary = false;
						SelectedVendor.IsPrimary = true;
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
				Console.WriteLine(e.Message);
				return false;
			}
		}

		public async Task<bool> AddVendor(string path, Vendor vendorTransferObject)
		{
			try
			{
				if (SelectedDistrict != null)
				{
					var parametrizedPath = $"{path}";
					vendorTransferObject.Districts = new List<District> { SelectedDistrict };
					HttpResponseMessage response = await Client.PostAsJsonAsync(path, vendorTransferObject).ConfigureAwait(false);
					if (response.IsSuccessStatusCode)
					{
						var success = await response.Content.ReadAsAsync<bool>();
						Vendors.Add(vendorTransferObject);
						return true;
					}
				}

				return false;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return false;
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
				var districtId = SelectedDistrict?.DistrictId;
				var parametrizedPath = $"{path}/{districtId}";
				HttpResponseMessage response = await Client.GetAsync(parametrizedPath).ConfigureAwait(false);
				if (response.IsSuccessStatusCode)
				{
					Shops = await response.Content.ReadAsAsync<IEnumerable<Shop>>();
				}

				return Shops;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return null;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public District selectedDistrict;
		public District SelectedDistrict
		{
			get { return selectedDistrict; }
			set
			{
				selectedDistrict = value;
				OnPropertyChanged("IsAddVendorEnabled");
				OnPropertyChanged("IsSetPrimaryVendorEnabled");
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

		public bool IsSetPrimaryVendorEnabled
		{
			get
			{
				return SelectedDistrict != null;
			}
		}

		public bool IsRemoveVendorEnabled
		{
			get
			{
				return SelectedVendor != null;
			}
		}
	}
}
