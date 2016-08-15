using System.Collections.Generic;

public interface IRepository<T> where T : class
{
    string ErrorMessage { get; set; }
    bool HasError { get; set; }

    void Add(T entity);
    void Delete(int id);
    T Get(int id);
    List<T> GetAll();
    void Update(T entity);
    void SaveChanges();
    void Dispose();
}