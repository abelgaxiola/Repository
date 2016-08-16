using System;
using System.Data.Entity;
using System.Configuration;
using System.ComponentModel.DataAnnotations.Schema;

public class Context<T> : DbContext where T : class
{
    private string TableName;
    public bool HasError { get; set; }
    public string ErrorMessage { get; set; }

    public Context(string connectionString) : base(connectionString)
    {
        try
        {
            // Make sure that the connectionString exists
            var keyValue = ConfigurationManager.ConnectionStrings[connectionString].ToString();
            if (keyValue == null)
                throw new Exception("Invalid key value for ConnectionString was passed to Context constructor");

            var attribute = (TableAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(TableAttribute));

            TableName = attribute.Name;
        }
        catch (Exception exception)
        {
            // Let the calling process know there was an error
            HasError = true;
            ErrorMessage = exception.ToString();
        }
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Entity<T>().ToTable(TableName);
        base.OnModelCreating(modelBuilder);
    }
}