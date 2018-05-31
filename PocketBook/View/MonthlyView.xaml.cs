﻿using System;
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
        public int Year;
        public MonthlyView()
        {
            InitializeComponent();
            provider = DataProvider.GetDataProvider();
            MonthList = new List<MonthData>();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Year = (int)e.Parameter;
            MonthList.Clear();
            foreach(var data in provider.GetMonthDataOfYear(Year))
            {
                MonthList.Add(data);
            }
        }
        private void OnGridViewSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Here I'm calculating the number of columns I want based on
            // the width of the page
            var columns = Math.Ceiling(ActualWidth / 300);
            ((ItemsWrapGrid)gridView.ItemsPanelRoot).ItemWidth = e.NewSize.Width / columns;
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var monthData = e.ClickedItem as MonthData;
            int[] arg = { Year, monthData.Month };
            this.Frame.Navigate(typeof(DailyView), arg);
        }
    }
}
