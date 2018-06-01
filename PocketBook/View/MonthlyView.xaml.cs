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
    public sealed partial class MonthlyView : Page
    {
        DataProvider provider;
        List<MonthData> MonthList;
        public List<string> analysis;
        public int Year;
        public MonthlyView()
        {
            InitializeComponent();
            provider = DataProvider.GetDataProvider();
            MonthList = new List<MonthData>();
            analysis = new List<string>();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Year = (int)e.Parameter;
            MonthList.Clear();
            datePicker.Date = new DateTime(Year, 1, 1);
            foreach (var data in provider.GetMonthDataOfYear(Year))
            {
                MonthList.Add(data);
            }
            analysis.Clear();
            foreach (var data in provider.GetPercentageAmongYear(Year))
            {
                analysis.Add($"{data.Key} : {data.Value.ToString("P2")}");
            }
        }
        private void OnGridViewSizeChanged(object sender, SizeChangedEventArgs e)
        {

            var columns = 4;
            ((ItemsWrapGrid)gridView.ItemsPanelRoot).ItemWidth = e.NewSize.Width / columns;
            ((ItemsWrapGrid)gridView.ItemsPanelRoot).ItemHeight = e.NewSize.Width / columns;
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var monthData = e.ClickedItem as MonthData;
            var date = new DateTime(Year, monthData.Month, 1);
            if (date > DateTime.Now) return;
            int[] arg = { Year, monthData.Month };
            this.Frame.Navigate(typeof(DailyView), arg);
        }

        private void DatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            var picker = sender as DatePicker;
            if (e.NewDate <= DateTime.Now)
            {
                this.Frame.Navigate(typeof(MonthlyView), e.NewDate.Year);
            }
            else picker.Date = e.OldDate;
        }
    }
}
