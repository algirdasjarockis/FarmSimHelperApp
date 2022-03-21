using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FarmSimHelper.Models;

namespace FarmSimHelper.Services
{
    public interface ISellPriceLoader
    {
        Task<IEnumerable<SellingPrice>> LoadSellingPrices(float baseFactor = 1.0f);
    }
}
