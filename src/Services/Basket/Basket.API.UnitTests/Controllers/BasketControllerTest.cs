using AutoMapper;
using Basket.API.Controllers;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Basket.API.UnitTests.Controllers
{
    public class BasketControllerTest
    {
        private readonly Mock<IBasketRepository> _basketRepository;
        private readonly Mock<IDiscountGrpcService> _discountGrpcService;
        private readonly Mock<IPublishEndpoint> _publishEndpoint;
        private readonly Mock<IMapper> _mapper;
        private readonly BasketController _basketController;
        
        public BasketControllerTest()
        {
            _basketRepository = new Mock<IBasketRepository>();
            _discountGrpcService = new Mock<IDiscountGrpcService>();
            _publishEndpoint = new Mock<IPublishEndpoint>();
            _mapper = new Mock<IMapper>();

            _basketController = new BasketController(
                _basketRepository.Object,
                _discountGrpcService.Object,
                _publishEndpoint.Object,
                _mapper.Object);
        }

        [Fact]
        public async Task GetBasketByIdAsync_ShouldReturnEmptyBasket_WhenBasketWithCertainUsernameDoesntExist()
        {
            //Arrange
            _basketRepository.Setup(x => x.GetBasket(It.IsAny<string>())).ReturnsAsync(value: null);

            //Act
            var result = await _basketController.GetBasket(It.IsAny<string>());

            //Assert
            Assert.Equal((result.Result as OkObjectResult).StatusCode, (int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetBasketByIdAsync_ShouldReturnBasket_WhenBasketWithCertainUsernameExists()
        {
            //Arrange
            var basket = GetShoppingCart();

            _basketRepository.Setup(x => x.GetBasket(It.IsAny<string>())).ReturnsAsync(await Task.FromResult(basket));

            //Act
            var result = await _basketController.GetBasket(basket.UserName);

            //Assert
            Assert.Equal((result.Result as OkObjectResult).StatusCode, (int)HttpStatusCode.OK);
            Assert.Equal((((ObjectResult)result.Result).Value as ShoppingCart).UserName, basket.UserName);
        }

        [Fact]
        public async Task UpdateBasket_ShouldUpdateBasket_WhenBasketRequestIsValid()
        {
            //Arrange
            var basket = GetShoppingCart();
            CouponModel couponModel = GetCouponModel();

            _discountGrpcService.Setup(x => x.GetDiscount(It.IsAny<string>())).ReturnsAsync(couponModel);
            _basketRepository.Setup(x => x.UpdateBasket(It.IsAny<ShoppingCart>())).ReturnsAsync(basket);

            //Act
            var result = (OkObjectResult)(await _basketController.UpdateBasket(basket)).Result;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(basket, result.Value);
        }

        private static CouponModel GetCouponModel()
        {
            return new CouponModel()
            {
                Id = 1,
                ProductName = "IPhone",
                Description = "IPhone X",
                Amount = 50
            };
        }

        [Fact]
        public async Task DeleteBasket_ShouldReturnOKResponse_WhenBasketWithCertainUsernameExists()
        {
            //Arrange
            _basketRepository.Setup(x => x.DeleteBasket(It.IsAny<string>()));

            //Act
            var result = (OkResult)await _basketController.DeleteBasket(It.IsAny<string>());

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task Checkout_ShouldReturnAccepted_WhenBasketCheckoutRequestIsValid()
        {
            //Arrange
            BasketCheckout basketCheckout = GetBasketCheckout();
            ShoppingCart basket = GetShoppingCart();
            BasketCheckoutEvent basketChekoutEvent = GetBasketChekoutEvent();

            _basketRepository.Setup(x => x.GetBasket(It.IsAny<string>())).ReturnsAsync(basket);
            _mapper.Setup(x => x.Map<BasketCheckoutEvent>(It.IsAny<BasketCheckout>())).Returns(basketChekoutEvent);
            _basketRepository.Setup(x => x.DeleteBasket(It.IsAny<string>()));

            //Act
            var result = (AcceptedResult)await _basketController.Checkout(basketCheckout);

            //Assert
            Assert.Equal((int)HttpStatusCode.Accepted, result.StatusCode);
        }

        private static BasketCheckoutEvent GetBasketChekoutEvent()
        {
            var basketCheckout = GetBasketCheckout();

            return new BasketCheckoutEvent()
            {
                UserName = basketCheckout.UserName,
                TotalPrice = basketCheckout.TotalPrice,
                FirstName = basketCheckout.FirstName,
                LastName = basketCheckout.LastName,
                EmailAddress = basketCheckout.EmailAddress,
                AddressLine = basketCheckout.AddressLine,
                Country = basketCheckout.Country,
                State = basketCheckout.State,
                ZipCode = basketCheckout.ZipCode,
                CardName = basketCheckout.CardName,
                CardNumber = basketCheckout.CardNumber,
                Expiration = basketCheckout.Expiration,
                CVV = basketCheckout.CVV,
                PaymentMethod = basketCheckout.PaymentMethod
            };
        }

        private static BasketCheckout GetBasketCheckout()
        {
            return new BasketCheckout()
            {
                UserName = "andrija",
                TotalPrice = 500,
                FirstName = "Andrija",
                LastName = "Mitrovic",
                EmailAddress = "andrija@gmail.com",
                AddressLine = "Herceg Novi",
                Country = "Montenegro",
                State = "Montenegro",
                ZipCode = "85340",
                CardName = "Visa",
                CardNumber = "1212121212121212",
                Expiration = "05/22",
                CVV = "111",
                PaymentMethod = 1
            };
        }

        private static ShoppingCart GetShoppingCart()
        {
            return new ShoppingCart()
            {
                UserName = "andrija",
                Items = new List<ShoppingCartItem>()
                {
                    new ShoppingCartItem()
                    {
                        Quantity = 1,
                        Color = "White",
                        Price = 500,
                        ProductId = Guid.NewGuid().ToString(),
                        ProductName = "IPhone"
                    }
                }
            };
        }
    }
}
