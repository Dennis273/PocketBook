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

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace PocketBook
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DataProvider provider = DataProvider.GetDataProvider();

        public MainPage()
        {
            this.InitializeComponent();
        }


        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var view = sender as NavigationView;
            var item = args.SelectedItem as NavigationViewItem;

            if (item.Content.ToString() == "设置")
            {
                contentFrame.Navigate(typeof(Setting));
            }
            switch(item.Tag)
            {
                case "Today":
                    contentFrame.Navigate(typeof(DetailDayView), DateTime.Now.Date);
                    break;
                case "Overview":
                    contentFrame.Navigate(typeof(Overview));
                    break;
                case "DailyView":
                    contentFrame.Navigate(typeof(DailyView));
                    break;
                case "MonthlyView":
                    contentFrame.Navigate(typeof(MonthlyView));
                    break;
                default:
                    break;
            }
            view.Header = item.Content.ToString();

        }

        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            var view = sender as NavigationView;
            contentFrame.Navigate(typeof(Overview));
            view.Header = "概览";
        }
    }
}
