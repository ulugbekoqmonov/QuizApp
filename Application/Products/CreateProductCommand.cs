using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Products;

public class CreateProductCommand:IRequest<IActionResult>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
}
