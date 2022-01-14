using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Antalaktiko.Services
{
    public interface IDataManger<T>
    {
        Task<IEnumerable<T>> GetAll();
    }
}
