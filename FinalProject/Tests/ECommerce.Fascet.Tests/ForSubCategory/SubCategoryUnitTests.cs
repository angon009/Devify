using Autofac.Extras.Moq;
using ECommerce.Fascet.ForSubCategory;
using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.UnitOfWorks;
using Moq;
using NUnit.Framework;
using Shouldly;
using System.Threading.Tasks;

namespace ECommerce.Fascet.Tests.ForSubSubCategory
{
    public class SubSubCategoryUnitTests
    {
        private AutoMock _mock;
        private Mock<IEcommerceUnitOfWork> _ecommerceUnitOfWorkMock;
        private ISubCategoryUnit _subCategoryUnit;


        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose(); // Initializing the mock object 
        }
        [SetUp]
        public void Setup()
        {
            _ecommerceUnitOfWorkMock = _mock.Mock<IEcommerceUnitOfWork>();
            _subCategoryUnit = _mock.Create<SubCategoryUnit>();
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
            SubCategory subCategory = new SubCategory
            {
                SubCategoryName = "Mobiles",
                Description = "Unofficial Mobiles"
            };

            _ecommerceUnitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.FromResult(true)).Verifiable();


            //Act
            await _subCategoryUnit.CreateServiceAsync(subCategory);

            //Assert
            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );

        }
        [Test]
        public async Task UpdateServiceAsync()
        {
            //Arrange
            SubCategory subCategory = new SubCategory
            {
                SubCategoryName = "Mobiles",
                Description = "Unofficial Mobiles"
            };

            _ecommerceUnitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.FromResult(true)).Verifiable();


            //Act
            await _subCategoryUnit.UpdateServiceAsync(subCategory);

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
            await _subCategoryUnit.DeleteServiceAsync(id);

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
            SubCategory subCategory = new SubCategory
            {
                SubCategoryName = "Mobiles",
                Description = "Unofficial Mobiles"
            };

            _ecommerceUnitOfWorkMock.Setup(x => x.Save()).Verifiable();


            //Act
            _subCategoryUnit.CreateService(subCategory);

            //Assert
            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );

        }
        [Test]
        public void UpdateService()
        {
            //Arrange
            SubCategory subCategory = new SubCategory
            {
                SubCategoryName = "Mobiles",
                Description = "Unofficial Mobiles"
            };

            _ecommerceUnitOfWorkMock.Setup(x => x.Save()).Verifiable();


            //Act
            _subCategoryUnit.UpdateService(subCategory);

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
            _subCategoryUnit.DeleteService(id);

            //Assert
            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );

        }
        #endregion
    }
}
