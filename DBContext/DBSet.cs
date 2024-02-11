using System.Reflection;
using MY_ORM.Model;

namespace MY_ORM.DBContext;

public class DBSet<T>
{
    private readonly T Entity; 

    public DBSet(T entity)
    {
        Entity = entity;
        AddMigrations(Entity);
    }

    public void AddMigrations(T Entity)
    {
        PropertyInfo[] properties = this.Entity.GetType().GetProperties();
        
        foreach (var property in properties)
        {
            var type = property.PropertyType.ToString();
        }
        
    }
}