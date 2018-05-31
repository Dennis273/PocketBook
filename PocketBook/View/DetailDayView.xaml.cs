using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace PocketBook
{

    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DetailDayView : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private DataProvider provider = DataProvider.GetDataProvider();
        public ObservableCollection<DataEntry> dataEntries;
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
            dataEntries = new ObservableCollection<DataEntry>(provider.GetDayDataEntry(Date.Year, Date.Month, Date.Day));
            provider.DataChanged += OnEntryListChanged;
        }

        private async void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            var entry = await CustomDialog.ShowNewEntryDialog(provider.GetCatagories());
            if (entry != null) provider.AddDataEntry(entry);
        }

        public void OnEntryListChanged(DataOperation operation, DataEntry dataEntry)
        {
            //    if (operation == DataOperation.Add)
            //    {
            //        dataEntries.Add(dataEntry);
            //    }
            //    if (operation == DataOperation.Remove)
            //    {
            //        dataEntries.Remove(dataEntry);
            //    }
            //    if (operation == DataOperation.Update)
            //    {

            //    }
        }

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var dataEntry = e.ClickedItem as DataEntry;
            var opt = await CustomDialog.ShowDetailDataEntry(dataEntry);
            if (opt == 1) provider.DeleteDataEntry(dataEntry);
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
