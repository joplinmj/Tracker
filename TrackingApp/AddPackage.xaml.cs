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
    public partial class AddPackage : PhoneApplicationPage
    {
        public AddPackage()
        {
            InitializeComponent();
        }

        private void SaveNewPackage(object sender, EventArgs e)
        {
            bool isFormValid = true;

            string _name = NameInputField.Text.Trim();
            if (_name == "")
            {
                NameInputField.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                isFormValid = false;
            }
            
            string _tn = TrackingNumberInputField.Text.Trim();
            if (_tn == "")
            {
                TrackingNumberInputField.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                isFormValid = false;
            }

            Carriers carrier = Carriers.INVALID;
            if(UPSButton.IsChecked.HasValue || FedExButton.IsChecked.HasValue
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

            if(isFormValid)
            {
                App.ViewModel.SavePackage(new PackageViewModel() { Name = NameInputField.Text, TrackingNumber = TrackingNumberInputField.Text, Carrier = carrier });
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

        private void DeleteNewPackage(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
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