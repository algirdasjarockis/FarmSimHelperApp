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
        public YieldBonusSelections YieldBonus { get; set; }

        public Settings()
        {
            Unit = SquareUnit.Hectares;
            Map = Maps[0];
            Fields = new List<FieldInfo>();
            YieldBonus = new YieldBonusSelections();
        }
    }

    public class YieldBonusSelections
    {
        public bool Fertilized1 { get; set; }
        public bool Fertilized2 { get; set; }
        public bool Weeded { get; set; }
        public bool Rolled { get; set; }
        public bool Limed { get; set; }
        public bool Plowed { get; set; }
        public bool Mulched { get; set; }
    }
}
