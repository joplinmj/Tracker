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

        public void ClearAllData()
        {
            Debug.WriteLine("Clearing data...");
            for (int i = Items.Count-1; i >= 0; --i)
            {
                Debug.WriteLine("Deleting package (" + Items[i].TrackingNumber + ")");
                RemovePackage(Items[i]);
            }
            Debug.WriteLine("...done clearing data");
            Save();
        }

        async public void CreateCalendarEvent(PackageViewModel package)
        {
            Appointment appointment = new Appointment();
            appointment.StartTime = new DateTimeOffset(package.DeliveryDate.Date.Add(((DateTime)App.Settings["DeliveryTime"]).TimeOfDay));
            appointment.Duration = TimeSpan.FromHours(2);
            appointment.Details = package.Name;
            appointment.Subject = (string)App.Settings["DeliveryTitle"];
            Debug.WriteLine("Calendar event created for package (" + package.TrackingNumber + ")");

            await Calendar.SaveAppointmentAsync(appointment);
            package.CalendarID = appointment.LocalId;
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

        /// <summary>
        /// Edits the calendar event with localid and adjusts
        /// it to match package. Assigns the localid to package.
        /// </summary>
        /// <param name="localid"></param>
        /// <param name="package"></param>
        async public void EditCalendarEvent(string localid, PackageViewModel package)
        {
            if (localid != null)
            {
                var calendarEvent = await Calendar.GetAppointmentAsync(localid);
                calendarEvent.Details = package.Name;
                DateTime newDate = package.DeliveryDate.Date + ((DateTime)App.Settings["DeliveryTime"]).TimeOfDay;
                calendarEvent.StartTime = newDate;
                package.CalendarID = calendarEvent.LocalId;
                await Calendar.SaveAppointmentAsync(calendarEvent);
                Debug.WriteLine("Calendar event (" + localid + ") edited successfully");
            }
        }

        /// <summary>
        /// Edits an existing package by carrying over necessary 
        /// information before deleting the old package and creating the 
        /// replacement.
        /// </summary>
        public void EditPackage(PackageViewModel package, PackageViewModel editedPackage, bool keepOrAddCalendar, bool keepOrAddReminder)
        {
            Debug.WriteLine("Editing package ("+package.TrackingNumber+")...");
            string calendarId = package.CalendarID;
            if ((calendarId != null) && keepOrAddCalendar)
            {
                EditCalendarEvent(calendarId, editedPackage);
                package.CalendarID = null; // Remove connection
                keepOrAddCalendar = false; // Keep, do not want AddPackage to make another
            }

            RemovePackage(package);
            AddPackage(editedPackage, keepOrAddCalendar, keepOrAddReminder);
            Debug.WriteLine("...done editing package ("+package.TrackingNumber+")");
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
                this.Items.Add(new PackageViewModel() { Name = "Sim Card Holder", TrackingNumber = "563256038607", Carrier = Carriers.FEDEX, DeliveryDate = DateTime.Parse("04/29/2014 15:00:00") });
                this.Items.Add(new PackageViewModel() { Name = "Investment Book", TrackingNumber = "1Z602E8E0323063092", Carrier = Carriers.UPS, DeliveryDate = DateTime.Parse("04/29/2014 15:00:00") });
                this.Items.Add(new PackageViewModel() { Name = "Voodoo Labs Power Pedal 2", TrackingNumber = "C11119708769009", Carrier = Carriers.ONTRAC, DeliveryDate = DateTime.Parse("04/29/2014 15:00:00") });
                this.Items.Add(new PackageViewModel() { Name = "BLU Soldier", TrackingNumber = "548260028870", Carrier = Carriers.FEDEX, DeliveryDate = DateTime.Parse("04/29/2014 15:00:00") });
                this.Items.Add(new PackageViewModel() { Name = "Diablo 3: Reaper of Souls", TrackingNumber = "528938939970", Carrier = Carriers.FEDEX, DeliveryDate = DateTime.Parse("04/29/2014 15:00:00") });
                this.Items.Add(new PackageViewModel() { Name = "Amzer Case", TrackingNumber = "9400110200828120779216", Carrier = Carriers.USPS, DeliveryDate = DateTime.Parse("04/29/2014 15:00:00") });
            }

            this.IsDataLoaded = true;
        }

        async public void RemoveCalendarEvent(PackageViewModel package)
        {
            if (package.CalendarID != null)
            {
                await Calendar.DeleteAppointmentAsync(package.CalendarID);
            }
        }

        /// <summary>
        /// Removes package from Items and cleans up reminders
        /// and calendars (if they exist). Does not save data to storage
        /// </summary>
        /// <param name="package"></param>
        public void RemovePackage(PackageViewModel package)
        {
            Debug.WriteLine("Removing package (" + package.TrackingNumber + ")...");
            if (HasReminder(package))
            {
                RemoveReminder(package);
            }
            if (package.CalendarID != null)
            {
                RemoveCalendarEvent(package);
            }
            Items.Remove(package);
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

        /// <summary>
        /// Adds a new package and handles creation
        /// of calendar events or reminders. Does not save data to storage
        /// </summary>
        /// <param name="package"></param>
        public void AddPackage(PackageViewModel package, bool addToCalendar, bool createReminder)
        {
            Debug.WriteLine("Adding package (" + package.TrackingNumber + ")...");
            App.ViewModel.Items.Add(package);
            
            if (addToCalendar)
            {
                App.ViewModel.CreateCalendarEvent(package);
            }
            if (createReminder)
            {
                App.ViewModel.CreateReminder(package);
            }

            Debug.WriteLine("...added package successfully.");
        }

        public void Save()
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