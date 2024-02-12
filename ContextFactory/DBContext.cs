namespace MY_ORM.ContextFactory;

public class DBContext
{
    public void CreateMigrations()
    {
        Factory factory = Factory.CreateFactory();

        foreach (var VARIABLE in typeof(Factory).GetProperties())
        {
            
        }
    }
}