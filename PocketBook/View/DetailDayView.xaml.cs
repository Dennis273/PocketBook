using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
            InitializeComponent();
            provider.DataChanged += OnEntryListChanged;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Date = (DateTime)e.Parameter;
            dataEntries = provider.GetDayDataEntry(Date.Year, Date.Month, Date.Day);
            provider.DataChanged += OnEntryListChanged;
        }

        private async void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            var entry = await CustomDialog.ShowNewEntryDialog(provider.GetCatagories());
            if (entry != null) provider.AddDataEntry(entry);
        }

        public void OnEntryListChanged(DataOperation operation, DataEntry dataEntry)
        {
            if (operation == DataOperation.Add)
            {
                dataEntries.Add(dataEntry);
            }
            if (operation == DataOperation.Remove)
            {
                dataEntries.Remove(dataEntry);
            }
            if (operation == DataOperation.Update)
            {
                var e = dataEntries.Find(entry => dataEntry.Id == entry.Id);
                e = dataEntry;
            }
        }

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var dataEntry = e.ClickedItem as DataEntry;
            var opt = await CustomDialog.ShowDetailDataEntry(dataEntry);
        }
    }
}
