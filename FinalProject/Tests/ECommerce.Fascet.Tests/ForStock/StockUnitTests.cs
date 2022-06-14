using Autofac.Extras.Moq;
using ECommerce.Fascet.ForStock;
using ECommerce.Infrastructure.BusinessObjects.Stores;
using ECommerce.Infrastructure.UnitOfWorks;
using Moq;
using NUnit.Framework;
using Shouldly;
using System.Threading.Tasks;

namespace ECommerce.Fascet.Tests.ForStock
{
    public class StockUnitTests
    {
        private AutoMock _mock;
        private Mock<IEcommerceUnitOfWork> _ecommerceUnitOfWorkMock;
        private IStockUnit _stockUnit;


        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose(); // Initializing the mock object 
        }
        [SetUp]
        public void Setup()
        {
            _ecommerceUnitOfWorkMock = _mock.Mock<IEcommerceUnitOfWork>();
            _stockUnit = _mock.Create<StockUnit>();
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
            Stock stock = new Stock
            {

            };

            _ecommerceUnitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.FromResult(true)).Verifiable();


            //Act
            await _stockUnit.CreateServiceAsync(stock);

            //Assert
            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );

        }
        [Test]
        public async Task UpdateServiceAsync()
        {
            //Arrange
            Stock stock = new Stock
            {

            };

            _ecommerceUnitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.FromResult(true)).Verifiable();


            //Act
            await _stockUnit.UpdateServiceAsync(stock);

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
            Stock stock = new Stock
            {

            };

            _ecommerceUnitOfWorkMock.Setup(x => x.Save()).Verifiable();


            //Act
            _stockUnit.CreateService(stock);

            //Assert
            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );

        }
        [Test]
        public void UpdateService()
        {
            //Arrange
            Stock stock = new Stock
            {

            };

            _ecommerceUnitOfWorkMock.Setup(x => x.Save()).Verifiable();


            //Act
            _stockUnit.UpdateService(stock);

            //Assert
            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );

        }

        #endregion

    }
}
