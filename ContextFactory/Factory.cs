using MY_ORM.DBContext;
using MY_ORM.Model;

namespace MY_ORM.ContextFactory;

public class Factory
{
    public DBSet<Products> products { get; set; } 
    public DBSet<Customers> customers { get; set; } 

    protected Factory()
    {
        
    }

    public static Factory CreateFactory()
    {
        Factory factory = new Factory();
        //factory.products = new DBSet<Products>(new Products());
        //factory.customers = new DBSet<Customers>(new Customers());
        
        return factory;
    }
}