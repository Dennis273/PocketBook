using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace PocketBook
{
    class ConvertMoney : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Single money = (Single)value;
            return money == 0 ? "-" : money.ToString();
        }

        // No need to implement converting back on a one-way binding
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    class ConvertPercentageToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Single money = (Single)value;
            // 247, 202, 76
            var color = Color.FromArgb((byte)255, (byte)247, (byte)202, (byte)76);
            var brush = new AcrylicBrush
            {
                TintColor = color,
                TintOpacity = 0.1
            };
            return brush;
        }

        // No need to implement converting back on a one-way binding
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

    }

}
