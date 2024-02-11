using System.Data.SqlClient;
using System.Reflection;

namespace MY_ORM.ORM;

public class ORM
{
    private readonly string _connectionString;

    private ORM(string connectionString)
    {
        _connectionString = connectionString;
    }

    public static ORM CreateNewORM(string connectionstring)
    {
        if (string.IsNullOrEmpty(connectionstring))
            throw new ArgumentException("Please set connection string");
        return new ORM(connectionstring);
    }
    #region private functions

    private void ValidateBaseEo<T>(T baseEo)
    {
        if (baseEo == null)
            throw new ArgumentException("nullable value cant be inserted");
        if (typeof(T).GetProperties().Where(x => x.Name == "Id").Count() == 0)
            throw new AggregateException("Entity must have a primary key Named Id");
    }
    
    private  string GetTableName<T>()
    {
        return typeof(T).Name;
    }

    private PropertyInfo[]  GetProperties<T>()
    {
        return typeof(T).GetProperties();
    }
    private void ExecuteNonQuery(string query)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    
    private List<T> ExecuteQuery<T>(string query)
    {
        List<T> result = new List<T>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                T item = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in typeof(T).GetProperties())
                {
                    prop.SetValue(item, Convert.ChangeType(reader[prop.Name], prop.PropertyType));
                }
                result.Add(item);
            }
            connection.Close();
        }

        return result;
    }
    #endregion

    #region CRUD
    public void Insert<T>(T baseEo)
    {
        ValidateBaseEo(baseEo);
        try
        {
            string tableName = GetTableName<T>();
            string columns = string.Join(",", typeof(T).GetProperties().ToString());
            string values  = string.Join(",", typeof(T).GetProperties().Select(p => $"'{p.GetValue(baseEo)}'"));
        
            string query = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";
            ExecuteNonQuery(query);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }


    }

    public void Update<T>(T baseEo)
    {
        ValidateBaseEo(baseEo);
        try
        {
            string tableName = GetTableName<T>();
            string primaryKey = "Id";
            var properties = typeof(T).GetProperties();

            string setStatment = string.Join(",", properties.Where(x => x.Name != primaryKey).Select(p => $"{p.Name} = '{p.GetValue(baseEo)}'"));
        
            string query = $"UPDATE {tableName} SET {setStatment} WHERE {primaryKey}='{properties.Single(p => p.Name == primaryKey).GetValue(baseEo)}'";
            ExecuteNonQuery(query);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void Delete<T>(T baseEo)
    {
        ValidateBaseEo(baseEo);
        try
        {
            string tableName = GetTableName<T>();
            string primaryKey = "Id";
            var properties = typeof(T).GetProperties();

            string query = $"DELETE FROM {tableName} WHERE Id = {properties.Single(p => p.Name == primaryKey).GetValue(baseEo)}";
            ExecuteNonQuery(query);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public List<T> GetAll<T>()
    {
        try
        {
            string tableName = GetTableName<T>();
            string query = $"SELECT * FROM {tableName}";
        
            return ExecuteQuery<T>(query);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        
    }

    public  bool CheckIfTableExists<T>()
    {
        string tableName = GetTableName<T>();
        string query = $"SELECT Object_id FROM sys.tables WHERE name = '{tableName}'";
        return (bool)ExcuteScalar(query);
    }
    private  object ExcuteScalar(string query)
    {
        object result;
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                conn.Open();
                result = cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        return result;
    }
    #endregion




}