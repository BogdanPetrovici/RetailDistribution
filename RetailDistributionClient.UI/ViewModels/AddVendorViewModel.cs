using RetailDistribution.Client.UI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace RetailDistribution.Client.UI.ViewModels
{
    public class AddVendorViewModel : INotifyPropertyChanged
    {
        private HttpClient client;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AddVendorViewModel(HttpClient client, int districtId)
        {
            this.client = client;
            DistrictId = districtId;
        }

        /// <summary>
        /// Calls the service endpoint and gets all the available vendors for a particular district
        /// </summary>
        /// <param name="path">The service endpoint path</param>
        /// <returns></returns>
        public async Task<IEnumerable<Vendor>> GetUnusedVendorsAsync(string path, int districtId)
        {
            try
            {
                var parametrizedPath = $"{path}/getremainingvendors/{districtId}";
                HttpResponseMessage response = await client.GetAsync(parametrizedPath).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    Vendors = await response.Content.ReadAsAsync<IList<Vendor>>();
                }

                return Vendors;
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return null;
            }
        }

        public async Task<bool> AddVendor(string path)
        {
            try
            {
                if (SelectedVendor != null)
                {
                    var parametrizedPath = $"{path}/{DistrictId}";
                    SelectedVendor.IsPrimary = IsPrimary;
                    HttpResponseMessage response = await client.PostAsJsonAsync(parametrizedPath, SelectedVendor).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {
                        var success = await response.Content.ReadAsAsync<bool>();
                        return true;
                    }
                }

                return false;
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private IList<Vendor> vendors;
        public IList<Vendor> Vendors
        {
            get { return vendors; }
            set
            {
                vendors = value; OnPropertyChanged("Vendors");
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
                OnPropertyChanged("IsAddVendorButtonEnabled");
            }
        }

        public bool IsPrimary { get; set; }

        //The id for which the form is opened
        public int DistrictId { get; set; }

        public bool IsAddVendorButtonEnabled
        {
            get
            {
                return SelectedVendor != null;
            }
        }
    }
}
