using FarmSimHelper.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmSimHelper.Services
{
    public class ProductPriceCalculator : IProductPriceCalculator
    {
        public SellingPrice CalculateSellingPrice(ProductInfo productInfo, float baseFactor)
        {
            SellingPrice price = new SellingPrice()
            {
                ProductName = productInfo.Name,
                AveragePrice = (int)Math.Round(CalculateAveragePrice(productInfo) * baseFactor),
                GoodPrice = (int)Math.Round(CalculateGoodPrice(productInfo) * baseFactor),
                BestPrice = (int)Math.Round(CalculateBestPrice(productInfo) * baseFactor),
            };

            return price;
        }

        private int CalculateAveragePrice(ProductInfo product)
        {
            decimal total = 0;
            foreach (PriceFactor factor in product.PriceFactors)
            {
                total += product.PricePerLiter * factor.Factor * 1000;
            }

            return (int)Math.Round(total / 12.0m);
        }

        private int CalculateBestPrice(ProductInfo product)
        {
            return 0;
        }

        private int CalculateGoodPrice(ProductInfo product)
        {
            return 0;
        }
    }
}
