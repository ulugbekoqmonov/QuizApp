﻿using System.Linq.Expressions;

namespace Application.Interfaces;

public interface IRepository<T>
{
    Task<List<T>> GetAllAsync();
    Task<T> GetByIdAsync(Guid Id);
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(Guid Id);    
}