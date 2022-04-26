using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Threading.Tasks;
using FarmSimHelper.Models;

namespace FarmSimHelper.Services
{
    public class ProductionInfoLoader : IDataLoader<ProductionInfo, int>
    {
        public async Task<IEnumerable<ProductionInfo>> LoadData(int param = 0)
        {
            List<ProductionInfo> items = new List<ProductionInfo>();
            if (!Directory.Exists(App.Config.DataPathProductions))
            {
                return items;
            }

            foreach (var production in Settings.Productions)
            {
                string fileSource = App.Config.GetDataPathProductions(production);

                XDocument xmlDoc = XDocument.Load(fileSource);
                var query =
                    from c in xmlDoc.Root.Descendants("production")
                    select new
                    {
                        ProductionName = production,
                        ProductId = c.Attribute("id").Value,
                        Cycles = int.Parse(c.Attribute("cyclesPerHour").Value),
                        Costs = float.Parse(c.Attribute("costsPerActiveHour").Value),
                        Inputs = c.Element("inputs").Descendants("input"),
                        Outputs = c.Element("outputs").Descendants("output")
                    };

                foreach (var element in query)
                {
                    ProductionInfo item = new ProductionInfo()
                    {
                        Id = element.ProductionName,
                        CyclesPerHour = element.Cycles,
                        Costs = element.Costs,
                        MainOutputProduct = element.ProductId
                    };

                    foreach (var input in element.Inputs)
                    {
                        item.Inputs.Add(ParseProductionItem(input));
                    }

                    foreach (var output in element.Outputs)
                    {
                        item.Outputs.Add(ParseProductionItem(output));
                    }

                    items.Add(item);
                }
            }

            return items;
        }

        ProductionItem ParseProductionItem(XElement element)
        {
            return new ProductionItem()
            {
                Name = element.Attribute("fillType").Value,
                Amount = float.Parse(element.Attribute("amount").Value)
            };
        }
    }
}
