using System.ComponentModel.DataAnnotations;

namespace MY_ORM.Model;

public class Products
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    
}