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
            var fileName = App.Config.GetDataPathFields(mapName);

            if (!File.Exists(fileName))
            {
                return items;
            }

            try
            {
                XDocument xmlDoc = XDocument.Load(fileName);
                var query =
                    from c in xmlDoc.Root.Descendants("field")
                    select new FieldInfo()
                    {
                        Id = int.Parse(c.Attribute("id").Value),
                        Size = float.Parse(c.Attribute("size").Value),
                        Price = decimal.Parse(c.Attribute("price").Value),
                    };

                foreach (var field in query)
                {
                    items.Add(field);
                }
            }
            catch (System.Xml.XmlException ex)
            {
                Console.WriteLine($" --- XML exception: {ex.Message}");
                File.Delete(fileName);
                return items;
            }

            return await Task.FromResult(items);
        }
    }
}
