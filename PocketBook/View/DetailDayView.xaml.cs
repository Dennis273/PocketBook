using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        private DataProvider provider = DataProvider.GetDataProvider();
        public List<DataEntry> dataEntries;
        public DateTime Date;
        public String Header
        {
            get
            {
                return $"{Date.Year}年{Date.Month}月{Date.Day}日";
            }
        }
        public DetailDayView()
        {
            this.InitializeComponent();
            // Add delegate to provider
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Date = (DateTime)e.Parameter;
            dataEntries = GetDataEntries();
            provider.DataChangedHandlers += OnEntryListChanged;
        }

        private List<DataEntry> GetDataEntries()
        {
            return provider.GetDayDataEntry(Date.Year, Date.Month, Date.Day);
        }

        private async void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            // show dialog to obtain user input
            // add entry to provider

            var entry = await CustomDialog.ShowNewEntryDialog();
            provider.AddDataEntry();

            var c = new List<string> { "sss", "vvv", "ccc" };
            var entry = await CustomDialog.ShowNewEntryDialog(c);

        }


        public void OnEntryListChanged(DataOperation dataOpration, DataEntry dataEntry)
        {

        }

        public void OnDataChanged()
        {

        }

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var dataEntry = e.ClickedItem as DataEntry;
            var opt = await CustomDialog.ShowDetailDataEntry(dataEntry);
        }
    }
}
