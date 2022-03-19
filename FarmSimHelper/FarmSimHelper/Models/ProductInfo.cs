using System;
using System.Collections.Generic;
using System.Text;

namespace FarmSimHelper.Models
{
    public class ProductInfo
    {
        public string Name { get; set; }
        public decimal PricePerLiter { get; set; }
        public List<PriceFactor> PriceFactors { get; set; }
    }
}
