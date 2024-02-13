using MY_ORM.ContextFactory;
using MY_ORM.Model;
using MY_ORM.ORM;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "server=.; database=Hackatthon-Watch-Party; Integrated Security=true";
        //ORM orm = ORM.CreateNewOrm(connectionString);

        //Factory.CreateDBContext(connectionString, orm);

        //orm.Insert(Products.CreateProduct("Item" , 9500));

        //List<Products> products = orm.GetAll<Products>();
        //
        //foreach (var product in products)
        //{
        //    Console.WriteLine($"ID: {product.Id}, Name: {product.Name}");
        //}

        //var ProductToUpdate = products[0];
        
        //ProductToUpdate.Name = "Updated Name";
        
        //orm.Update(ProductToUpdate);

        //orm.Delete(products[0]);
    }
}