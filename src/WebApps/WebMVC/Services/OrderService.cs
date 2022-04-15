using WebMVC.Extensions;
using WebMVC.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebMVC.Interfaces;

namespace WebMVC.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _client;

        public OrderService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName)
        {
            var response = await _client.GetAsync($"{Constants.ORDER_REQUEST_URI}/{userName}");
            return await response.ReadContentAs<List<OrderResponseModel>>();
        }
    }
}