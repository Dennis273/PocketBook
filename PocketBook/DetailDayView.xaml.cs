using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace PocketBook
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DetailDayView : Page
    {
        public List<DataEntry> dataEntries;

        public DetailDayView()
        {
            this.InitializeComponent();
            // Add delegate to provider
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            dataEntries = GetDataEntries();
        }

        private List<DataEntry> GetDataEntries()
        {
            return null;
        }

        private async void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            // show dialog to obtain user input
            // add entry to provider
            var entry = await DialogManager.ShowNewEntryDialog();
        }


        public void OnEntryListChanged()
        {

        }
        public void OnDataChanged()
        {

        }
    }
}
