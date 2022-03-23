using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using System.Reflection;

namespace FarmSimHelper.Converters
{
    public class YieldConverter : BindableObject, IValueConverter
    {
        public static readonly BindableProperty UseHaProperty =
            BindableProperty.Create("UseHa", typeof(bool), typeof(YieldConverter), null);

        public bool UseHa
        {
            get { return (bool)GetValue(UseHaProperty); }
            set { SetValue(UseHaProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float litersPerSqm = (float)value;          

            return litersPerSqm * (UseHa ? 10000 : 4046.86);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
