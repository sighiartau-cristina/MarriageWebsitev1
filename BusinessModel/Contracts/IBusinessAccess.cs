using System.Collections.Generic;

namespace BusinessModel.Contracts
{
    interface IBusinessAccess<T>
    {
        ResponseEntity<T> Add(T entity);

        ResponseEntity<T> Delete(int id);

        ResponseEntity<T> Update(T entity);

        ResponseEntity<T> Get(int id);

        ResponseEntity<ICollection<T>> GetAll();

    }
}
