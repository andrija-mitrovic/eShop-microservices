using Shopping.Aggregator.DTOs;
using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<OrderResponseDto>> GetOrdersByUserName(string userName)
        {
            var response = await _httpClient.GetAsync($"{Constants.ORDER_REQUEST_URI}/{userName}");

            return await response.ReadContentAs<IEnumerable<OrderResponseDto>>();
        }
    }
}
