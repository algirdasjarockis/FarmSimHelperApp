using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using FarmSimHelper.Models;

namespace FarmSimHelper.Services
{
    public class FieldInfoLoader : IDataLoader<FieldInfo, string>
    {
        public async Task<IEnumerable<FieldInfo>> LoadData(string mapName)
        {
            List<FieldInfo> items = new List<FieldInfo>();

            if (!File.Exists(App.Config.GetDataPathFields(mapName)))
            {
                return items;
            }

            XDocument xmlDoc = XDocument.Load(App.Config.DataPathYield);
            var query =
                from c in xmlDoc.Root.Descendants("map")
                select new FieldInfo()
                {
                    Id = int.Parse(c.Element("field").Attribute("id").Value),
                    Size = float.Parse(c.Element("field").Attribute("size").Value),
                    Price = decimal.Parse(c.Element("field").Attribute("price").Value),
                };

            foreach (var field in query)
            {
                items.Add(field);
            }

            return await Task.FromResult(items);
        }
    }
}
