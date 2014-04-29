using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TrackingApp.Resources;

namespace TrackingApp.ViewModels
{
    public class PackageViewModel : INotifyPropertyChanged
    {
        private string _calendarid = null;
        /// <summary>
        /// Reference to Calendar event localid
        /// </summary>
        public string CalendarID
        {
            get
            {
                return _calendarid;
            }
            set
            {
                if (value != _calendarid)
                {
                    _calendarid = value;
                }
            }
        }

        private Carriers _carrier;
        /// <summary>
        /// Which carrier is providing the service.
        /// </summary>
        public Carriers Carrier
        {
            get
            {
                return _carrier;
            }
            set
            {
                if (value != _carrier)
                {
                    _carrier = value;
                    NotifyPropertyChanged("Carrier");
                }
            }
        }

        private DateTime _deliverydate;
        /// <summary>
        /// The delivery date of the package
        /// </summary>
        public DateTime DeliveryDate
        {
            get
            {
                return _deliverydate;
            }
            set
            {
                if (value != _deliverydate)
                {
                    _deliverydate = value;
                    NotifyPropertyChanged("Delivery Date");
                }
            }

        }

        private string _name;
        /// <summary>
        /// Name of the package
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string _tn;
        /// <summary>
        /// Tracking Number for the package.
        /// </summary>
        /// <returns></returns>
        public string TrackingNumber
        {
            get
            {
                return _tn;
            }
            set
            {
                if (value != _tn)
                {
                    _tn = value;
                    NotifyPropertyChanged("TrackingNumber");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}