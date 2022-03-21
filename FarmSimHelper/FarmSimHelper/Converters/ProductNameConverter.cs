using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace FarmSimHelper.Converters
{
    public class ProductNameConverter : IValueConverter
    {
        readonly Dictionary<string, string> names = new Dictionary<string, string>()
        {
            { "WHEAT", "Wheat" },
            { "BARLEY", "Barley" },
            { "OAT", "Oat" },
            { "CANOLA", "Canola" },
            { "SORGHUM", "Sorghum" },
            { "GRAPE", "Grape" },
            { "OLIVE", "Olive" },
            { "SUNFLOWER", "Sunflower" },
            { "SOYBEAN", "Soybean" },
            { "MAIZE", "Corn" },
            { "POTATO", "Potato" },
            { "SUGARBEET", "Sugar Beet" },
            { "SUGARBEET_CUT", "Sugar Beet Cut" },
            { "COTTON", "Cotton" },
            { "SUGARCANE", "Sugarcane" },
            { "EGG", "Egg" },
            { "WOOL", "Wool" },
            { "MILK", "Milk" },
            { "WATER", "Water" },
            { "CHAFF", "Chaff" },
            { "WOODCHIPS", "Woodchips" },
            { "SILAGE", "Silage" },
            { "GRASS", "Grass" },
            { "GRASS_WINDROW", "GRASS_WINDROW" },
            { "DRYGRASS", "Hay" },
            { "DRYGRASS_WINDROW", "DRYGRASS_WINDROW" },
            { "STRAW", "Straw" },
            { "FLOUR", "Flour" },
            { "BREAD", "Bread" },
            { "CAKE", "Cake" },
            { "BUTTER", "Butter" },
            { "CHEESE", "Cheese" },
            { "FABRIC", "Fabric" },
            { "CLOTHES", "Clothes" },
            { "SUGAR", "Sugar" },
            { "HONEY", "Honey" },
            { "CEREAL", "Cereal" },
            { "SUNFLOWER_OIL", "Sunflower Oil" },
            { "CANOLA_OIL", "Canola Oil" },
            { "OLIVE_OIL", "Olive Oil" },
            { "RAISINS", "Raisins" },
            { "GRAPEJUICE", "Grape Juice" },
            { "LETTUCE", "Lettuce" },
            { "TOMATO", "Tomato" },
            { "STRAWBERRY", "Strawberry" },
            { "CHOCOLATE", "Chocoloate" },
            { "BOARDS", "Boards" },
            { "FURNITURE", "Furniture" },
            { "WHEATBEER", "Wheat Beer" },
            { "BARLEYBEER", "Barley Beer" },
            { "HONEYBEER", "Honey Beer" },
            { "STRAWBERRYBEER", "Strawberry Beer" },
        };
       
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && names.ContainsKey((string)value))
            {
                return names[(string)value];
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
