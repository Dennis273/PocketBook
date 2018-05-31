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
    public sealed partial class DailyView : Page
    {
        DataProvider provider;
        List<DayData> DayList;
        public int Year;
        public int Month;
        public DailyView()
        {
            this.InitializeComponent();
            provider = DataProvider.GetDataProvider();
            DayList = new List<DayData>();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var arg = e.Parameter as int[];
            Year = arg[0];
            Month = arg[1];
            DayList.Clear();
            foreach (var data in provider.GetDayDataOfMonth(Year, Month))
            {
                DayList.Add(data);
            }
        }
        private void OnGridViewSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Here I'm calculating the number of columns I want based on
            // the width of the page
            var columns = Math.Ceiling(ActualWidth / 300);
            ((ItemsWrapGrid)gridView.ItemsPanelRoot).ItemWidth = e.NewSize.Width / columns;
        }
    }
}
