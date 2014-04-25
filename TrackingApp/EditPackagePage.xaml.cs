using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TrackingApp.ViewModels;
using TrackingApp.Resources;
using System.Windows.Media;

namespace TrackingApp
{
    public partial class EditPackagePage : PhoneApplicationPage
    {
        public EditPackagePage()
        {
            InitializeComponent();
        }

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

            PackageViewModel package = (PackageViewModel)DataContext;
            NameInputField.Text = package.Name;
            TrackingNumberInputField.Text = package.TrackingNumber;
            switch (package.Carrier)
            {
                case Carriers.UPS:
                    UPSButton.IsChecked = true;
                    break;
                case Carriers.FEDEX:
                    FedExButton.IsChecked = true;
                    break;
                case Carriers.DHL:
                    DHLButton.IsChecked = true;
                    break;
                case Carriers.USPS:
                    USPSButton.IsChecked = true;
                    break;
            }
        }

        private void SavePackage(object sender, EventArgs e)
        {
            string name, tn;
            Carriers carrier = Carriers.INVALID;
            if (ValidateFields(out name, out tn, out carrier))
            {
                App.ViewModel.Items.Remove((PackageViewModel)DataContext);
                App.ViewModel.SavePackage(new PackageViewModel() { Name = name, TrackingNumber = tn, Carrier = carrier });
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

        private bool ValidateFields(out string name, out string trackingNumber, out Carriers carrier)
        {
            bool isFormValid = true;

            name = NameInputField.Text.Trim();
            if (name == "")
            {
                NameInputField.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                isFormValid = false;
            }

            trackingNumber = TrackingNumberInputField.Text.Trim();
            if (trackingNumber == "")
            {
                TrackingNumberInputField.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                isFormValid = false;
            }

            carrier = Carriers.INVALID;
            if (UPSButton.IsChecked.HasValue || FedExButton.IsChecked.HasValue
                || USPSButton.IsChecked.HasValue || DHLButton.IsChecked.HasValue)
            {
                if ((bool)UPSButton.IsChecked)
                {
                    carrier = Carriers.UPS;
                }
                else if ((bool)FedExButton.IsChecked)
                {
                    carrier = Carriers.FEDEX;
                }
                else if ((bool)USPSButton.IsChecked)
                {
                    carrier = Carriers.USPS;
                }
                else if ((bool)DHLButton.IsChecked)
                {
                    carrier = Carriers.DHL;
                }
            }
            if (carrier == Carriers.INVALID)
            {
                // Notify user
                CarrierFieldWarning.Visibility = Visibility.Visible;
                isFormValid = false;
            }

            return isFormValid;
        }

        private void ValidateNameInputField(object sender, RoutedEventArgs e)
        {
            if (NameInputField.Text != "")
            {
                NameInputField.BorderBrush = new SolidColorBrush(Color.FromArgb(191, 255, 255, 255));
            }
        }

        private void ValidateTrackingNumberInputField(object sender, RoutedEventArgs e)
        {
            if (TrackingNumberInputField.Text != "")
            {
                TrackingNumberInputField.BorderBrush = new SolidColorBrush(Color.FromArgb(191, 255, 255, 255));
            }
        }

        private void CarrierSelectionMade(object sender, RoutedEventArgs e)
        {
            CarrierFieldWarning.Visibility = Visibility.Collapsed;
        }
    }
}