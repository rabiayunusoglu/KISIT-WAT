
using System.Collections.Generic;

namespace Constraint.DataAccessLayer.Repositories.Abstract
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity GetById(System.Guid id);
        IEnumerable<TEntity> GetAll();
        TEntity Add(TEntity entity);
        bool Remove(System.Guid id);
        bool RemoveAll(string tableName);
        bool RemoveNoEnteredDelay();
        bool CreateAllConstraintFromTheBegining();

        TEntity Update(TEntity entity);
    }
}
