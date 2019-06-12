using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RetailDistribution.Client.UI.Model
{
	public class Vendor : INotifyPropertyChanged
	{
		public int VendorId { get; set; }

		public string VendorName { get; set; }

		private bool isPrimary;
		public bool IsPrimary
		{
			get
			{
				return isPrimary;
			}
			set
			{
				isPrimary = value;
				OnPropertyChanged("IsPrimary");
			}
		}

		#region INotifyPropertyChanged members

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
