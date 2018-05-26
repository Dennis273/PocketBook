using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class Setting : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public List<string> Catagories;
        public string Username
        {
            get
            {
                return setting.Username;
            }
        }
        public string RenewDate
        {
            get
            {
                return $"每月{setting.RenewDate}日";
            }
        }
        private DataProvider provider;
        private UserSetting setting;
        public string MonthBudget
        {
            get
            {
                return $"{setting.Budget.ToString()}元";
            }
        }
        public Setting()
        {
            InitializeComponent();
            provider = DataProvider.GetDataProvider();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            setting = provider.GetUserSetting();
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
            if (username != null)
            {
                setting.Username = username;
                provider.SetUserSetting(setting);
                OnPropertyChanged("Username");
            }
        }
        private async void GetInputBudget()
        {
            var b = await CustomDialog.ShowBudgetDialog();
            if (b != -1) {
                setting.Budget = b;
                provider.SetUserSetting(setting);
                OnPropertyChanged("MonthBudget");
            }
        }
        private async void GetInputRenewDate()
        {
            var d = await CustomDialog.ShowRenewDateDialog();
            if (d != setting.RenewDate)
            {
                setting.RenewDate = d;
                provider.SetUserSetting(setting);
                OnPropertyChanged("RenewDate");
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            switch (button.Tag)
            {
                case "removeAllData":
                    if (await CustomDialog.ShowConfirmDialog("清除所有数据", "此操作将会清楚所有用户数据，您确定要继续吗？") == true) {
                        provider.__DeleteAll();
                        // Navigate to Welcome Page
                    }
                    break;
                case "viewCatagories":
                    ShowCatagories();
                    break;
            }
        }

        private async void ShowCatagories()
        {
            var dialog = new ContentDialog();
            var panel = new StackPanel();
            var list = new ListView();
            foreach(var catagory in provider.GetCatagories())
            {
                list.Items.Add(catagory);
            }
            panel.Children.Add(list);
            dialog.Content = panel;
            await dialog.ShowAsync();
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
