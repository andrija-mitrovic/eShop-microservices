using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration[Constants.MONGO_DB_CONNECTION_STRING]);
            var database = client.GetDatabase(configuration[Constants.MONGO_DB_DATABASE_NAME]);

            Products = database.GetCollection<Product>(configuration[Constants.MONGO_DB_COLLECTION_NAME]);
            CatalogContextSeed.SeedData(Products);
        }

        public IMongoCollection<Product> Products { get; }
    }
}
