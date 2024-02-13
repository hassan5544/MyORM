namespace MY_ORM.Repositories;

public class ProductsRepository : ORM.DBContext
{
    public ProductsRepository(string connectionString) : base(connectionString)
    {
    }
}