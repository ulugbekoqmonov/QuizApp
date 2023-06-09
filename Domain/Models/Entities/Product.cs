using Domain.Models.Common;

namespace Domain.Models.Entities;

public class Product:BaseAuditableEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
}
