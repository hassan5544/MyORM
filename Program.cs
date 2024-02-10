using MY_ORM.Model;
using MY_ORM.ORM;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "";
        ORM orm = ORM.CreateNewORM(connectionString);

        orm.Insert(new Products() { Id = new Guid(), Name = "test 1" , Price = 1000});

        List<Products> products = orm.GetAll<Products>();
        foreach (var product in products)
        {
            Console.WriteLine($"ID: {product.Id}, Name: {product.Name}");
        }

        var ProductToUpdate = products[0];
        ProductToUpdate.Name = "Updated Name";
        orm.Update(ProductToUpdate);

        orm.Delete(products[0]);
    }
}