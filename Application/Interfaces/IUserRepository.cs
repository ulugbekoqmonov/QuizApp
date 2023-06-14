using Domain.Models.Entities;
using System.Linq.Expressions;

namespace Application.Interfaces;

public interface IUserRepository:IRepository<User>
{
    public Task<IQueryable<User>> GetByFilteringAsync(Expression<Func<User, bool>> expression);
}