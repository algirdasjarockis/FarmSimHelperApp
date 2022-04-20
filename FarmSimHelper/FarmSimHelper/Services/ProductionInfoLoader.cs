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

            XDocument xmlDoc = XDocument.Load(App.Config.DataPathYield);
            var query =
                from c in xmlDoc.Root.Descendants("production")
                select new
                {
                    ProductionName = c.Attribute("id").Value,
                    Cycles = int.Parse(c.Attribute("cyclesPerHour").Value),
                    Costs = float.Parse(c.Attribute("costsPerActiveHour").Value),
                    Inputs = c.Descendants("inputs"),
                    Outputs = c.Descendants("outputs"),
                };

            return items;
        }
    }
}
