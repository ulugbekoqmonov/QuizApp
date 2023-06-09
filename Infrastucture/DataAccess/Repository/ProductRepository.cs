using Application.Abstraction;
using Application.Interfaces;
using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.DataAccess.Repository;

public class ProductRepository:IProductRepository
{
    private readonly IApplicationDbContext _dbContext;

    public ProductRepository(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product> CreateAsync(Product entity)
    {        
        _dbContext.Products.Add(entity);
        Product? Product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Name == entity.Name);
        await _dbContext.SaveChangesAsync();
        return Product;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        Product? Product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        _dbContext.Products.Remove(Product);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public Task<IQueryable<Product>> GetAllAsync()
    {
        IQueryable<Product> queryable = _dbContext.Products;
        return Task.FromResult(queryable);
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        Product? product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        return product;
    }

    public async Task<Product> UpdateAsync(Product entity)
    {        
        _dbContext.Products.Update(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }
    public async Task<Product?> GetAsync(Expression<Func<Product, bool>> expression)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(expression);
    }

    public Task<IQueryable<Product>> GetAllAsync(Expression<Func<Product, bool>>? expression = null)
    {
        return Task.FromResult(_dbContext.Products.Where(expression));
    }
}
