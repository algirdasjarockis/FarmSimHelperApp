using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FarmSimHelper.Models
{
    public class ProductYieldInfo
    {
        public ProductInfo Product { get; set; }
        public ImageSource ProductImage { get; set; }
        public float LitersPerSqm { get; set; }
    }
}
