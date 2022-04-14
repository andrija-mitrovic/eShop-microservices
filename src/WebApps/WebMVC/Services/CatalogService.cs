using WebMVC.Extensions;
using WebMVC.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebMVC.Interfaces;

namespace WebMVC.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _client;

        public CatalogService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalog()
        {
            var response = await _client.GetAsync(Constants.CATALOG_REQUEST_URI);
            return await response.ReadContentAs<List<CatalogModel>>();
        }

        public async Task<CatalogModel> GetCatalog(string id)
        {
            var response = await _client.GetAsync($"{Constants.CATALOG_REQUEST_URI}/{id}");
            return await response.ReadContentAs<CatalogModel>();
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category)
        {
            var response = await _client.GetAsync($"{Constants.CATALOG_REQUEST_URI}/GetProductByCategory/{category}");
            return await response.ReadContentAs<List<CatalogModel>>();
        }

        public async Task<CatalogModel> CreateCatalog(CatalogModel model)
        {
            var response = await _client.PostAsJson(Constants.CATALOG_REQUEST_URI, model);
            if (response.IsSuccessStatusCode)
            {
                return await response.ReadContentAs<CatalogModel>();
            }
            else
            {
                throw new Exception("Something went wrong when calling api.");
            }
        }
    }
}