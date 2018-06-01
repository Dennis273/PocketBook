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
            var arg = (int[])e.Parameter;
            Year = arg[0];
            Month = arg[1];
            DayList.Clear();
            datePicker.Date = new DateTime(Year, Month, 1);
            float sum = 0;
            provider.GetDayDataOfMonth(Year, Month).ForEach(data => sum += data.Money);
            foreach (var data in provider.GetDayDataOfMonth(Year, Month))
            {
                data.Percentage = data.Money / sum;
                DayList.Add(data);
            }
        }

        private void OnGridViewSizeChanged(object sender, SizeChangedEventArgs e)
        {

            //var columns = Math.Ceiling(e.NewSize.Width / 100);
            var columns = 7;
            ((ItemsWrapGrid)gridView.ItemsPanelRoot).ItemWidth = e.NewSize.Width / columns;
            ((ItemsWrapGrid)gridView.ItemsPanelRoot).ItemHeight = e.NewSize.Width / columns;
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var dayData = e.ClickedItem as DayData;
            var date = new DateTime(Year, Month, dayData.Day);
            if (date > DateTime.Now.Date) return;
            this.Frame.Navigate(typeof(DetailDayView), new DateTime(Year, Month, dayData.Day));
        }

        private void DatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            var picker = sender as DatePicker;
            if (e.NewDate <= DateTime.Now)
            {
                int[] arg = { e.NewDate.Year, e.NewDate.Month };
                this.Frame.Navigate(typeof(DailyView), arg);
            }
            else
            {
                picker.Date = e.OldDate;
            }
        }
    }
}
