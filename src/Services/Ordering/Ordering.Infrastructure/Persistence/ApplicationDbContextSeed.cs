using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext dbContext, ILogger<ApplicationDbContextSeed> logger)
        {
            if (!dbContext.Orders.Any())
            {
                dbContext.Orders.AddRange(GetPreconfiguredOrders());
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(ApplicationDbContext).Name);
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order(){UserName="swn", FirstName="Andrija", LastName="Mitrovic", EmailAddress="andrija@gmail.com", AddressLine="Herceg Novi", Country="Montenegro", TotalPrice=350 }
            };
        }
    }
}
