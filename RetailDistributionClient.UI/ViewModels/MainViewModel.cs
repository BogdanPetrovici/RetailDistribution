using RetailDistribution.Client.UI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace RetailDistribution.Client.UI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private static HttpClient client;

        public MainViewModel()
        {
            client = new HttpClient();
            // Update port # in the following line.
            client.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["localServerAddress"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
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
                var parametrizedPath = $"{path}/{districtId}";
                HttpResponseMessage response = await client.GetAsync(parametrizedPath).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    Vendors = await response.Content.ReadAsAsync<IEnumerable<Vendor>>();
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
                HttpResponseMessage response = await client.GetAsync(path).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    Districts = await response.Content.ReadAsAsync<IEnumerable<District>>();
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
                HttpResponseMessage response = await client.GetAsync(parametrizedPath).ConfigureAwait(false);
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

        public District SelectedDistrict { get; set; }

        private IEnumerable<District> districts;

        public IEnumerable<District> Districts
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

        private IEnumerable<Vendor> vendors;

        public IEnumerable<Vendor> Vendors
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
    }
}
