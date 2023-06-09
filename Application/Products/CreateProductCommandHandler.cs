using Application.Interfaces;
using AutoMapper;
using Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Products;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, IActionResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<IActionResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        Product Product = _mapper.Map<Product>(request);
        Product product = await _productRepository.CreateAsync(Product);
        return new OkResult();
    }
}
