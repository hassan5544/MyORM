using MY_ORM.DBContext;
using MY_ORM.Model;

namespace MY_ORM.ContextFactory;

public class Factory
{
    public DBSet<Products> products { get; set; }
    public DBSet<Customers> customers { get; set; }

    
}