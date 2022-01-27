using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkiWeb.Repository.Contract
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(string url, int Id);
        Task<TServiceResponce> GetAsyncServiceWrapper<TServiceResponce>(string url, int Id);
        Task<IEnumerable<T>> GetAllAsync(string url);
        Task<T> GetAllAsyncServiceWrapper<T>(string url);
       
        Task<bool> CreateAsync(string url, T ModelToCreate);
        Task<bool> UpdateAsync(string url, T ModelToUpdate);
        Task<bool> DeleteAsync(string url, int Id);
    }
}
