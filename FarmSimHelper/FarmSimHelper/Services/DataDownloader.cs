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

        public async Task<bool> DownloadFile(string url, string targetPath)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                Directory.CreateDirectory(Path.GetDirectoryName(targetPath));

                var sourceStream = await client.GetStreamAsync(url);
                var writer = File.OpenWrite(targetPath);

                await sourceStream.CopyToAsync(writer);
                await writer.FlushAsync();
                writer.Close();

                Console.WriteLine($" -- Downloaded '{url}' to '{targetPath}'");
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }
    }
}
