using System.Collections.Generic;

namespace ESBackupServer.Database.Repositories
{
    internal abstract class AbRepository<T>
    {
        protected DatabaseContext _Context { get; set; } = DatabaseContext.GetInstance();
        internal virtual void SaveChanges()
        {
            this._Context.SaveChanges();
        }

        internal abstract T Find(object id);
        internal abstract List<T> FindAll();
        protected abstract void Add(T item);
        internal abstract void Remove(T item);
        internal abstract void Update(T item);
    }
}