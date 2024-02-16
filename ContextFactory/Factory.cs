using System.Data.SqlClient;
using System.Reflection;
using MY_ORM.DBContext;
using MY_ORM.Model;
namespace MY_ORM.ContextFactory;

public class Factory : ORM.DBContext
{
    public DBSet<Products> products { get; set; } = new DBSet<Products>();
    public DBSet<Customers> customers { get; set; } = new DBSet<Customers>();

    private static string _connectionStringforCreatingDatabase;
    public static void CreateDBContext(string connectionstring)
    {
        Factory factory = new Factory();
        factory.NewDBContext(connectionstring);
        _connectionStringforCreatingDatabase = Factory.CreateNewDatabaseConnectionString(connectionstring);
        
        string query = factory.CreateDatabaseIfNotExists(_connectionStringforCreatingDatabase);
        Factory.ExcuteQueryForCreateDatabase(query);

        PropertyInfo[] DbSets = typeof(Factory).GetProperties();
        foreach (var dbSet in DbSets)
        {
            var TableName = dbSet.Name;
            var properties =  dbSet.PropertyType.GetGenericArguments().FirstOrDefault().GetProperties();
            if (!factory.CheckTable(TableName))
            {
                string CreateTableQuery = $"CREATE TABLE {TableName} (";
                foreach (var property in properties)
                {
                    string PropertyName = "";
                    switch (property.PropertyType.Name)
                    {
                        case"String":
                            PropertyName = "Varchar(100)";
                            break;
                        case "char":
                            PropertyName = "Varchar(1)";
                            break;
                        case "Guid":
                            PropertyName = "uniqueidentifier";
                            break;
                        case "Decimal":
                            PropertyName = "Decimal(19,6)";
                            break;
                        default:
                            throw new ArgumentException("unhandeled property name ", property.PropertyType.Name);
                            
                    }
                    CreateTableQuery += property.Name + " " + PropertyName + ",";
                }

                CreateTableQuery += ")";
                CreateTableQuery = CreateTableQuery.Replace(",)", ")");
                
                factory.ExecuteNonQuery(CreateTableQuery);
                
            }
        }
    }

    private bool CheckTable(string tableName)
    {
        string query = $"IF EXISTS (SELECT 1 FROM sys.Tables WHERE Name = '{tableName}') SELECT CAST(1 as bit)  ELSE SELECT CAST(0 as bit)";
        return (bool)ExcuteScalar(query);
    }
    private static void ExcuteQueryForCreateDatabase(string query)
    {
        using (SqlConnection connection = new SqlConnection(_connectionStringforCreatingDatabase))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
    } 

    private static string CreateNewDatabaseConnectionString(string connectionString)
    {
         return connectionString.Replace(connectionString.Split(";").Where(x => x.Contains("database")).SingleOrDefault() ,"database = master");
        

    }
    private string CreateDatabaseIfNotExists(string _connectionString )
    {
        string dbName = _connectionString.Split(";").SingleOrDefault(x => x.Contains("database"));
        dbName = dbName.Replace(" database=" , "");
        if (string.IsNullOrEmpty(dbName))
        {
            throw new ArgumentException("Invalid Connection string ");
        }

        var query = $"If(db_id(N'{dbName}') IS NULL) CREATE DATABASE [{dbName}]";
        
       return query;

    }

    
}