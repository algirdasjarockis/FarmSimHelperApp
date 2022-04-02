using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FarmSimHelper.Services
{
    public interface IDataLoader<T, P>
    {
        Task<IEnumerable<T>> LoadData(P param = default);
    }
}
