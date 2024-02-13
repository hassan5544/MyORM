using System.Reflection;
using MY_ORM.DBContext;
using MY_ORM.Model;
namespace MY_ORM.ContextFactory;

public class Factory : ORM.DBContext
{
    public DBSet<Products> products { get; set; } 
    public DBSet<Customers> customers { get; set; }
    private List<Type> ListOfDbSet = new List<Type>();
    protected Factory(string conntectionString) : base(conntectionString)
    {
        
    }

    public static Factory CreateDBContext(string connectionstring )
    {
        Factory factory = new Factory(connectionstring);
        //factory.products = new DBSet<Products>(new Products());
        //factory.customers = new DBSet<Customers>(new Customers());
        
        string query = factory.CreateDatabaseIfNotExists(connectionstring);
        factory.ExecuteNonQuery(query);        
        PropertyInfo[] DbSets = typeof(Factory).GetProperties(BindingFlags.Public);
        foreach (var dbSet in DbSets)
        {
            var properties =  dbSet.PropertyType.GetProperties();
        }
        return factory;
    }
    
    
    private string CreateDatabaseIfNotExists(string _connectionString )
    {
        string dbName = _connectionString.Split(";").SingleOrDefault(x => x == "database");
        if (string.IsNullOrEmpty(dbName))
        {
            throw new ArgumentException("Invalid Connection string ");
        }

        var query = $"If(db_id(N'{dbName}') IS NULL) CREATE DATABASE [{dbName}]";
        
       return query;

    }

    
}