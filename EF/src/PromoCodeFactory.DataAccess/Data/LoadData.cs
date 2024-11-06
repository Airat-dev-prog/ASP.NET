using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Domain.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Data
{
    public class LoadData : ILoadData
    {
        private readonly AppDbContext _appDbContext;
        public LoadData(AppDbContext dbContext) 
        {
            _appDbContext = dbContext;
        }
        public void LoadDataInDB()
        {
            _appDbContext.Database.EnsureDeleted();
            _appDbContext.Database.EnsureCreated();

            _appDbContext.AddRange(FakeDataFactory.Employees);
            _appDbContext.SaveChanges();

            _appDbContext.AddRange(FakeDataFactory.Preferences);
            _appDbContext.SaveChanges();

            _appDbContext.AddRange(FakeDataFactory.Customers);
            _appDbContext.SaveChanges();
        }
    }
}
