using Application.Abstraction;
using Application.Exeptions;
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
        EntityEntry entry = _dbContext.Users.Add(entity);
        if(entry.State is EntityState.Added)
        {
            await _dbContext.SaveChangesAsync();
        }
        return (User)entry.Entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        bool result = false;
        User? user = _dbContext.Users.FirstOrDefault(a => a.Id == id);
        if (user is not null)
        {
            var entry = _dbContext.Users.Remove(user);
            result = entry.State == EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
            return result;
        }
        else
        {
            throw new NotFoundExeption();
        }
    }

    public Task<IQueryable<User>> GetAllAsync()
    {
        IQueryable<User> users = _dbContext.Users;
        return Task.FromResult(users);
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        User? user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user is not null)
        {
            return user;
        }
        else
        {
            throw new NotFoundExeption();
        }
    }

    public async Task<User> UpdateAsync(User entity)
    {
        EntityEntry entry = _dbContext.Users.Update(entity);
        if (entry.State == EntityState.Modified)
        {
            await _dbContext.SaveChangesAsync();
        }
        return (User)entry.Entity;
    }
    public async Task<User?> GetAsync(Expression<Func<User, bool>> expression)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(expression);
    }

    public Task<IQueryable<User>> GetByFilteringAsync(Expression<Func<User, bool>> expression)
    {
        IQueryable<User> users = _dbContext.Users.Where(expression);
        return Task.FromResult(users);
    }
}