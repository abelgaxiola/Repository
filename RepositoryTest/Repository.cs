using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

public class Repository<T> : IDisposable, IRepository<T> where T : class
{
    public bool HasError { get; set; }
    public string ErrorMessage { get; set; }
    private DbSet<T> DbSet { get; set; }
    private DbContext DbContext = null;

    public Repository(string connectionString)
    {
        try
        {
            var context = new Context<T>(connectionString);

            if (context.HasError)
                throw new Exception(context.ErrorMessage);

            DbContext = context;
            DbSet = DbContext.Set<T>();
        }
        catch (Exception exception)
        {
            SetErrorMessageAndFlag(exception.ToString());
        }
    }

    public List<T> GetAll()
    {
        try
        {
            return DbSet.ToList();
        }
        catch (Exception exception)
        {
            SetErrorMessageAndFlag(exception.ToString());
            return new List<T>();
        }
    }

    public T Get(int id)
    {
        try
        {
            var entity = DbSet.Find(id);
            // If found return the record, else return a new instance of the same type
            if (entity != null)
                return entity;

            return GetNew();
        }
        catch (Exception exception)
        {
            SetErrorMessageAndFlag(exception.ToString());
            return GetNew();
        }
    }

    public T GetNew()
    {
        try
        {
            var entity = (T)Activator.CreateInstance(typeof(T));
            return entity;
        }
        catch (Exception exception)
        {
            SetErrorMessageAndFlag(exception.ToString());
            return null;
        }
    }

    public void Add(T entity)
    {
        try
        {
            DbSet.Add(entity);
        }
        catch (Exception exception)
        {
            SetErrorMessageAndFlag(exception.ToString());
        }
    }

    public virtual void Update(T entity)
    {
        try
        {
            DbContext.Entry(entity).State = EntityState.Modified;
        }
        catch (Exception exception)
        {
            SetErrorMessageAndFlag(exception.ToString());
        }
    }

    public void Delete(int id)
    {
        try
        {
            DbSet.Remove(DbSet.Find(id));
        }
        catch (Exception exception)
        {
            SetErrorMessageAndFlag(exception.ToString());
        }
    }

    public void SaveChanges()
    {
        try
        {
            DbContext.SaveChanges();
        }
        catch (Exception exception)
        {
            SetErrorMessageAndFlag(exception.ToString());
        }
    }

    private bool disposed = false;
    public void Dispose()
    {
        if (!disposed)
        {
            if (DbContext != null)
                DbContext.Dispose();

            disposed = true;
        }
    }

    private void SetErrorMessageAndFlag(string message)
    {
        // Set the error flag and the error message to let calling process know
        HasError = true;
        ErrorMessage = message;
    }

}// End Repository class