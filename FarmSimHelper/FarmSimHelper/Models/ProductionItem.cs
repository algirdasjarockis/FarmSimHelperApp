using System;
using System.Collections.Generic;
using System.Text;

namespace FarmSimHelper.Models
{
    public class ProductionItem
    {
        string Name { get; set; }
        float Amount { get; set; }

        ProductionItem()
        {
            Name = "";
            Amount = 0.0f;
        }
    }
}
