using Catalog.API.Controllers;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Catalog.API.UnitTests.Controllers
{
    public class CatalogControllerTest
    {
        private readonly Mock<IProductRepository> _productRepository;
        private readonly Mock<ILogger<CatalogController>> _logger;
        private readonly CatalogController _catalogController;

        public CatalogControllerTest()
        {
            _productRepository = new Mock<IProductRepository>();
            _logger = new Mock<ILogger<CatalogController>>();
            _catalogController = new CatalogController(_productRepository.Object, _logger.Object);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnProducts_WhenProductsExist()
        {
            //Arrange
            Product[] products = GetProducts();

            _productRepository.Setup(x => x.GetProducts()).ReturnsAsync(products);

            //Act
            var result = (OkObjectResult)(await _catalogController.GetProducts()).Result;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(products, result.Value);
        }

        [Fact]
        public async Task GetProduct_ShouldReturnProduct_WhenProductWithCertainIdExists()
        {
            //Arrange
            var product = GetProducts().FirstOrDefault();

            _productRepository.Setup(x => x.GetProduct(It.IsAny<string>())).ReturnsAsync(product);

            //Act
            var result = (OkObjectResult)(await _catalogController.GetProduct(It.IsAny<string>())).Result;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(product, result.Value);
        }

        [Fact]
        public async Task GetProduct_ShouldReturnNotFound_WhenProductWithCertainIdDoesntExist()
        {
            //Arrange
            _productRepository.Setup(x => x.GetProduct(It.IsAny<string>())).ReturnsAsync(value: null);

            //Act
            var result = (NotFoundResult)(await _catalogController.GetProduct(It.IsAny<string>())).Result;

            //Assert
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task GetProductByName_ShouldReturnProducts_WhenProductsByNameExist()
        {
            //Arrange
            var products = GetProducts();

            _productRepository.Setup(x => x.GetProductByName(It.IsAny<string>())).ReturnsAsync(products);

            //Act
            var result = (OkObjectResult)(await _catalogController.GetProductByName(It.IsAny<string>())).Result;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(products, result.Value);
        }

        [Fact]
        public async Task GetProductByName_ShouldReturnNotFound_WhenProductsByNameDoesntExist()
        {
            //Arrange
            _productRepository.Setup(x => x.GetProductByName(It.IsAny<string>())).ReturnsAsync(value: null);

            //Act
            var result = (NotFoundResult)(await _catalogController.GetProductByName(It.IsAny<string>())).Result;

            //Assert
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task GetProductByCategory_ShouldReturnProducts_WhenProductsByCategoryExist()
        {
            //Arrange
            var products = GetProducts();

            _productRepository.Setup(x => x.GetProductByCategory(It.IsAny<string>())).ReturnsAsync(products);

            //Act
            var result = (OkObjectResult)(await _catalogController.GetProductByCategory(It.IsAny<string>())).Result;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(products, result.Value);
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnCreatedProduct_WhenCreateRequestIsValid()
        {
            //Arrange
            var product = GetProducts().FirstOrDefault();

            _productRepository.Setup(x => x.CreateProduct(It.IsAny<Product>()));

            //Act
            var result = (CreatedAtRouteResult)(await _catalogController.CreateProduct(product)).Result;

            //Assert
            Assert.Equal((int)HttpStatusCode.Created, result.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_ShouldUpdateProduct_WhenUpdateRequestIsValid()
        {
            //Arrange
            _productRepository.Setup(x => x.UpdateProduct(It.IsAny<Product>())).ReturnsAsync(value: true);

            //Act
            var result = (OkObjectResult)(await _catalogController.UpdateProduct(It.IsAny<Product>()));

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(true, result.Value);
        }

        [Fact]
        public async Task DeleteProduct_ShouldDeleteProduct_WhenProductWithCertainIdExists()
        {
            //Arrange
            _productRepository.Setup(x => x.DeleteProduct(It.IsAny<string>())).ReturnsAsync(value: true);

            //Act
            var result = (OkObjectResult)(await _catalogController.DeleteProduct(It.IsAny<string>()));

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(true, result.Value);
        }


        private static Product[] GetProducts()
        {
            return new[]
            {
                new Product()
                {
                    Name = "IPhone",
                    Category = "Smart phone",
                    Summary = "IPhone",
                    Description = "IPhone X",
                    ImageFile = "//iphone.png",
                    Price = 500
                }
            };
        }
    }
}
