using Application.Abstraction;
using Application.Extersion;
using Application.Interfaces;
using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Infrastructure.DataAccess.Repository;

public class UserRepository : IUserRepository
{
    private readonly IApplicationDbContext _dbContext;

    public UserRepository(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> CreateAsync(User entity)
    {
        entity.Password = entity.Password.ComputeHash();
        _dbContext.Users.Add(entity);
        User? user = await _dbContext.Users.FirstOrDefaultAsync(u=>u.UserName== entity.UserName);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        User? user = _dbContext.Users.FirstOrDefault(a => a.Id == id);
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();        
        return true;
    }

    public Task<IQueryable<User>> GetAllAsync()
    {
        IQueryable<User> queryable = _dbContext.Users;
        return Task.FromResult(queryable);
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        User? account = _dbContext.Users.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(account);
    }

    public async Task<User> UpdateAsync(User entity)
    {
        entity.Password = entity.Password.ComputeHash();
       _dbContext.Users.Update(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }
    public async Task<User?> GetAsync(Expression<Func<User, bool>> expression)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(expression);
    }

    public Task<IQueryable<User>> GetAllAsync(Expression<Func<User, bool>>? expression = null)
    {
        return Task.FromResult(_dbContext.Users.Where(expression));
    }
}