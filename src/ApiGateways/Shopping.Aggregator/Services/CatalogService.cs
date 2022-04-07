using Shopping.Aggregator.DTOs;
using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<CatalogDto>> GetCatalog()
        {
            var response = await _httpClient.GetAsync(Constants.CATALOG_REQUEST_URI);

            return await response.ReadContentAs<IEnumerable<CatalogDto>>();
        }

        public async Task<CatalogDto> GetCatalog(string id)
        {
            var response = await _httpClient.GetAsync($"{Constants.CATALOG_REQUEST_URI}/{id}");

            return await response.ReadContentAs<CatalogDto>();
        }

        public async Task<IEnumerable<CatalogDto>> GetCatalogByCategory(string category)
        {
            var response = await _httpClient.GetAsync($"{Constants.CATALOG_REQUEST_URI}/GetProductByCategory/{category}");

            return await response.ReadContentAs<IEnumerable<CatalogDto>>();
        }
    }
}
