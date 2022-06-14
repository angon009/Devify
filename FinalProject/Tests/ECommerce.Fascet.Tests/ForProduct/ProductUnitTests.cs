using Autofac.Extras.Moq;
using ECommerce.Fascet.ForProduct;
using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.UnitOfWorks;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Threading.Tasks;

namespace ECommerce.Fascet.Tests.ForProduct
{
    public class ProductUnitTests
    {
        private AutoMock _mock;
        private Mock<IEcommerceUnitOfWork> _ecommerceUnitOfWorkMock;
        private IProductUnit _productUnit;


        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose(); // Initializing the mock object 
        }
        [SetUp]
        public void Setup()
        {
            _ecommerceUnitOfWorkMock = _mock.Mock<IEcommerceUnitOfWork>();
            _productUnit = _mock.Create<ProductUnit>();
        }
        [TearDown]
        public void TearDown()
        {
            //Resetting mock after each test
            _ecommerceUnitOfWorkMock.Reset();
        }
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose(); // Disposing the object 
        }
        #region Asynchronous Methods Tests
        [Test]
        public async Task CreateServiceAsync()
        {
            //Arrange
            Product product = new Product
            {
                Name = "Tooth Brush Set",

                SalePrice = 1200,
                CostPrice = 900,

                Model = "",
                Brand = "",
                ExpireDate = DateTime.Now,
                ManufactureDate = DateTime.Now,
                Weight = "",
                Size = "",
                ProductDetails = ""

            };

            _ecommerceUnitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.FromResult(true)).Verifiable();


            //Act
            await _productUnit.CreateServiceAsync(product);

            //Assert
            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );

        }
        [Test]
        public async Task UpdateServiceAsync()
        {
            //Arrange
            Product product = new Product
            {
                Name = "Tooth Brush Set",

                SalePrice = 1200,
                CostPrice = 900,

                Model = "",
                Brand = "",
                ExpireDate = DateTime.Now,
                ManufactureDate = DateTime.Now,
                Weight = "",
                Size = "",
                ProductDetails = ""

            };

            _ecommerceUnitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.FromResult(true)).Verifiable();


            //Act
            await _productUnit.UpdateServiceAsync(product);

            //Assert
            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );

        }
        [Test]
        public async Task DeleteServiceAsync()
        {
            //Arrange
            int id = 1;

            _ecommerceUnitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.FromResult(true)).Verifiable();


            //Act
            await _productUnit.DeleteServiceAsync(id);

            //Assert
            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );

        }
        #endregion

        #region Non-Asynchronous Methods Tests
        [Test]
        public void CreateService()
        {
            //Arrange
            Product product = new Product
            {
                Name = "Tooth Brush Set",

                SalePrice = 1200,
                CostPrice = 900,

                Model = "",
                Brand = "",
                ExpireDate = DateTime.Now,
                ManufactureDate = DateTime.Now,
                Weight = "",
                Size = "",
                ProductDetails = ""

            };

            _ecommerceUnitOfWorkMock.Setup(x => x.Save()).Verifiable();


            //Act
            _productUnit.CreateService(product);

            //Assert
            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );

        }
        [Test]
        public void UpdateService()
        {
            //Arrange
            Product product = new Product
            {
                Name = "Tooth Brush Set",

                SalePrice = 1200,
                CostPrice = 900,

                Model = "",
                Brand = "",
                ExpireDate = DateTime.Now,
                ManufactureDate = DateTime.Now,
                Weight = "",
                Size = "",
                ProductDetails = ""

            };

            _ecommerceUnitOfWorkMock.Setup(x => x.Save()).Verifiable();


            //Act
            _productUnit.UpdateService(product);

            //Assert
            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );

        }
        [Test]
        public void DeleteService()
        {
            //Arrange
            int id = 1;

            _ecommerceUnitOfWorkMock.Setup(x => x.Save()).Verifiable();


            //Act
            _productUnit.DeleteService(id);

            //Assert
            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );

        }
        #endregion
    }
}
