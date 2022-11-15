using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomerProvider : ICustomerProvider
    {
        private readonly CustomersDbContext dbContext;
        private readonly ILogger<CustomerProvider> logger;
        private readonly IMapper mapper;

        public CustomerProvider(CustomersDbContext dbContext, ILogger<CustomerProvider> LOGGER, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = LOGGER;
            this.mapper = mapper;
            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Customers.Any())
            {
                dbContext.Customers.Add(new Db.Customer() { Id = 1, Name = "Aaron smith", Address = "123 John Street" });
                dbContext.Customers.Add(new Db.Customer() { Id = 2, Name = "John Doe", Address = "456 Peter Street" });
                dbContext.Customers.Add(new Db.Customer() { Id = 3, Name = "Mark Zuck", Address = "1122 Highway Street" });
                dbContext.Customers.Add(new Db.Customer() { Id = 4, Name = "Larry Bird", Address = "404 Not-Found Street" });
                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Customer> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                var customers = await dbContext.Customers.ToListAsync();
                if (customers != null && customers.Any())
                {
                    return (true, customers, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception exception)
            {
                logger?.LogError(exception.ToString());
                return (false, null, exception.Message);

            }
        }

        public async Task<(bool IsSuccess, Customer customer, string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                var customer = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id);

                if (customer != null)
                {
                    var result = mapper.Map<Db.Customer, Models.Customer>(customer);
                    return (true, customer, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
