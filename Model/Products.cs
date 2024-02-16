using System.ComponentModel.DataAnnotations;

namespace MY_ORM.Model;

public class Products
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }


    private Products(string name ,decimal price)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
    }

    public static Products CreateProduct(string NewName , decimal Newprice)
    {
        Products NewProduct = new Products(NewName , Newprice);
        return NewProduct;
    }
}