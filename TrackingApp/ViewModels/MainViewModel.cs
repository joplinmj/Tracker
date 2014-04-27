using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TrackingApp.Resources;
using Microsoft.Phone.Scheduler;
using Windows.ApplicationModel.Appointments;

namespace TrackingApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new TrackingNumberCollection();
            LoadCalendar();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public TrackingNumberCollection Items { get; set; }

        public AppointmentCalendar Calendar { get; set; }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        async public void CreateCalendarEvent(PackageViewModel package)
        {
            Appointment appointment = new Appointment();
            appointment.StartTime = (DateTime)App.Settings["DeliveryTime"];
            appointment.Duration = TimeSpan.FromHours(2);
            appointment.Details = package.Name;
            appointment.Subject = (string)App.Settings["DeliveryTitle"];

            await Calendar.SaveAppointmentAsync(appointment);
        }

        public void CreateReminder(PackageViewModel package)
        {
            Reminder deliveryReminder = new Reminder(package.TrackingNumber);
            deliveryReminder.BeginTime = System.DateTime.Now.AddSeconds(30);
            deliveryReminder.ExpirationTime = System.DateTime.Now.AddSeconds(610);
            deliveryReminder.Content = "\"" + package.Name + "\" will be arriving today!";
            ScheduledActionService.Add(deliveryReminder);
            Debug.WriteLine("Reminded added for package (" + package.TrackingNumber + ")");
        }

        public bool HasReminder(PackageViewModel package)
        {
            ScheduledAction reminder = ScheduledActionService.Find(package.TrackingNumber);
            if (reminder != null)
            {
                Debug.WriteLine("Reminder found for package (" + package.TrackingNumber + ")");
                return true;
            }

            Debug.WriteLine("No reminder found for package (" + package.TrackingNumber + ")");
            return false;
        }

        /// <summary>
        /// Loads the existing PackageDeliveryDates calendar or
        /// creates a new one if it does not exist.
        /// </summary>
        async private void LoadCalendar()
        {
            this.Calendar = null;
            var appointmentStore = await AppointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AppCalendarsReadWrite);
            var calendars = await appointmentStore.FindAppointmentCalendarsAsync();

            foreach (var calendar in calendars)
            {
                if (calendar.DisplayName == "PackageDeliveryDates")
                {
                    Calendar = calendar;
                    break;
                }
            }

            if (Calendar == null)
            {
                Calendar = await appointmentStore.CreateAppointmentCalendarAsync("PackageDeliveryDates");
            }
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
            if (isolatedStorage.FileExists("packages.xml"))
            {
                Debug.WriteLine("Loading from Isolated Storage.");
                XmlSerializer serializer = new XmlSerializer(typeof(TrackingNumberCollection));

                using (IsolatedStorageFileStream stream = isolatedStorage.OpenFile("packages.xml", FileMode.Open))
                {
                    TrackingNumberCollection newItems = (TrackingNumberCollection)serializer.Deserialize(stream);
                    foreach (PackageViewModel item in newItems)
                    {
                        App.ViewModel.Items.Add(item);
                    }
                }

                Debug.WriteLine("Loaded " + App.ViewModel.Items.Count + " packages.");
            }
            else
            {
                Debug.WriteLine("No data found, creating fake data.");
                this.Items.Add(new PackageViewModel() { Name = "Sim Card Holder", TrackingNumber = "563256038607", Carrier = Carriers.FEDEX });
                this.Items.Add(new PackageViewModel() { Name = "Investment Book", TrackingNumber = "1Z602E8E0323063092", Carrier = Carriers.UPS });
                this.Items.Add(new PackageViewModel() { Name = "Voodoo Labs Power Pedal 2", TrackingNumber = "C11119708769009", Carrier = Carriers.ONTRAC });
                this.Items.Add(new PackageViewModel() { Name = "BLU Soldier", TrackingNumber = "548260028870", Carrier = Carriers.FEDEX });
                this.Items.Add(new PackageViewModel() { Name = "Diablo 3: Reaper of Souls", TrackingNumber = "528938939970", Carrier = Carriers.FEDEX });
                this.Items.Add(new PackageViewModel() { Name = "Amzer Case", TrackingNumber = "9400110200828120779216", Carrier = Carriers.USPS });
            }

            this.IsDataLoaded = true;
        }

        public void RemovePackage(PackageViewModel package)
        {
            Debug.WriteLine("Deleting package (" + package.TrackingNumber + ") and writing out to storage...");
            Items.Remove(package);
            SaveAll();
            Debug.WriteLine("...Saving completed.");
        }

        public void RemoveReminder(PackageViewModel package)
        {
            try
            {
                ScheduledActionService.Remove(package.TrackingNumber);
                Debug.WriteLine("Reminded removed for package (" + package.TrackingNumber + ")");
            }
            catch (Exception error)
            {
                Debug.WriteLine("ERROR: " + error.Message);
            }
        }

        public void SavePackage(PackageViewModel package)
        {
            Debug.WriteLine("Adding package (" + package.TrackingNumber + ") and writing out to storage...");
            App.ViewModel.Items.Add(package);
            SaveAll();
            Debug.WriteLine("...saving completed.");
        }

        public void SaveAll()
        {
            IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream stream = null;
            stream = isolatedStorage.OpenFile("packages.xml", FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(TrackingNumberCollection));

            serializer.Serialize(stream, Items);
            stream.Close();
            stream.Dispose();
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