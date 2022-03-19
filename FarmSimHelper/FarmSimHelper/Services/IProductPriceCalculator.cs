using System;
using System.Collections.Generic;
using System.Text;
using FarmSimHelper.Models;

namespace FarmSimHelper.Services
{
    public interface IProductPriceCalculator
    {
        SellingPrice CalculateSellingPrice(ProductInfo productInfo, float baseFactor);
    }
}
