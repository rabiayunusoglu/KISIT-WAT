using Constraint.DataAccessLayer.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.DataAccessLayer.Repositories.Concrete
{
    public class Repository<E> : IRepository<E> where E : class
    {
        protected DbContext _context;
        private DbSet<E> _dbSet;
        public Repository(DbContext dbContext)
        {
            _context = dbContext;
            _dbSet = _context.Set<E>();
        }

        public E Add(E entity) => _dbSet.Add(entity);

        public IEnumerable<E> GetAll()
        {
            return _dbSet.ToList();
        }

        public E GetById(System.Guid id)
        {
            E value= _dbSet.Find(id);
            if (value == null)
            {
                return null;
            }
            return value;
        }

        public bool Remove(System.Guid id)
        {
            E deleted = GetById(id);
            if (deleted != null)
            {
                _dbSet.Remove(deleted);
                return true;
            }
            return false;
        }
        public bool RemoveAll(string tableName)
        {
            /* IEnumerable<E> deleted = GetAll();
             if (deleted != null)
             {
                 foreach (E del in deleted)
                 {
                     _dbSet.Remove(del);
                 }
                 return true;
             }
             return false;*/
            try
            {
                _context.Database.ExecuteSqlCommand("DELETE FROM [" + tableName+"]");
                return true;
            }
            catch(Exception e)
            {
                var t = e.Message;
                Console.WriteLine(t);
                return false;
            }
           
        }
        public bool CreateAllConstraintFromTheBegining()
        {
            try
            {
                _context.Database.ExecuteSqlCommand("INSERT INTO [Constraint] ([materialCode],[materialText],[productCode],[plannedDate],[amount],[customer],[version],[aboveLine],[treeAmount],[mip],[tob]) select p.materialCode,p.materialText, m.productCode, m.plannedDate, m.amount,m.customerType, m.[version],m.aboveLine,p.amount,p.mip,p.tob from [MontageProductPlan] m inner join [ProductTree] p on m.productCode = p.productCode");
                return true;
            }
            catch (Exception e)
            {
                var t = e.Message;
                Console.WriteLine(t);
                return false;
            }

        }
        public bool RemoveNoEnteredDelay()
        {
            try
            {
                _context.Database.ExecuteSqlCommand("DELETE FROM [Constraint] WHERE isDelayEntered = 0");
                return true;
            }
            catch (Exception e)
            {
                var t = e.Message;
                Console.WriteLine(t);
                return false;
            }

        }
        public E Update(E entity)
        {
            if (entity == null)
            {
                return null;
            }
            _context.Entry(entity).State = System.Data.EntityState.Modified;
            return entity;
        }
    }
}
