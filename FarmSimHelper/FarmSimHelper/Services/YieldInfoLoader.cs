using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using FarmSimHelper.Models;
using Xamarin.Forms;

namespace FarmSimHelper.Services
{
    public class YieldInfoLoader : IDataLoader<ProductYieldInfo, SquareUnit>
    {
        public async Task<IEnumerable<ProductYieldInfo>> LoadData(SquareUnit unit = default)
        {
            List<ProductYieldInfo> items = new List<ProductYieldInfo>();

            if (!File.Exists(App.Config.DataPathYield))
            {
                return items;
            }

            XDocument xmlDoc = XDocument.Load(App.Config.DataPathYield);
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
                    Liters = productElement.LiterPerSqm * (unit == SquareUnit.Hectares ? 10000 : 4046.86f),
                    ProductImage = ImageSource.FromResource($"FarmSimHelper.Resources.ProductIcons.{productElement.ProductName.ToLower()}.png")
                };

                items.Add(item);
            }

            return await Task.FromResult(items);
        }
    }
}
