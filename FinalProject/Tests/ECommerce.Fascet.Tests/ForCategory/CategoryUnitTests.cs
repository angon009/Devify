using Autofac.Extras.Moq;
using ECommerce.Fascet.ForCategory;
using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.UnitOfWorks;
using Moq;
using NUnit.Framework;
using Shouldly;
using System.Threading.Tasks;

namespace ECommerce.Fascet.Tests.ForCategory
{
    public class CategoryUnitsTests
    {
        private AutoMock _mock;
        private Mock<IEcommerceUnitOfWork> _ecommerceUnitOfWorkMock;
        private ICategoryUnit _categoryUnit;


        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose(); // Initializing the mock object 
        }
        [SetUp]
        public void Setup()
        {
            _ecommerceUnitOfWorkMock = _mock.Mock<IEcommerceUnitOfWork>();
            _categoryUnit = _mock.Create<CategoryUnit>();
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
            Category category = new Category
            {
                CategoryName = "Mobiles",
                Description = "Unofficial Mobiles"
            };

            _ecommerceUnitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.FromResult(true)).Verifiable();


            //Act
            await _categoryUnit.CreateServiceAsync(category);

            //Assert
            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );

        }
        [Test]
        public async Task UpdateServiceAsync()
        {
            //Arrange
            Category category = new Category
            {
                CategoryName = "Mobiles",
                Description = "Unofficial Mobiles"
            };

            _ecommerceUnitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.FromResult(true)).Verifiable();


            //Act
            await _categoryUnit.UpdateServiceAsync(category);

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
            await _categoryUnit.DeleteServiceAsync(id);

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
            Category category = new Category
            {
                CategoryName = "Mobiles",
                Description = "Unofficial Mobiles"
            };

            _ecommerceUnitOfWorkMock.Setup(x => x.Save()).Verifiable();


            //Act
            _categoryUnit.CreateService(category);

            //Assert
            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );

        }
        [Test]
        public void UpdateService()
        {
            //Arrange
            Category category = new Category
            {
                CategoryName = "Mobiles",
                Description = "Unofficial Mobiles"
            };

            _ecommerceUnitOfWorkMock.Setup(x => x.Save()).Verifiable();


            //Act
            _categoryUnit.UpdateService(category);

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
            _categoryUnit.DeleteService(id);

            //Assert
            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );

        }
        #endregion


    }
}