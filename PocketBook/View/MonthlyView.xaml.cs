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
        List<MonthData> MonthList;
        public MonthlyView()
        {
            this.InitializeComponent();
            MonthList = new List<MonthData>();
            // get month data from provider;
            
            // dummy data below
            for (int i = 1; i <= 12; i++)
            {
                MonthList.Add(new MonthData(i, i * 100));
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
