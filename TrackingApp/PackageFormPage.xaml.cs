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
    public partial class PackageFormPage : PhoneApplicationPage
    {
        public PackageFormPage()
        {
            InitializeComponent();
        }

        private void CarrierSelectionMade(object sender, RoutedEventArgs e)
        {
            CarrierFieldWarning.Visibility = Visibility.Collapsed;
        }

        private void DeletePackage(object sender, EventArgs e)
        {
            // If we are editing an item, delete it
            if (DataContext != null)
            {
                App.ViewModel.RemovePackage((PackageViewModel)DataContext);
            }

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string selectedIndex = "";
            if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
            {
                DataContext = App.ViewModel.Items[selectedIndex];

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
                if (App.ViewModel.HasReminder(package))
                {
                    RemindersEnabled.IsChecked = true;
                }

                PageTitle.Text = "edit package";
            }
            else
            {
                PageTitle.Text = "add package";
            }
        }

        private void SavePackage(object sender, EventArgs e)
        {
            string name, tn;
            Carriers carrier = Carriers.INVALID;
            DateTime date;
            if (ValidateFields(out name, out tn, out carrier, out date))
            {
                PackageViewModel package = new PackageViewModel() { Name = name, TrackingNumber = tn, Carrier = carrier, DeliveryDate = date };

                // If we are editing an item, delete the original first
                // Delete any reminders attached to it as well
                if (DataContext != null)
                {
                    App.ViewModel.Items.Remove((PackageViewModel)DataContext);
                    if(App.ViewModel.HasReminder((PackageViewModel)DataContext))
                    {
                        App.ViewModel.RemoveReminder((PackageViewModel)DataContext);
                    }
                }

                if ((bool)RemindersEnabled.IsChecked == true)
                {
                    App.ViewModel.CreateReminder(package);
                }

                App.ViewModel.SavePackage(package);
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

        private bool ValidateFields(out string name, out string trackingNumber, out Carriers carrier, out DateTime date)
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

            DateTime? _date = DeliveryDateInputField.Value;
            date = (DateTime)_date;
            if (_date == null)
            {
                DeliveryDateInputField.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
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
    }
}