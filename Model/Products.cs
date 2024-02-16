using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MY_ORM.Model;

public class Products
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    [NotMapped]
    public List<int> Count { get; set; } // local use

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