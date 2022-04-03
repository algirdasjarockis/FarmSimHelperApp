using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace FarmSimHelper.Converters
{
    public class YieldConverter : BindableObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float litersPerSqm = (float)value;

            return litersPerSqm * (App.Config.Settings.Unit == Models.SquareUnit.Hectares ? 10000 : 4046.86);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
