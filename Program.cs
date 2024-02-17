using System.Net.Http.Headers;
using MY_ORM.ContextFactory;
using MY_ORM.Model;
using MY_ORM.ORM;
using MY_ORM.Repositories;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "server=.; database=TestingORMCreation; Integrated Security=true";

        Factory.CreateDBContext(connectionString);
        Products products = Products.CreateProduct("item" , 1000);
        ProductsRepository repository = new ProductsRepository();
        repository.GetAll<Products>();
  
    }
}