using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TrackingApp.ViewModels;
using Windows.Storage;

namespace TrackingApp
{
    public enum SortFlags { NAME, TRACKING, CARRIER };

    public class TrackingNumberCollection:ObservableCollection<PackageViewModel>
    {

        public TrackingNumberCollection():base()
        {

        }

        public TrackingNumberCollection(ObservableCollection<PackageViewModel> collection)
        {
            foreach (PackageViewModel item in collection)
            {
                this.Add(item);
            }
        }

        public PackageViewModel this [string tn]
        {
            get
            {
                PackageViewModel _found = null;
                foreach (PackageViewModel package in this)
                {
                    if (package.TrackingNumber == tn)
                    {
                        _found = package;
                        break;
                    }
                }

                if (_found == null)
                {
                    throw new Exception("ERROR: Tracking Number not found");
                }

                return _found;
            }
        }

        public TrackingNumberCollection Sort(SortFlags sortBy)
        {
            List<PackageViewModel> temp;

            if (sortBy == SortFlags.CARRIER)
            {
                temp = this.OrderBy(i => i.Carrier).ToList<PackageViewModel>();
            }
            else if (sortBy == SortFlags.NAME)
            {
                temp = this.OrderBy(i => i.Name).ToList<PackageViewModel>();
            }
            else
            {
                temp = this.OrderBy(i => i.TrackingNumber).ToList<PackageViewModel>();
            }

            return new TrackingNumberCollection(new ObservableCollection<PackageViewModel>(temp));
        }
    }
}
