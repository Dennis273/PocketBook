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
        float used;
        float avgLeft;
        public Overview()
        {
            this.InitializeComponent();
        }
        public string Summary
        {
            get
            {
                return "今天是" + DateTime.Now.Year + "年" + DateTime.Now.Month + "月" + DateTime.Now.Day + "日, " +
                        "您本月已消费" + GetSpentMoney().ToString() + "元, 平均每日剩余" + GetAvgLeftMoney().ToString() + "元。";
            }
        }
        private float GetSpentMoney()
        {
            return 700;
        }
        private float GetAvgLeftMoney()
        {
            return 60;
        }
    }
}
