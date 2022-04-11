using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using FarmSimHelper.Models;

namespace FarmSimHelper.Converters
{
    public class YieldConverter : BindableObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var newValue = (float)value 
                * (App.Config.Settings.Unit == Models.SquareUnit.Hectares ? 10000 : 4046.86)
                * GetYieldBonusValue(App.Config.Settings.YieldBonus);

            return Math.Round(newValue, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        float GetYieldBonusValue(YieldBonusSelections selectedBonus)
        {
            float bonus = 1.0f;

            bonus += selectedBonus.Fertilized1 ? 0.225f : 0;
            bonus += selectedBonus.Fertilized2 ? 0.225f : 0;
            bonus += selectedBonus.Weeded ? 0.2f : 0;
            bonus += selectedBonus.Limed ? 0.15f : 0;
            bonus += selectedBonus.Plowed ? 0.15f : 0;
            bonus += selectedBonus.Rolled ? 0.025f : 0;
            bonus += selectedBonus.Mulched ? 0.025f : 0;

            return bonus;
        }
    }
}
