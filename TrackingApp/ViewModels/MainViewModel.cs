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

namespace TrackingApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new TrackingNumberCollection();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public TrackingNumberCollection Items { get; set; }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        public void RemovePackage(PackageViewModel package)
        {
            Items.Remove(package);
            SaveAll();
        }

        public void SavePackage(PackageViewModel package)
        {
            App.ViewModel.Items.Add(package);
            SaveAll();
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