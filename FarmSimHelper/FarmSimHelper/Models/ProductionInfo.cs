using System;
using System.Collections.Generic;
using System.Text;

namespace FarmSimHelper.Models
{
    public class ProductionInfo
    {
        string Id { get; set; }
        int CyclesPerHour { get; set; }
        float Costs { get; set; }
        List<ProductionItem> Inputs { get; set; }
        List<ProductionItem> Outputs { get; set; }

        ProductionInfo()
        {
            Id = string.Empty;
            CyclesPerHour = 0;
            Costs = 0;
            Inputs = new List<ProductionItem>();
            Outputs = new List<ProductionItem>();
        }
    }
}
