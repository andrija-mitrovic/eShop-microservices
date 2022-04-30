using Discount.API.Controllers;
using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Discount.API.UnitTests
{
    public class DiscountControllerTest
    {
        private readonly Mock<IDiscountRepository> _discountRepository;
        private readonly DiscountController _discountController;

        public DiscountControllerTest()
        {
            _discountRepository = new Mock<IDiscountRepository>();
            _discountController = new DiscountController(_discountRepository.Object);
        }

        [Fact]
        public async Task GetDiscount_ShouldReturnDiscount_WhenDiscountWithCertainProductNameExists()
        {
            var coupon = GetCoupon();

            _discountRepository.Setup(x => x.GetDiscount(It.IsAny<string>())).ReturnsAsync(coupon);

            //Act
            var result = (OkObjectResult)(await _discountController.GetDiscount(It.IsAny<string>())).Result;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(coupon, result.Value);
        }

        [Fact]
        public async Task CreateDiscount_ShouldReturnOkResponse_WhenCreateRequestIsValid()
        {
            //Arrange
            var coupon = GetCoupon();

            _discountRepository.Setup(x => x.CreateDiscount(coupon));

            //Act
            var result = (CreatedAtRouteResult)(await _discountController.CreateDiscount(coupon)).Result;

            //Assert
            Assert.Equal((int)HttpStatusCode.Created, result.StatusCode);
        }

        [Fact]
        public async Task UpdateDiscount_ShouldUpdateDiscount_WhenUpdateRequestIsValid()
        {
            //Arrange
            _discountRepository.Setup(x => x.UpdateDiscount(It.IsAny<Coupon>())).ReturnsAsync(value: true);

            //Act
            var result = (OkObjectResult)(await _discountController.UpdateDiscount(It.IsAny<Coupon>())).Result;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(true, result.Value);
        }

        [Fact]
        public async Task DeleteDiscount_ShouldDeleteDiscount_WhenDiscountWhitCertainIdExists()
        {
            //Arrange
            _discountRepository.Setup(x => x.DeleteDiscount(It.IsAny<string>())).ReturnsAsync(value: true);

            //Act
            var result = (OkObjectResult)(await _discountController.DeleteDiscount(It.IsAny<string>())).Result;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(true, result.Value);
        }

        private static Coupon GetCoupon()
        {
            //Arrange
            return new Coupon()
            {
                ProductName = "IPhone",
                Description = "IPhone",
                Amount = 100
            };
        }
    }
}
