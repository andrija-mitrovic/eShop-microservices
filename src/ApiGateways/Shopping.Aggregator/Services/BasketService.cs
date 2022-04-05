using Shopping.Aggregator.DTOs;
using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;

        public BasketService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<BasketDto> GetBasket(string userName)
        {
            var response = await _httpClient.GetAsync($"{Constants.BASKET_REQUEST_URI}/{userName}");

            return await response.ReadContentAs<BasketDto>();
        }
    }
}
