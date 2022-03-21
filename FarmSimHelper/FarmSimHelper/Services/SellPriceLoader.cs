using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using FarmSimHelper.Models;
using Xamarin.Forms;

namespace FarmSimHelper.Services
{
    public class SellPriceLoader : ISellPriceLoader
    {
        private readonly string sourceUrl = @"https://raw.githubusercontent.com/algirdasjarockis/FarmSimHelperApp/master/data/fillTypes.xml";
        private readonly HttpClient client;
        private readonly IProductPriceCalculator productPriceCalculator;
        private readonly List<string> ignoredProducts = new List<string>()
        {
            "GRASS_WINDROW",
            "DRYGRASS_WINDROW",
            "WATER"
        };

        public SellPriceLoader(HttpClient client, IProductPriceCalculator calculator)
        {
            this.client = client;
            this.productPriceCalculator = calculator;
        }

        public async Task<IEnumerable<SellingPrice>> LoadSellingPrices()
        {
            List<SellingPrice> items = new List<SellingPrice>();
            try
            {
                HttpResponseMessage response = await client.GetAsync(sourceUrl);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                XDocument xmlDoc = XDocument.Parse(responseBody);
                var query = from c in xmlDoc.Root.Descendants("fillType")
                            select new
                            {
                                ProductName = c.Attribute("name").Value,
                                PricePerLiter = decimal.Parse(c.Element("economy").Attribute("pricePerLiter").Value),
                                PriceFactors = c.Element("economy")?.Element("factors")?.Descendants("factor")
                            };

                foreach (var productElement in query)
                {
                    if (productElement.PriceFactors == null || ignoredProducts.Contains(productElement.ProductName))
                        continue;

                    List<PriceFactor> factors = new List<PriceFactor>();
                    foreach (var factor in productElement.PriceFactors)
                    {
                        factors.Add(new PriceFactor()
                        {
                            Month = ConvertToNormalMonth(byte.Parse(factor.Attribute("period").Value)),
                            Factor = decimal.Parse(factor.Attribute("value").Value)
                        });
                    }

                    ProductInfo productInfo = new ProductInfo()
                    {
                        Name = productElement.ProductName,
                        PricePerLiter = productElement.PricePerLiter,
                        PriceFactors = factors,
                    };

                    var sellingPrice = productPriceCalculator.CalculateSellingPrice(productInfo, 1.0f);
                    sellingPrice.ProductImage = ImageSource.FromResource($"FarmSimHelper.Resources.ProductIcons.{productElement.ProductName.ToLower()}.png");
                    items.Add(sellingPrice);
                }
            }
            catch (HttpRequestException exception)
            {
                Console.WriteLine(exception.Message);
            }
            
            return await Task.FromResult(items);
        }

        private byte ConvertToNormalMonth(byte period)
        {
            byte step = 2;
            byte totalMonths = 12;
            byte newMonth = (byte)(period + step);
            
            if (newMonth > totalMonths)
            {
                newMonth = (byte)(newMonth - totalMonths);
            }

            return newMonth;
        }
    }
}
