using System;
using System.Collections.Generic;
using System.Text;

namespace FarmSimHelper.Models
{
    public enum SquareUnit
    {
        Hectares,
        Acres
    }

    public class Settings
    {
        public SquareUnit Unit { get; set; }
        public string Map { get; set; }
        public List<FieldInfo> Fields { get; set; }
        public static List<string> Maps => new List<string>()
        {
            "Elmcreek",
            "Erlengrat",
            "Beyleron"
        };

        public Settings()
        {
            Unit = SquareUnit.Hectares;
            Map = Maps[0];
            Fields = new List<FieldInfo>();
        }
    }
}
