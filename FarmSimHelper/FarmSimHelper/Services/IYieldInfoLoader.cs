using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FarmSimHelper.Models;

namespace FarmSimHelper.Services
{
    public interface IYieldInfoLoader
    {
        Task<IEnumerable<ProductYieldInfo>> LoadYieldInfo();
    }
}
