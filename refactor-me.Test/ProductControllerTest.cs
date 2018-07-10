

using Moq;
using NUnit.Framework;
using refactor_me.Context;
using refactor_me.Controllers;
using refactor_me.Helpers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;

namespace refactor_me.Test
{
    [TestFixture]
    public class ProductControllerTest
    {

        public Mock<IProductService> mockProductService = new Mock<IProductService>();

        [SetUp]
        public void TestInitialize()
        {
            mockProductService = new Mock<IProductService>();           
        }

        [Test]
        public void Get_All_Returns_AllProducts()

        {            
            // Arrange 

            mockProductService.Setup(x => x.GetAll()).Returns(fakeProducts);
            ProductsController controller = new ProductsController(mockProductService.Object);
           
            // Act
            var result = controller.GetAll();

            // Assert
            Assert.IsNotNull(result, null);           

        }

        [Test]
        public void Get_All_Returns_AllProductOptions()

        {
            // Arrange 

            mockProductService.Setup(x => x.GetOptions(It.IsAny<Guid>())).Returns(fakeProductOptions);
            ProductsController controller = new ProductsController(mockProductService.Object);

            // Act
            var result = controller.GetOptions(new Guid("DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3"));

            // Assert
            Assert.IsNotNull(result, null);

        }


        [Test]
        public void Post_Product_Returns_CreatedStatusCode()
        {
            // Arrange 
            mockProductService.Setup(c => c.Create(It.IsAny<Product>()));              

            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "products" } });
            var controller = new ProductsController(mockProductService.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/products/")
                {
                    Properties =
                                {
                                    { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                                    { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
                                }
                }
            };

           // Act           

            var response = controller.Create(fakeProducts.Find(f => f.Id == _productId1));
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }

        [Test]
        public void Delete_Product_Returns_NoContentStatusCode()
        {
            // Arrange
            mockProductService.Setup(c => c.Delete(It.IsAny<Product>()));
            ProductsController controller = new ProductsController(mockProductService.Object)
            {
                Request = new HttpRequestMessage()
                {
                    Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } }
                }
            };
            // Act          
            var response = controller.Delete(_productId1);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }


        private static Guid _productId1 = Guid.NewGuid();
        private static Guid _productId2 = Guid.NewGuid();
        private static Guid _productId3 = Guid.NewGuid();

        List<Product> fakeProducts = new List<Product> {
                new Product { Id = _productId1 ,Name ="product1", DeliveryPrice =1, Description ="product",Price=45 },
                new Product { Id = _productId2 ,Name ="product2", DeliveryPrice =2, Description ="product",Price=34 },
                new Product { Id = _productId3 ,Name ="product3", DeliveryPrice =3, Description ="product",Price=12 },

            };

        private static Guid _productOptionId1 = Guid.NewGuid();
        private static Guid _productOptionId2 = Guid.NewGuid();
        private static Guid _productOptionId3 = Guid.NewGuid();
        private static Guid _productOptionId4 = Guid.NewGuid();

        List<ProductOption> fakeProductOptions = new List<ProductOption> {
                new ProductOption { Id = _productOptionId1 ,ProductId = _productId1, Name="product option1", Description="product option" },
                new ProductOption { Id = _productOptionId2 ,ProductId = _productId2, Name="product option2", Description="product option" },
                new ProductOption { Id = _productOptionId3 ,ProductId = _productId2, Name="product option2", Description="product option" },
                new ProductOption { Id = _productOptionId4 ,ProductId = _productId3, Name="product option3", Description="product option" }

            };
    }
}
