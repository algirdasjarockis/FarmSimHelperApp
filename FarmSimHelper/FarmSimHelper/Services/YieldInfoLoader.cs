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
    public class YieldInfoLoader : IYieldInfoLoader
    {
#if DEBUG
        private readonly string sourceUrl = @"http://10.0.2.2:8000/fruitTypes.xml";
#else
        private readonly string sourceUrl = @"https://raw.githubusercontent.com/algirdasjarockis/FarmSimHelperApp/master/data/fruitTypes.xml";
#endif
        private readonly HttpClient client;

        public YieldInfoLoader(HttpClient client)
        {
            this.client = client;
        }

        public async Task<IEnumerable<ProductYieldInfo>> LoadYieldInfo()
        {
            List<ProductYieldInfo> items = new List<ProductYieldInfo>();
            try
            {
                HttpResponseMessage response = await client.GetAsync(sourceUrl);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                XDocument xmlDoc = XDocument.Parse(responseBody);
                var query = 
                    from c in xmlDoc.Root.Descendants("fruitType")
                    select new
                    {
                        ProductName = c.Attribute("name").Value,
                        LiterPerSqm = float.Parse(c.Element("harvest").Attribute("literPerSqm").Value),
                    };

                foreach (var productElement in query)
                {
                    var item = new ProductYieldInfo()
                    {
                        Product = new ProductInfo()
                        {
                            Name = productElement.ProductName,
                            PricePerLiter = 0,
                        },
                        LitersPerSqm = productElement.LiterPerSqm,
                        Liters = productElement.LiterPerSqm * 10000,
                        ProductImage = ImageSource.FromResource($"FarmSimHelper.Resources.ProductIcons.{productElement.ProductName.ToLower()}.png")
                    };

                    items.Add(item);
                }
            }
            catch (HttpRequestException exception)
            {
                Console.WriteLine(exception.Message);
            }
            catch (System.Net.WebException exception)
            {
                Console.WriteLine(exception.Message);
            }

            return await Task.FromResult(items);
        }
    }
}
