using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
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
        public DataEntry data;
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
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Date = (DateTime)e.Parameter;
            dataEntries = new ObservableCollection<DataEntry>(provider.GetDayDataEntry(Date.Year, Date.Month, Date.Day));
            provider.DataChanged += OnEntryListChanged;
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            provider.DataChanged -= OnEntryListChanged;
        }

        private async void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            var entry = await CustomDialog.ShowNewEntryDialog(provider.GetCatagories());
            if (entry != null) provider.AddDataEntry(entry);
        }

        public void OnEntryListChanged(DataOperation operation, DataEntry dataEntry)
        {
            if (dataEntry.SpendDate != Date) return;
            switch (operation)
            {
                case DataOperation.Add:
                    dataEntries.Insert(0, dataEntry);
                    break;
                case DataOperation.Remove:
                    dataEntries.Remove(dataEntry);
                    break;
                case DataOperation.Update:
                    break;
            }
        }

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var dataEntry = e.ClickedItem as DataEntry;
            var opt = await CustomDialog.ShowDetailDataEntry(dataEntry);
            // 0 for default, 1 for delete operation
            if (opt == 1) provider.DeleteDataEntry(dataEntry);
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ListView_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            var listView = (FrameworkElement)sender;
            // ListViewItemPresenter is the item tapped
            if (!(e.OriginalSource is ListViewItemPresenter item)) return;
            // data is the binded dataEntry
            data = item.Content as DataEntry;
            var flyout = new MenuFlyout();
            var flyoutItem = new MenuFlyoutItem
            {
                Text = "分享",
                Tag = "share",
            };
            flyoutItem.Click += MenuFlyoutItemTapped;
            var flyoutItem2 = new MenuFlyoutItem
            {
                Text = "删除",
                Tag = "delete",
            };
            flyoutItem2.Click += (object sender2, RoutedEventArgs e2) =>
            {
                provider.DeleteDataEntry(data);
            };
            flyout.Items.Add(flyoutItem);
            flyout.Items.Add(flyoutItem2);
            flyout.ShowAt(item);
        }


        private void MenuFlyoutItemTapped(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuFlyoutItem;
            switch (item.Tag)
            {
                case "share":
                    ShowShareDialog();
                    break;
            }
        }
        private void ShowShareDialog()
        {
            DataTransferManager.ShowShareUI();
        }
        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            DataRequestDeferral deferal = request.GetDeferral();
            request.Data.Properties.Title = "消费记录";
            //设置共享内容
            request.Data.SetText($"消费金额：{data.Money}元\n类别：{data.Catagory}\n备注：{data.Comment}");
            deferal.Complete();
        }
    }
}
