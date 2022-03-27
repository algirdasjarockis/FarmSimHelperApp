using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace FarmSimHelper.Services
{
    public class DataDownloader : IDataDownloader
    {
        HttpClient client;

        public DataDownloader(HttpClient client)
        {
            this.client = client;
        }

        public async Task DownloadFile(string url, string targetPath)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var sourceStream = await client.GetStreamAsync(url);
            var writer = File.OpenWrite(targetPath);

            await sourceStream.CopyToAsync(writer);
            await writer.FlushAsync();
            writer.Close();

            Console.WriteLine($" -- Downloaded '{url}' to '{targetPath}'");
        }
    }
}
