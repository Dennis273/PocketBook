using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PocketBook
{
    class CustomDialog
    {
        private ContentDialog dialog;
        public CustomDialog(string title)
        {
            dialog = new ContentDialog
            {
                Title = title,
                Content = new StackPanel()
            };
        }
        public CustomDialog AddMessage(string message)
        {
            var panel = dialog.Content as StackPanel;
            var text = new TextBlock
            {
                Margin = new Thickness(0, 10, 0, 10),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Text = message
            };
            panel.Children.Add(text);
            return this;
        }
        public CustomDialog AddTextInput(string header)
        {
            var panel = dialog.Content as StackPanel;
            var text = new TextBox
            {
                Margin = new Thickness(0, 10, 0, 10),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Header = header
            };
            panel.Children.Add(text);
            return this;
        }
        public CustomDialog AddComboBox(string header, List<string> options)
        {
            var combobox = new ComboBox
            {
                Margin = new Thickness(0, 10, 0, 10),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Header = header
            };
            foreach (var option in options)
            {
                combobox.Items.Add(option);
            }
            var panel = dialog.Content as StackPanel;
            panel.Children.Add(combobox);
            return this;
        }
        public CustomDialog AddTwinButtons(string primary, string secondary)
        {
            dialog.PrimaryButtonText = primary;
            dialog.SecondaryButtonText = secondary;
            return this;
        }
        public CustomDialog AddDateInput(string header)
        {
            var dateInput = new DatePicker
            {
                Header = header,
                YearVisible = false,
                MonthVisible = false,
            };
            var panel = dialog.Content as Panel;
            panel.Children.Add(dateInput);
            return this;
        }
        public async Task<List<Object>> ShowInputDialog()
        {
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                List<Object> a = new List<object>();
                var panel = dialog.Content as StackPanel;
                foreach(var i in panel.Children)
                {
                    a.Add(i);
                }
                return a;
            }
            return null;
        }
        public async Task<ContentDialogResult> ShowDialog()
        {
            return await dialog.ShowAsync();
        }
        public static async Task<DataEntry> ShowNewEntryDialog(List<string> catagories)
        {
            var d = new CustomDialog("新增条目");
            d.AddTextInput("金额").AddDateInput("日期").AddComboBox("类别", catagories).AddTwinButtons("确定", "取消");
            var list = await d.ShowInputDialog();
            var t = list[0] as TextBox;
            var j = list[1] as CalendarDatePicker;
            var k = list[2] as ComboBox;
            if (t == null || j == null || k == null) return null;
            return new DataEntry(float.Parse(t.Text), j.Date.Value.Date, (string)k.SelectedValue);
        }
        public static async Task<bool> ShowConfirmDialog(string message = "您确定要执行此操作？", string description = "")
        {
            var dialog = new ContentDialog
            {
                Title = message,
                Content = description,
                PrimaryButtonText = "确定",
                SecondaryButtonText = "取消"
            };
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                return true;
            }
            else return false;
        }
        public static async Task<string> ShowUsernameDialog()
        {
            var textInput = new TextBox
            {
                Header = "用户名"
            };
            var dialog = new ContentDialog
            {
                Content = textInput,
                Title = "修改用户名",
                PrimaryButtonText = "确定",
                SecondaryButtonText = "取消"
            };
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                return textInput.Text;
            }
            else
            {
                return "";
            }
        }
        public static async Task<float> ShowBudgetDialog()
        {
            var textInput = new TextBox
            {
                Header = "月预算"
            };
            var dialog = new ContentDialog
            {
                Content = textInput,
                Title = "修改月预算",
                PrimaryButtonText = "确定",
                SecondaryButtonText = "取消"
            };
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                // some logic
                try
                {
                    return float.Parse(textInput.Text);
                }
                catch (Exception e)
                {
                    return -1;
                }
            }
            else
            {
                return 0;
            }
        }
        public static async Task<int> ShowRenewDateDialog()
        {
            var dateInput = new DatePicker
            {
                Header = "更新日",
                YearVisible = false,
                MonthVisible = false,
            };
            var dialog = new ContentDialog
            {
                Content = dateInput,
                Title = "修改修改更新日",
                PrimaryButtonText = "确定",
                SecondaryButtonText = "取消"
            };
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                return dateInput.Date.Day;
            }
            else
            {
                return 0;
            }
        }
        public static async Task<int> ShowDetailDataEntry(DataEntry item)
        {
            var dialog = new CustomDialog("详细信息");
            dialog.AddMessage($"金额: {item.Money}");
            dialog.AddMessage($"日期: {item.SpendDate.ToShortDateString()}");
            dialog.AddMessage($"类别: {item.Catagory}");
            dialog.AddTwinButtons("返回", "删除");
            var result = await dialog.ShowDialog();
            if (result == ContentDialogResult.Primary) return 0;
            if (result == ContentDialogResult.Secondary) return 1;
            return -1;
           
        }
        private static ComboBox GetComboBox()
        {
            // get catagory from provider
            string[] a = { "food", "drinks", "sht" };
            var combobox = new ComboBox();
            foreach (string c in a)
            {
                combobox.Items.Add(c);
            }
            combobox.Header = "类型";
            return combobox;
        }
    }
}
