using WebMVC.Extensions;
using WebMVC.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebMVC.Interfaces;

namespace WebMVC.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _client;

        public BasketService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<BasketModel> GetBasket(string userName)
        {
            var response = await _client.GetAsync($"{Constants.BASKET_REQUEST_URI}/{userName}");
            return await response.ReadContentAs<BasketModel>();
        }

        public async Task<BasketModel> UpdateBasket(BasketModel model)
        {
            var response = await _client.PostAsJson(Constants.BASKET_REQUEST_URI, model);
            if (response.IsSuccessStatusCode)
            {
                return await response.ReadContentAs<BasketModel>();
            }
            else
            {
                throw new Exception("Something went wrong when calling api.");
            }
        }

        public async Task CheckoutBasket(BasketCheckoutModel model)
        {
            var response = await _client.PostAsJson($"{Constants.BASKET_REQUEST_URI}/Checkout", model);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Something went wrong when calling api.");
            }
        }
    }
}