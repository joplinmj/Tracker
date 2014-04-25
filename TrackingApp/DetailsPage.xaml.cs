using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TrackingApp.Resources;
using Microsoft.Phone.Tasks;
using TrackingApp.ViewModels;

namespace TrackingApp
{
    public partial class DetailsPage : PhoneApplicationPage
    {
        // Constructor
        public DetailsPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (DataContext == null)
            {
                string selectedIndex = "";
                if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
                {
                    DataContext = App.ViewModel.Items[selectedIndex];
                }
            }
        }

        private void DeletePackage(object sender, EventArgs e)
        {
            if (MessageBox.Show("This action will be permanent", "Delete package", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                App.ViewModel.RemovePackage((PackageViewModel)DataContext);
                App.ViewModel.SaveAll();
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

        private void NavigateEditPackage(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/PackageFormPage.xaml?selectedItem=" + ((PackageViewModel)DataContext).TrackingNumber, UriKind.Relative));
        }

        private void TrackInBrowser(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string trackingURL = "";
            PackageViewModel current = (PackageViewModel)DataContext;

            switch (current.Carrier)
            {
                case Carriers.UPS:
                    trackingURL = "http://wwwapps.ups.com/WebTracking/track?track=yes&trackNums=";
                    break;
                case Carriers.FEDEX:
                    trackingURL = "http://www.fedex.com/Tracking?action=track&tracknumbers=";
                    break;
                case Carriers.USPS:
                    trackingURL = "https://tools.usps.com/go/TrackConfirmAction_input?qtc_tLabels1=";
                    break;
                case Carriers.DHL:
                    trackingURL = "http://track.dhl-usa.com/TrackByNbr.asp?ShipmentNumber=";
                    break;
                case Carriers.ONTRAC:
                    trackingURL = "http://www.ontrac.com/trackingres.asp?tracking_number=";
                    break;
            }

            trackingURL += current.TrackingNumber;

            WebBrowserTask task = new WebBrowserTask();
            task.Uri = new Uri(trackingURL, UriKind.Absolute);
            task.Show();
        }

    }
}