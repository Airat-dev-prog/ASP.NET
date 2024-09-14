using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using PromoCodeFactory.Core.Domain.Administration;
namespace PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T>: IRepository<T> where T: BaseEntity
    {
        protected IEnumerable<T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data;
        }

        public Task<T> Create(T item)
        {
            Data = Data.Append(item);
            return Task.FromResult(item);
        }
        
        public Task<T> Update(T item)
        {
            Data = Data.Select(i => i.Id == item.Id ? item : i);
            T newItem = Data.FirstOrDefault(x => x.Id == item.Id);
            return Task.FromResult(newItem);
        }

        public Task<T> Delete(Guid id)
        {
            T item = Data.FirstOrDefault(x => x.Id == id);
            Data = Data.Where(x => x.Id != id).ToList();
            return Task.FromResult(item);
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }
    }
}