using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace TrackingApp
{
    public partial class SortPage : PhoneApplicationPage
    {
        public SortPage()
        {
            InitializeComponent();
        }

        private void SortByName(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ViewModel.Items = App.ViewModel.Items.Sort(SortFlags.NAME);
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void SortByCarrier(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ViewModel.Items = App.ViewModel.Items.Sort(SortFlags.CARRIER);
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void SortByTrackingNumber(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.ViewModel.Items = App.ViewModel.Items.Sort(SortFlags.TRACKING);
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}