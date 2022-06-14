using Autofac.Extras.Moq;
using AutoMapper;
using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.Exceptions;
using ECommerce.Infrastructure.Repositories.ForSubCategory;
using ECommerce.Infrastructure.Services.ForSubCategory;
using ECommerce.Infrastructure.UnitOfWorks;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SubCategoryEntity = ECommerce.Core.Entities.Products.SubCategory;

namespace ECommerce.Infrastructure.Tests
{
    public class SubCategoryServicesTests
    {
        private AutoMock _mock;
        private Mock<IEcommerceUnitOfWork> _ecommerceUnitOfWorkMock;
        private Mock<ISubCategoryRepository> _subCategoryRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private ISubCategoryService _subCategoryService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose(); // Initializing the mock object 
        }

        [SetUp]
        public void Setup()
        {
            _ecommerceUnitOfWorkMock = _mock.Mock<IEcommerceUnitOfWork>(); //Mocking dependency
            _subCategoryRepositoryMock = _mock.Mock<ISubCategoryRepository>(); //Mocking dependency
            _mapperMock = _mock.Mock<IMapper>(); //Mocking dependency

            _subCategoryService = _mock.Create<SubCategoryService>();// Will test this service class's methods 

        }
        [TearDown]
        public void TearDown()
        {
            //Resetting mock after each test
            _ecommerceUnitOfWorkMock.Reset();
            _subCategoryRepositoryMock.Reset();
            _mapperMock.Reset();
        }
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose(); // Disposing the object 
        }

        #region Tests of Asynchronus Methods

        [Test]
        public async Task CreateSubCategoryAsync_SameSubCategoryNotExist_CreateSubCategory()
        {
            #region Arrange
            SubCategory subCategory = new SubCategory
            {
                SubCategoryName = "Mobiles",
                Description = "Unofficial Mobiles"
            };
            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(s => s.SubCategories)
               .Returns(_subCategoryRepositoryMock.Object);
            });

            _subCategoryRepositoryMock.Setup(s => s.GetCountAsync(
              It.IsAny<Expression<Func<SubCategoryEntity, bool>>>())).ReturnsAsync(0);

            await Task.Run(() =>
            {
                _mapperMock.Setup(s => s.Map<SubCategoryEntity>(subCategory))
                .Returns(new SubCategoryEntity()
                {
                    SubCategoryName = subCategory.SubCategoryName,
                    Description = subCategory.Description
                });
            });

            _subCategoryRepositoryMock.Setup(x => x.AddAsync(It.Is<SubCategoryEntity>
                (y => y.SubCategoryName == subCategory.SubCategoryName
                && y.Description == subCategory.Description))).Returns(Task.FromResult(true)).Verifiable();


            #endregion


            #region Act
            await _subCategoryService.CreateSubCategoryAsync(subCategory);
            #endregion


            #region Assert

            this.ShouldSatisfyAllConditions(

               () => _subCategoryRepositoryMock.VerifyAll(),
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );


            #endregion
        }

        [Test]
        public async Task CreateSubCategoryAsync_SameSubCategoryExist_CreateSubCategory()
        {
            //Arrange
            SubCategory subCategory = new SubCategory
            {
                SubCategoryName = "Mobiles",
                Description = "Official Mobile Sold Here"
            };

            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(x => x.SubCategories)
                    .Returns(_subCategoryRepositoryMock.Object);
            });

            _subCategoryRepositoryMock.Setup(x => x.GetCountAsync(
                It.IsAny<Expression<Func<SubCategoryEntity, bool>>>())).ReturnsAsync(1);

            //Act
            Should.Throw<DuplicateDataException>(
                () => _subCategoryService.CreateSubCategoryAsync(subCategory));

        }

        [Test]
        public async Task UpdateSubCategoryAsync_SameSubCategoryNotExist_UpdateSubCategory()
        {
            #region Arrange
            SubCategory subCategory = new SubCategory
            {
                Id = 1,
                SubCategoryName = "Mobiles",
                Description = "Unofficial Mobiles"
            };
            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(s => s.SubCategories)
                    .Returns(_subCategoryRepositoryMock.Object);
            });

            _subCategoryRepositoryMock.Setup(s => s.GetCountAsync(
              It.IsAny<Expression<Func<SubCategoryEntity, bool>>>())).ReturnsAsync(0);

            _subCategoryRepositoryMock.Setup(s => s.GetByIdAsync(subCategory.Id))
                .Returns(Task.FromResult(new SubCategoryEntity
                {
                    SubCategoryName = subCategory.SubCategoryName,
                    Description = subCategory.Description
                }));

            await Task.Run(() =>
            {
                _mapperMock.Setup(s => s.Map<SubCategoryEntity>(subCategory))
                .Returns(new SubCategoryEntity()
                {
                    SubCategoryName = subCategory.SubCategoryName,
                    Description = subCategory.Description
                });
            });


            #endregion


            #region Act
            await _subCategoryService.UpdateSubCategoryAsync(subCategory);
            #endregion


            #region Assert

            this.ShouldSatisfyAllConditions(

               () => _subCategoryRepositoryMock.VerifyAll(),
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );


            #endregion
        }

        [Test]
        public async Task UpdateSubCategoryAsync_SameSubCategoryExist_UpdateSubCategory()
        {
            //Arrange
            SubCategory subCategory = new SubCategory
            {
                SubCategoryName = "Mobiles",
                Description = "Official Mobile Sold Here"
            };

            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(x => x.SubCategories)
                    .Returns(_subCategoryRepositoryMock.Object);
            });

            _subCategoryRepositoryMock.Setup(x => x.GetCountAsync(
                It.IsAny<Expression<Func<SubCategoryEntity, bool>>>())).ReturnsAsync(1);

            //Act
            Should.Throw<DuplicateDataException>(
                () => _subCategoryService.UpdateSubCategoryAsync(subCategory));

        }

        [Test]
        public async Task DeleteSubCategoryAsync_DeleteCategory()
        {
            #region Arrange
            SubCategory subCategory = new SubCategory
            {
                Id = 1,
                SubCategoryName = "Mobiles",
                Description = "Unofficial Mobiles"
            };
            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(s => s.SubCategories)
                    .Returns(_subCategoryRepositoryMock.Object);
            });

            _subCategoryRepositoryMock.Setup(x => x.RemoveAsync(
                It.IsAny<Expression<Func<SubCategoryEntity, bool>>>())).Returns(Task.FromResult(true)).Verifiable();

            #endregion


            #region Act
            await _subCategoryService.DeleteSubCategoryAsync(subCategory.Id);
            #endregion


            #region Assert

            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );


            #endregion
        }

        [Test]
        public async Task GetSubCategoryAsync_ValidId_ReturnsSubCategory()
        {
            // Arrange
            var id = 3;

            SubCategoryEntity subCategoryEntity = new SubCategoryEntity
            {
                Id = 3
            };
            SubCategory subCategory = new SubCategory() { Id = subCategoryEntity.Id };

            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(x => x.SubCategories)
                .Returns(_subCategoryRepositoryMock.Object);
            });

            _ecommerceUnitOfWorkMock.Setup(x => x.SubCategories)
                .Returns(_subCategoryRepositoryMock.Object);

            _subCategoryRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(subCategoryEntity);

            await Task.Run(() =>
            {
                _mapperMock.Setup(x => x.Map<SubCategory>(subCategoryEntity))
                .Returns(subCategory);
            });


            // Act
            var result = await _subCategoryService.GetSubCategoryAsync(id);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Id.ShouldBe(id)
            );
        }

        [Test]
        public async Task GetSubcategoriesAsync_ReturnsSubCategories()
        {
            List<SubCategoryEntity> subCategoriesEntity = new List<SubCategoryEntity>();

            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(x => x.SubCategories)
                    .Returns(_subCategoryRepositoryMock.Object);
            });

            _subCategoryRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(subCategoriesEntity);

            // Act
            //var result = await _subCategoryService.GetSubCategoriesAsync();

            //// Assert
            //this.ShouldSatisfyAllConditions(
            //    () => result.ShouldNotBeNull()
            //);

        }

        [Test]
        public async Task GetSubCategoriesAsync_ReturnSubCategoriesTotalTotalDisplay()
        {
            int pageIndex = 1;
            int pageSize = 1;
            string orderBy = string.Empty;

            List<SubCategoryEntity> subCategoryEntities = new List<SubCategoryEntity>();

            int Total = 5;
            int TotalDisplay = 10;

            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(x => x.SubCategories)
                   .Returns(_subCategoryRepositoryMock.Object);
            });

            _subCategoryRepositoryMock.Setup(s => s.GetDynamicAsync(
              It.IsAny<Expression<Func<SubCategoryEntity, bool>>>(), orderBy,
              string.Empty, pageIndex, pageSize, true)).ReturnsAsync((subCategoryEntities, Total, TotalDisplay)); //How to return multiple values??


            // Act
            //var result = await _subCategoryService.GetSubCategoriesAsync(pageIndex, pageSize, string.Empty, string.Empty);

            // Assert
            //this.ShouldSatisfyAllConditions(
            //    () => result.total.ShouldBe(Total),
            //    () => result.totalDisplay.ShouldBe(TotalDisplay)

            //);
        }
        #endregion



        #region Tests of Non-AsynchronusMethods
        [Test]
        public void CreateSubCategory_SameSubCategoryNotExist_CreateSubCategory()
        {
            #region Arrange
            SubCategory subCategory = new SubCategory
            {
                SubCategoryName = "Mobiles",
                Description = "Unofficial Mobiles"
            };
            _ecommerceUnitOfWorkMock.Setup(s => s.SubCategories)
           .Returns(_subCategoryRepositoryMock.Object);

            _subCategoryRepositoryMock.Setup(s => s.GetCount(
              It.IsAny<Expression<Func<SubCategoryEntity, bool>>>())).Returns(0);


            _mapperMock.Setup(s => s.Map<SubCategoryEntity>(subCategory))
            .Returns(new SubCategoryEntity()
            {
                SubCategoryName = subCategory.SubCategoryName,
                Description = subCategory.Description
            });

            _subCategoryRepositoryMock.Setup(x => x.Add(It.Is<SubCategoryEntity>
                (y => y.SubCategoryName == subCategory.SubCategoryName
                && y.Description == subCategory.Description))).Verifiable();


            #endregion


            #region Act
            _subCategoryService.CreateSubCategory(subCategory);
            #endregion


            #region Assert

            this.ShouldSatisfyAllConditions(

               () => _subCategoryRepositoryMock.VerifyAll()
               //() => _ecommerceUnitOfWorkMock.VerifyAll()
           );


            #endregion
        }

        [Test]
        public void CreateSubCategory_SameSubCategoryExist_CreateSubCategory()
        {
            //Arrange
            SubCategory subCategory = new SubCategory
            {
                SubCategoryName = "Mobiles",
                Description = "Official Mobile Sold Here"
            };
            _ecommerceUnitOfWorkMock.Setup(x => x.SubCategories)
                .Returns(_subCategoryRepositoryMock.Object);

            _subCategoryRepositoryMock.Setup(x => x.GetCount(
                It.IsAny<Expression<Func<SubCategoryEntity, bool>>>())).Returns(1);

            //Act
            Should.Throw<DuplicateDataException>(
                () => _subCategoryService.CreateSubCategory(subCategory));

        }

        [Test]
        public void UpdateSubCategory_SameSubCategoryNotExist_UpdateSubCategory()
        {
            #region Arrange
            SubCategory subCategory = new SubCategory
            {
                Id = 1,
                SubCategoryName = "Mobiles",
                Description = "Unofficial Mobiles"
            };
            _ecommerceUnitOfWorkMock.Setup(s => s.SubCategories)
                .Returns(_subCategoryRepositoryMock.Object);

            _subCategoryRepositoryMock.Setup(s => s.GetCount(
              It.IsAny<Expression<Func<SubCategoryEntity, bool>>>())).Returns(0);

            _subCategoryRepositoryMock.Setup(s => s.GetById(subCategory.Id))
                .Returns(new SubCategoryEntity
                {
                    SubCategoryName = subCategory.SubCategoryName,
                    Description = subCategory.Description
                });

            _mapperMock.Setup(s => s.Map<SubCategoryEntity>(subCategory))
            .Returns(new SubCategoryEntity()
            {
                SubCategoryName = subCategory.SubCategoryName,
                Description = subCategory.Description
            });


            #endregion


            #region Act
            _subCategoryService.UpdateSubCategory(subCategory);
            #endregion


            #region Assert

            this.ShouldSatisfyAllConditions(

               () => _subCategoryRepositoryMock.VerifyAll(),
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );


            #endregion
        }

        [Test]
        public void UpdateSubCategory_SameSubCategoryExist_UpdateSubCategory()
        {
            //Arrange
            SubCategory subCategory = new SubCategory
            {
                SubCategoryName = "Mobiles",
                Description = "Official Mobile Sold Here"
            };


            _ecommerceUnitOfWorkMock.Setup(x => x.SubCategories)
                .Returns(_subCategoryRepositoryMock.Object);

            _subCategoryRepositoryMock.Setup(x => x.GetCount(
                It.IsAny<Expression<Func<SubCategoryEntity, bool>>>())).Returns(1);

            //Act
            Should.Throw<DuplicateDataException>(
                () => _subCategoryService.UpdateSubCategory(subCategory));

        }

        [Test]
        public void DeleteSubCategory_DeleteCategory()
        {
            #region Arrange
            SubCategory subCategory = new SubCategory
            {
                Id = 1,
                SubCategoryName = "Mobiles",
                Description = "Unofficial Mobiles"
            };
            _ecommerceUnitOfWorkMock.Setup(s => s.SubCategories)
                .Returns(_subCategoryRepositoryMock.Object);

            _subCategoryRepositoryMock.Setup(x => x.Remove(
                It.IsAny<Expression<Func<SubCategoryEntity, bool>>>())).Verifiable();


            #endregion


            #region Act
            _subCategoryService.DeleteSubCategory(subCategory.Id);
            #endregion


            #region Assert

            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );


            #endregion
        }

        [Test]
        public void GetSubCategory_ValidId_ReturnsSubCategory()
        {
            // Arrange
            var id = 3;

            SubCategoryEntity subCategoryEntity = new SubCategoryEntity
            {
                Id = 3
            };
            SubCategory subCategory = new SubCategory() { Id = subCategoryEntity.Id };

            _ecommerceUnitOfWorkMock.Setup(x => x.SubCategories)
                .Returns(_subCategoryRepositoryMock.Object);

            _subCategoryRepositoryMock.Setup(x => x.GetById(id)).Returns(subCategoryEntity);

            _mapperMock.Setup(x => x.Map<SubCategory>(subCategoryEntity))
                .Returns(subCategory);

            // Act
            var result = _subCategoryService.GetSubCategory(id);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Id.ShouldBe(id)
            );
        }

        [Test]
        public void GetSubCategories_ReturnsSubCategories()
        {
            List<SubCategoryEntity> subCategoriesEntities = new List<SubCategoryEntity>();

            _ecommerceUnitOfWorkMock.Setup(x => x.SubCategories)
                .Returns(_subCategoryRepositoryMock.Object);

            _subCategoryRepositoryMock.Setup(x => x.GetAll()).Returns(subCategoriesEntities);

            // Act
            //var result = _subCategoryService.GetSubCategories();

            //// Assert
            //this.ShouldSatisfyAllConditions(
            //    () => result.ShouldNotBeNull()
            //);

        }

        [Test]
        public void GetSubCategories_ReturnSubCategoriesTotalTotalDisplay()
        {
            int pageIndex = 1;
            int pageSize = 1;
            string orderBy = string.Empty;

            List<SubCategoryEntity> subCategoryEntities = new List<SubCategoryEntity>();

            int Total = 5;
            int TotalDisplay = 10;

            _ecommerceUnitOfWorkMock.Setup(x => x.SubCategories)
               .Returns(_subCategoryRepositoryMock.Object);

            _subCategoryRepositoryMock.Setup(s => s.GetDynamic(
              It.IsAny<Expression<Func<SubCategoryEntity, bool>>>(), orderBy,
              string.Empty, pageIndex, pageSize, true)).Returns((subCategoryEntities, Total, TotalDisplay)); //How to return multiple values??


            // Act
            var result = _subCategoryService.GetSubCategories(pageIndex, pageSize, string.Empty, string.Empty);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.total.ShouldBe(Total),
                () => result.totalDisplay.ShouldBe(TotalDisplay)

            );
        }

        #endregion
    }
}