using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Ordering.API.Controllers;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Ordering.API.UnitTests.COntrollers
{
    public class OrderControllerTest
    {
        private readonly OrderController _orderController;
        private readonly ISender _sender = Substitute.For<IMediator>();

        public OrderControllerTest()
        {
            _orderController = new OrderController();
        }

        [Fact]
        public async Task GetOrdersByUserName_ShouldReturnOrders_WhenOrdersExist()
        {
            //Arrange
            var orders = GetOrders();
            var userName = orders.FirstOrDefault().UserName;
            var query = new GetOrdersListQuery(userName);

            await _sender.Send(query);

            //Act
            var result = (OkObjectResult)(await _orderController.GetOrdersByUserName(userName)).Result;

            //Assert
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(orders);
        }

        private new List<Order> GetOrders()
        {
            return new List<Order>
            {
                new Order
                {
                    UserName="andrija",
                    TotalPrice=500,
                    FirstName="Andrija",
                    LastName="Mitrovic",
                    EmailAddress="andrija@gmail.com",
                    AddressLine="Jadranski put 48",
                    Country="Montenegro",
                    State="Montenegro",
                    ZipCode="8547",
                    CardName="Visa",
                    CardNumber="111111111111",
                    Expiration="10/22",
                    CVV="888",
                    PaymentMethod=PaymentMethod.Card
                }
            };
        }
    }
}
