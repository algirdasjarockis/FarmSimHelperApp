using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace FarmSimHelper.Converters
{
    public class MonthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var months = value as int[];

                if (months.Length <= 0)
                {
                    return null;
                }

                string[] monthNames = new string[months.Length];

                for (int i = 0; i < months.Length; i++)
                {
                    var specificCulture = CultureInfo.CreateSpecificCulture("en-US");
                    monthNames[i] = specificCulture.DateTimeFormat.GetAbbreviatedMonthName(months[i]);
                }

                return string.Join(", ", monthNames);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
