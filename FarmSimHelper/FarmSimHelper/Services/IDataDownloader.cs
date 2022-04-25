using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FarmSimHelper.Services
{
    public interface IDataDownloader
    {
        Task<bool> DownloadFile(string url, string targetPath);
    }
}
