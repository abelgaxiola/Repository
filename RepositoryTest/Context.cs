using System;
using System.Data.Entity;
using System.Configuration;

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
            // Adding the letter "s" to pluralize the table
            TableName = typeof(T).Name + "s";
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