using System;
using System.Collections.Generic;
using System.Text;

namespace FarmSimHelper.Models
{
    public class ProductionItem
    {
        public string Name { get; set; }
        public float Amount { get; set; }

        public ProductionItem()
        {
            Name = "";
            Amount = 0.0f;
        }
    }
}
