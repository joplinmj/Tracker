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
using TrackingApp.ViewModels;

namespace TrackingApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the LongListSelector control to the sample data
            DataContext = App.ViewModel;
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        // Handle selection changed on LongListSelector
        private void NavigatePackage(object sender, SelectionChangedEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            if (MainLongListSelector.SelectedItem == null)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + (MainLongListSelector.SelectedItem as PackageViewModel).TrackingNumber, UriKind.Relative));

            // Reset selected item to null (no selection)
            MainLongListSelector.SelectedItem = null;
        }

        private void NavigateAddPackage(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/PackageFormPage.xaml", UriKind.Relative));
        }

        private void NavigateSort(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SortPage.xaml", UriKind.Relative));
        }

        private void NavigateFilter(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/FilterPage.xaml", UriKind.Relative));
        }

        private void NavigateHistory(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/HistoryPage.xaml", UriKind.Relative));
        }

        private void NavigateAbout(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

    }
}