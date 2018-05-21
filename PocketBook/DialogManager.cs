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
    static class DialogManager
    {
        public static async Task<DataEntry> ShowNewEntryDialog()
        {
            var combobox = GetComboBox();
            var amounInput = new TextBox
            {
                Header = "金额"
            };
            var dateInput = new CalendarDatePicker
            {
                Date = DateTime.Now.Date,
                Header = "日期"
            };
            var panel = new StackPanel();
            amounInput.Margin = new Thickness(0, 10, 0, 10);
            dateInput.Margin = new Thickness(0, 10, 0, 10);
            combobox.Margin = new Thickness(0, 10, 0, 10);
            amounInput.HorizontalAlignment = HorizontalAlignment.Stretch;
            dateInput.HorizontalAlignment = HorizontalAlignment.Stretch;
            combobox.HorizontalAlignment = HorizontalAlignment.Stretch;
            panel.Children.Add(amounInput);
            panel.Children.Add(dateInput);
            panel.Children.Add(combobox);

            var dialog = new ContentDialog
            {
                Content = panel,
                Title = "新增条目",
                PrimaryButtonText = "确定",
                SecondaryButtonText = "取消"
            };
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                return new DataEntry(float.Parse(amounInput.Text, CultureInfo.InvariantCulture.NumberFormat), dateInput.Date.Value.DateTime, (string)combobox.SelectedValue);
            }
            else
            {
                return null;
            }
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
