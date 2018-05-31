using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace PocketBook
{
    class ConvertMoney : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Single money = (Single)value;
            return $"消费：{money.ToString()}元 ";
        }

        // No need to implement converting back on a one-way binding
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
