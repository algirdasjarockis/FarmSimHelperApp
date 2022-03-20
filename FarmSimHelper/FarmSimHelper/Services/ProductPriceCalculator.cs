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
                BestPrice = (int)Math.Round(CalculateBestPrice(productInfo) * baseFactor),
                Product = productInfo,
                BestMonths = FindBestMonths(productInfo)
            };

            return price;
        }

        public SellingPrice RecalculateSellingPrice(SellingPrice price, float baseFactor)
        {
            price.AveragePrice = (int)Math.Round(CalculateAveragePrice(price.Product) * baseFactor);
            price.BestPrice = (int)Math.Round(CalculateBestPrice(price.Product) * baseFactor);

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
            decimal max = 0;
            foreach (PriceFactor factor in product.PriceFactors)
            {
                decimal price = CalculateBasePrice(product.PricePerLiter, factor.Factor);
                if (price > max)
                {
                    max = price;
                }
            }

            return (int)Math.Round(max);
        }

        private decimal CalculateBasePrice(decimal pricePerLiter, decimal factor)
        {
            return pricePerLiter * factor * 1000;
        }

        private int[] FindBestMonths(ProductInfo product)
        {
            int count = product.PriceFactors.Count;
            product.PriceFactors.Sort();

            return new int[] { product.PriceFactors[count - 1].Month, product.PriceFactors[count - 2].Month };
        }
    }
}
