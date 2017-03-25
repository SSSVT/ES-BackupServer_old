namespace ESBackupServer.Database.Repositories
{
    internal abstract class AbRepository<T>
    {
        protected DatabaseContext _Context { get; set; } = DatabaseContext.GetInstance();

        internal abstract T FindByID(int id);
        internal abstract void Add(T item);
        internal abstract void Remove(T item);
        internal abstract void Update(T item);
    }
}