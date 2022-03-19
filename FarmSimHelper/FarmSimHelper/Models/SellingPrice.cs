using System;
using System.Collections.Generic;
using System.Text;

namespace FarmSimHelper.Models
{
    public class SellingPrice
    {
        public string ProductName { get; set; }
        public string Icon { get; set; }
        public int AveragePrice { get; set; }
        public int GoodPrice { get; set; }
        public int BestPrice { get; set; }
        public int[] BestMonths { get; set; }

        public SellingPrice ()
        {
            BestMonths = new int[2];
        }
    }
}
