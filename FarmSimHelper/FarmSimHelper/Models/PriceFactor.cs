using System;
using System.Collections.Generic;
using System.Text;

namespace FarmSimHelper.Models
{
    public class PriceFactor : IComparable<PriceFactor>
    {
        public byte Month { get; set; }
        public decimal Factor { get; set; }

        public int CompareTo(PriceFactor other)
        {
            if (other == null)
            {
                return 1;
            }

            return Factor.CompareTo(other.Factor);
        }
    }
}
