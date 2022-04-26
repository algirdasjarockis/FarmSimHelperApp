using System;
using System.Collections.Generic;
using System.Text;

namespace FarmSimHelper.Models
{
    public class ProductionInfo
    {
        public string Id { get; set; }
        public int CyclesPerHour { get; set; }
        public float Costs { get; set; }
        public string MainOutputProduct { get; set; }
        public List<ProductionItem> Inputs { get; set; }
        public List<ProductionItem> Outputs { get; set; }

        public ProductionInfo()
        {
            Id = string.Empty;
            CyclesPerHour = 0;
            Costs = 0;
            MainOutputProduct = string.Empty;
            Inputs = new List<ProductionItem>();
            Outputs = new List<ProductionItem>();
        }
    }
}
