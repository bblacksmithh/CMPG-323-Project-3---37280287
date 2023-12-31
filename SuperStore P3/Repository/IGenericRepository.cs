﻿namespace EcoPower_Logistics.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task Update(T entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}