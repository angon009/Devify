using Autofac.Extras.Moq;
using ECommerce.Fascet.ForStore;
using ECommerce.Infrastructure.BusinessObjects.Stores;
using ECommerce.Infrastructure.UnitOfWorks;
using Moq;
using NUnit.Framework;
using Shouldly;
using System.Threading.Tasks;

namespace ECommerce.Fascet.Tests.ForStore
{
    public class StoreUnitTests
    {
        private AutoMock _mock;
        private Mock<IEcommerceUnitOfWork> _ecommerceUnitOfWorkMock;
        private IStoreUnit _storeUnit;


        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose(); // Initializing the mock object 
        }
        [SetUp]
        public void Setup()
        {
            _ecommerceUnitOfWorkMock = _mock.Mock<IEcommerceUnitOfWork>();
            _storeUnit = _mock.Create<StoreUnit>();
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
            Store store = new Store
            {
                StoreName = "Sabbirer Mudikhanar Dokan"
            };

            _ecommerceUnitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.FromResult(true)).Verifiable();


            //Act
            await _storeUnit.CreateServiceAsync(store);

            //Assert
            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );

        }
        [Test]
        public async Task UpdateServiceAsync()
        {
            //Arrange
            Store store = new Store
            {
                StoreName = "Sabbirer Mudikhanar Dokan"
            };

            _ecommerceUnitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.FromResult(true)).Verifiable();


            //Act
            await _storeUnit.UpdateServiceAsync(store);

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
            await _storeUnit.DeleteServiceAsync(id);

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
            Store store = new Store
            {
                StoreName = "Sabbirer Mudikhanar Dokan"
            };

            _ecommerceUnitOfWorkMock.Setup(x => x.Save()).Verifiable();


            //Act
            _storeUnit.CreateService(store);

            //Assert
            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );

        }
        [Test]
        public void UpdateService()
        {
            //Arrange
            Store store = new Store
            {
                StoreName = "Sabbirer Mudikhanar Dokan"
            };

            _ecommerceUnitOfWorkMock.Setup(x => x.Save()).Verifiable();


            //Act
            _storeUnit.UpdateService(store);

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
            _storeUnit.DeleteService(id);

            //Assert
            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );

        }
        #endregion
    }
}
