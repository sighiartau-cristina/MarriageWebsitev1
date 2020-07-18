using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Contracts
{
    interface IDataAccess<T>
    {
        void Add(T entity);

        void Delete(int id);

        void Update(T entity);

        T Get(int id);

    }
}
