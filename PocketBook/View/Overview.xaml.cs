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
    public sealed partial class Overview : Page
    {
        DataProvider provider;
        public Overview()
        {
            this.InitializeComponent();
            provider = DataProvider.GetDataProvider();
        }
        public string Summary
        {
            get
            {
                var s = "今天是" + DateTime.Now.Year + "年" + DateTime.Now.Month + "月" + DateTime.Now.Day + "日, " +
                        "您本月已消费" + GetSpentMoney().ToString() + "元, ";
                var q = "平均每日剩余" + GetAvgLeftMoney().ToString("f2") + "元。";
                var t = "已经超支!";
                if (GetAvgLeftMoney() > 0) return s + q;
                else return s + t;
            }
        }
        private float GetSpentMoney()
        {
            return provider.GetSpentMoneyOfCurrentMonth().Money;
        }
        private float GetAvgLeftMoney()
        {
            return provider.GetAverageLeftOfCurrentMonth();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var entry = await CustomDialog.ShowNewEntryDialog(provider.GetCatagories());
            if (entry != null)
            {
                provider.AddDataEntry(entry);
            }
        }
    }
}
