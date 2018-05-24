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
    public sealed partial class Setting : Page
    {
        public List<string> Catagories;
        public string Username { get; set; }
        public string RenewDate
        {
            get
            {
                return $"每月{renewDate}日";
            }
        }
        private int renewDate;
        public string MonthBudget
        {
            get
            {
                return $"{budget.ToString()}元";
            }
        }
        private float budget;
        public Setting()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // give delegate to provider
            // get data from provider
            // dummy
            Username = "陈祐洋";
            budget = 1500;
            renewDate = 1;
            // get catagories;
            Catagories = new List<string>
            {
                "food",
                "drink"
            };
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as HyperlinkButton;
            switch(button.Tag)
            {
                case "changeUsername":
                    GetInputUsername();
                    break;
                case "changeBudget":
                    GetInputBudget();
                    break;
                case "changeRenewDate":
                    GetInputRenewDate();
                    break;
                case "changeLanguage":
                    CustomDialog.ShowConfirmDialog("更改语言", "不存在的");
                    break;
                default:
                    break;
            }
        }
        private async void GetInputUsername()
        {
            var username = await CustomDialog.ShowUsernameDialog();
            Username = username;
        }
        private async void GetInputBudget()
        {
            var b = await CustomDialog.ShowBudgetDialog();
            budget = b;
        }
        private async void GetInputRenewDate()
        {
            var d = await CustomDialog.ShowRenewDateDialog();
            renewDate = d;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            switch (button.Tag)
            {
                case "removeAllData":
                    if (await CustomDialog.ShowConfirmDialog("清除所有数据", "此操作将会清楚所有用户数据，您确定要继续？") == true) {
                        // remove all data
                        // Navigate to Welcome Page
                    }
                    break;
                case "viewCatagories":
                    break;
            }
        }
    }
}
