﻿#pragma checksum "C:\Users\Andrew\Documents\GitHub\Tracker\TrackingApp\EditPackagePage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8B672B801CE475670ADAB5727B571C1A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace TrackingApp {
    
    
    public partial class EditPackagePage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBox NameInputField;
        
        internal System.Windows.Controls.TextBox TrackingNumberInputField;
        
        internal System.Windows.Controls.TextBlock CarrierFieldWarning;
        
        internal System.Windows.Controls.RadioButton UPSButton;
        
        internal System.Windows.Controls.RadioButton FedExButton;
        
        internal System.Windows.Controls.RadioButton USPSButton;
        
        internal System.Windows.Controls.RadioButton DHLButton;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/TrackingApp;component/EditPackagePage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.NameInputField = ((System.Windows.Controls.TextBox)(this.FindName("NameInputField")));
            this.TrackingNumberInputField = ((System.Windows.Controls.TextBox)(this.FindName("TrackingNumberInputField")));
            this.CarrierFieldWarning = ((System.Windows.Controls.TextBlock)(this.FindName("CarrierFieldWarning")));
            this.UPSButton = ((System.Windows.Controls.RadioButton)(this.FindName("UPSButton")));
            this.FedExButton = ((System.Windows.Controls.RadioButton)(this.FindName("FedExButton")));
            this.USPSButton = ((System.Windows.Controls.RadioButton)(this.FindName("USPSButton")));
            this.DHLButton = ((System.Windows.Controls.RadioButton)(this.FindName("DHLButton")));
        }
    }
}

