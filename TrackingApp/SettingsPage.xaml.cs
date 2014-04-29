using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

namespace TrackingApp
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void ApplyChanges(object sender, EventArgs e)
        {
            App.Settings["ShowInCalendar"] = (bool)ShowInCalendarToggle.IsChecked;
            App.Settings["Reminders"] = (bool)RemindersToggle.IsChecked;
            App.Settings["DeliveryTitle"] = DeliveryTitleInput.Text;
            App.Settings["DeliveryTime"] = DeliveryTimeInput.Value;
            App.Settings["ReminderTime"] = ReminderTimeInput.Value;
            App.Settings.Save();

            NavigateMain(null, null);
        }

        private void ClearAllData(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("This action will be permanent", "Delete all packages", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                App.ViewModel.ClearAllData();
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

        private void NavigateMain(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (App.Settings.Contains("ShowInCalendar"))
            {
                ShowInCalendarToggle.IsChecked = (bool)App.Settings["ShowInCalendar"];
            }
            if (App.Settings.Contains("Reminders"))
            {
                RemindersToggle.IsChecked = (bool)App.Settings["Reminders"];
            }
            if (App.Settings.Contains("DeliveryTitle"))
            {
                DeliveryTitleInput.Text = (string)App.Settings["DeliveryTitle"];
            }
            if (App.Settings.Contains("DeliveryTime"))
            {
                DeliveryTimeInput.Value = (DateTime)App.Settings["DeliveryTime"];
            }
            if (App.Settings.Contains("ReminderTime"))
            {
                ReminderTimeInput.Value = (DateTime)App.Settings["ReminderTime"];
            }
        }
    }
}