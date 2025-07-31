using System.Collections;
using System.Threading.Tasks;
using UPA.BLL.Interfaces;
using UPA.BLL.Repository;
using UPA.DAl;
using UPA.DAL.Models;

namespace UPA.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UPAModel _context;
        private Hashtable _repostories;

        public UnitOfWork(UPAModel context)
        {
            _context = context;
        }
        public async Task<int> Complete()
            => await _context.SaveChangesAsync();

        public void Dispose()
            => _context.Dispose();

        public IGenericRepo<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if(_repostories == null) _repostories = new Hashtable();

            var type = typeof(TEntity).Name;

            if(!_repostories.ContainsKey(type))
            {
                var repository = new GenericRepo<TEntity>(_context);
                _repostories.Add(type, repository);
            }

            return (IGenericRepo<TEntity>) _repostories[type];
        }

    }
}
