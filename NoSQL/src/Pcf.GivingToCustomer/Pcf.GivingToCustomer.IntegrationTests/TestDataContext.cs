using Microsoft.EntityFrameworkCore;
using Pcf.GivingToCustomer.DataAccess;

namespace Pcf.GivingToCustomer.IntegrationTests
{
    public class TestDataContext
        : Удалить_DataContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=PromocodeFactoryGivingToCustomerDb.sqlite");
        }
    }
}