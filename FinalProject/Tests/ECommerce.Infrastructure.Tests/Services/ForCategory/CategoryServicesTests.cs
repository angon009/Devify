using Autofac.Extras.Moq;
using AutoMapper;
using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.Exceptions;
using ECommerce.Infrastructure.Repositories.ForCategory;
using ECommerce.Infrastructure.Services.ForCategory;
using ECommerce.Infrastructure.UnitOfWorks;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CategoryEntity = ECommerce.Core.Entities.Products.Category;

namespace ECommerce.Infrastructure.Tests
{
    public class CategoryServicesTests
    {
        private AutoMock _mock;
        private Mock<IEcommerceUnitOfWork> _ecommerceUnitOfWorkMock;
        private Mock<ICategoryRepository> _categoryRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private ICategoryService _categoryService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose(); // Initializing the mock object 
        }

        [SetUp]
        public void Setup()
        {
            _ecommerceUnitOfWorkMock = _mock.Mock<IEcommerceUnitOfWork>(); //Mocking dependency
            _categoryRepositoryMock = _mock.Mock<ICategoryRepository>(); //Mocking dependency
            _mapperMock = _mock.Mock<IMapper>(); //Mocking dependency

            _categoryService = _mock.Create<CategoryService>();// Will test this service class's methods 

        }
        [TearDown]
        public void TearDown()
        {
            //Resetting mock after each test
            _ecommerceUnitOfWorkMock.Reset();
            _categoryRepositoryMock.Reset();
            _mapperMock.Reset();
        }
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose(); // Disposing the object 
        }

        #region Tests of Asynchronus Methods

        [Test]
        public async Task CreateCategoryAsync_SameCategoryNotExist_CreateCategory()
        {
            #region Arrange
            Category category = new Category
            {
                CategoryName = "Mobiles",
                Description = "Unofficial Mobiles"
            };
            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(s => s.Categories)
               .Returns(_categoryRepositoryMock.Object);
            });

            _categoryRepositoryMock.Setup(s => s.GetCountAsync(
              It.IsAny<Expression<Func<CategoryEntity, bool>>>())).ReturnsAsync(0);

            await Task.Run(() =>
            {
                _mapperMock.Setup(s => s.Map<CategoryEntity>(category))
                .Returns(new CategoryEntity()
                {
                    CategoryName = category.CategoryName,
                    Description = category.Description
                });
            });

            _categoryRepositoryMock.Setup(x => x.AddAsync(It.Is<CategoryEntity>
                (y => y.CategoryName == category.CategoryName
                && y.Description == category.Description))).Returns(Task.FromResult(true)).Verifiable();

            #endregion


            #region Act
            await _categoryService.CreateCategoryAsync(category);
            #endregion


            #region Assert

            this.ShouldSatisfyAllConditions(

               () => _categoryRepositoryMock.VerifyAll(),
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );


            #endregion
        }

        [Test]
        public async Task CreateCategoryAsync_SameCategoryExist_CreateCategory()
        {
            //Arrange
            Category category = new Category
            {
                CategoryName = "Mobiles",
                Description = "Official Mobile Sold Here"
            };

            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(x => x.Categories)
                    .Returns(_categoryRepositoryMock.Object);
            });

            _categoryRepositoryMock.Setup(x => x.GetCountAsync(
                It.IsAny<Expression<Func<CategoryEntity, bool>>>())).ReturnsAsync(1);

            //Act
            Should.Throw<DuplicateDataException>(
                () => _categoryService.CreateCategoryAsync(category));

        }

        [Test]
        public async Task UpdateCategoryAsync_SameCategoryNotExist_UpdateCategory()
        {
            #region Arrange
            Category category = new Category
            {
                Id = 1,
                CategoryName = "Mobiles",
                Description = "Unofficial Mobiles"
            };
            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(s => s.Categories)
                    .Returns(_categoryRepositoryMock.Object);
            });

            _categoryRepositoryMock.Setup(s => s.GetCountAsync(
              It.IsAny<Expression<Func<CategoryEntity, bool>>>())).ReturnsAsync(0);

            _categoryRepositoryMock.Setup(s => s.GetByIdAsync(category.Id))
                .Returns(Task.FromResult(new CategoryEntity
                {
                    CategoryName = category.CategoryName,
                    Description = category.Description
                }));

            await Task.Run(() =>
            {
                _mapperMock.Setup(s => s.Map<CategoryEntity>(category))
                .Returns(new CategoryEntity()
                {
                    CategoryName = category.CategoryName,
                    Description = category.Description
                });
            });


            #endregion


            #region Act
            await _categoryService.UpdateCategoryAsync(category);
            #endregion


            #region Assert

            this.ShouldSatisfyAllConditions(

               () => _categoryRepositoryMock.VerifyAll(),
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );


            #endregion
        }

        [Test]
        public async Task UpdateCategoryAsync_SameCategoryExist_UpdateCategory()
        {
            //Arrange
            Category category = new Category
            {
                CategoryName = "Mobiles",
                Description = "Official Mobile Sold Here"
            };

            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(x => x.Categories)
                    .Returns(_categoryRepositoryMock.Object);
            });

            _categoryRepositoryMock.Setup(x => x.GetCountAsync(
                It.IsAny<Expression<Func<CategoryEntity, bool>>>())).ReturnsAsync(1);

            //Act
            Should.Throw<DuplicateDataException>(
                () => _categoryService.UpdateCategoryAsync(category));

        }

        [Test]
        public async Task DeleteCategoryAsync_DeleteCategory()
        {
            #region Arrange
            Category category = new Category
            {
                Id = 1,
                CategoryName = "Mobiles",
                Description = "Unofficial Mobiles"
            };
            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(s => s.Categories)
                    .Returns(_categoryRepositoryMock.Object);
            });

            _categoryRepositoryMock.Setup(x => x.RemoveAsync(
                It.IsAny<Expression<Func<CategoryEntity, bool>>>())).Returns(Task.FromResult(true)).Verifiable();

            #endregion


            #region Act
            await _categoryService.DeleteCategoryAsync(category.Id);
            #endregion


            #region Assert

            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );


            #endregion
        }

        [Test]
        public async Task GetCategoryAsync_ValidId_ReturnsCategory()
        {
            // Arrange
            var id = 3;

            CategoryEntity categoryEntity = new CategoryEntity
            {
                Id = 3
            };
            Category category = new Category() { Id = categoryEntity.Id };

            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(x => x.Categories)
                .Returns(_categoryRepositoryMock.Object);
            });

            _ecommerceUnitOfWorkMock.Setup(x => x.Categories)
                .Returns(_categoryRepositoryMock.Object);

            _categoryRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(categoryEntity);

            await Task.Run(() =>
            {
                _mapperMock.Setup(x => x.Map<Category>(categoryEntity))
                .Returns(category);
            });


            // Act
            var result = await _categoryService.GetCategoryAsync(id);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Id.ShouldBe(id)
            );
        }

        [Test]
        public async Task GetSubcategoriesAsync_ReturnsCategories()
        {
            List<CategoryEntity> categoriesEntity = new List<CategoryEntity>();

            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(x => x.Categories)
                    .Returns(_categoryRepositoryMock.Object);
            });

            _categoryRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(categoriesEntity);

            // Act
            //var result = await _categoryService.GetCategoriesAsync();

            //// Assert
            //this.ShouldSatisfyAllConditions(
            //    () => result.ShouldNotBeNull()
            //);

        }

        [Test]
        public async Task GetCategoriesAsync_ReturnCategoriesTotalTotalDisplay()
        {
            int pageIndex = 1;
            int pageSize = 1;
            string orderBy = string.Empty;

            List<CategoryEntity> categoryEntities = new List<CategoryEntity>();

            int Total = 5;
            int TotalDisplay = 10;

            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(x => x.Categories)
                   .Returns(_categoryRepositoryMock.Object);
            });

            _categoryRepositoryMock.Setup(s => s.GetDynamicAsync(
              It.IsAny<Expression<Func<CategoryEntity, bool>>>(), orderBy,
              string.Empty, pageIndex, pageSize, true)).ReturnsAsync((categoryEntities, Total, TotalDisplay)); //How to return multiple values??


            // Act
            //var result = await _categoryService.GetCategoriesAsync(pageIndex, pageSize, string.Empty, string.Empty);

            // Assert
            //this.ShouldSatisfyAllConditions(
            //    () => result.total.ShouldBe(Total),
            //    () => result.totalDisplay.ShouldBe(TotalDisplay)

            //);
        }
        #endregion



        #region Tests of Non-AsynchronusMethods
        [Test]
        public void CreateCategory_SameCategoryNotExist_CreateCategory()
        {
            #region Arrange
            Category category = new Category
            {
                CategoryName = "Mobiles",
                Description = "Unofficial Mobiles"
            };
            _ecommerceUnitOfWorkMock.Setup(s => s.Categories)
           .Returns(_categoryRepositoryMock.Object);

            _categoryRepositoryMock.Setup(s => s.GetCount(
              It.IsAny<Expression<Func<CategoryEntity, bool>>>())).Returns(0);


            _mapperMock.Setup(s => s.Map<CategoryEntity>(category))
            .Returns(new CategoryEntity()
            {
                CategoryName = category.CategoryName,
                Description = category.Description
            });

            _categoryRepositoryMock.Setup(x => x.Add(It.Is<CategoryEntity>
                (y => y.CategoryName == category.CategoryName
                && y.Description == category.Description))).Verifiable();


            #endregion


            #region Act
            _categoryService.CreateCategory(category);
            #endregion


            #region Assert

            this.ShouldSatisfyAllConditions(

               () => _categoryRepositoryMock.VerifyAll(),
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );


            #endregion
        }

        [Test]
        public void CreateCategory_SameCategoryExist_CreateCategory()
        {
            //Arrange
            Category category = new Category
            {
                CategoryName = "Mobiles",
                Description = "Official Mobile Sold Here"
            };
            _ecommerceUnitOfWorkMock.Setup(x => x.Categories)
                .Returns(_categoryRepositoryMock.Object);

            _categoryRepositoryMock.Setup(x => x.GetCount(
                It.IsAny<Expression<Func<CategoryEntity, bool>>>())).Returns(1);

            //Act
            Should.Throw<DuplicateDataException>(
                () => _categoryService.CreateCategory(category));

        }

        [Test]
        public void UpdateCategory_SameCategoryNotExist_UpdateCategory()
        {
            #region Arrange
            Category category = new Category
            {
                Id = 1,
                CategoryName = "Mobiles",
                Description = "Unofficial Mobiles"
            };
            _ecommerceUnitOfWorkMock.Setup(s => s.Categories)
                .Returns(_categoryRepositoryMock.Object);

            _categoryRepositoryMock.Setup(s => s.GetCount(
              It.IsAny<Expression<Func<CategoryEntity, bool>>>())).Returns(0);

            _categoryRepositoryMock.Setup(s => s.GetById(category.Id))
                .Returns(new CategoryEntity
                {
                    CategoryName = category.CategoryName,
                    Description = category.Description
                });

            _mapperMock.Setup(s => s.Map<CategoryEntity>(category))
            .Returns(new CategoryEntity()
            {
                CategoryName = category.CategoryName,
                Description = category.Description
            });


            #endregion


            #region Act
            _categoryService.UpdateCategory(category);
            #endregion


            #region Assert

            this.ShouldSatisfyAllConditions(

               () => _categoryRepositoryMock.VerifyAll(),
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );


            #endregion
        }

        [Test]
        public void UpdateCategory_SameCategoryExist_UpdateCategory()
        {
            //Arrange
            Category category = new Category
            {
                CategoryName = "Mobiles",
                Description = "Official Mobile Sold Here"
            };


            _ecommerceUnitOfWorkMock.Setup(x => x.Categories)
                .Returns(_categoryRepositoryMock.Object);

            _categoryRepositoryMock.Setup(x => x.GetCount(
                It.IsAny<Expression<Func<CategoryEntity, bool>>>())).Returns(1);

            //Act
            Should.Throw<DuplicateDataException>(
                () => _categoryService.UpdateCategory(category));

        }

        [Test]
        public void DeleteCategory_DeleteCategory()
        {
            #region Arrange
            Category category = new Category
            {
                Id = 1,
                CategoryName = "Mobiles",
                Description = "Unofficial Mobiles"
            };
            _ecommerceUnitOfWorkMock.Setup(s => s.Categories)
                .Returns(_categoryRepositoryMock.Object);

            _categoryRepositoryMock.Setup(x => x.Remove(
                It.IsAny<Expression<Func<CategoryEntity, bool>>>())).Verifiable();


            #endregion


            #region Act
            _categoryService.DeleteCategory(category.Id);
            #endregion


            #region Assert

            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );


            #endregion
        }

        [Test]
        public void GetCategory_ValidId_ReturnsCategory()
        {
            // Arrange
            var id = 3;

            CategoryEntity categoryEntity = new CategoryEntity
            {
                Id = 3
            };
            Category category = new Category() { Id = categoryEntity.Id };

            _ecommerceUnitOfWorkMock.Setup(x => x.Categories)
                .Returns(_categoryRepositoryMock.Object);

            _categoryRepositoryMock.Setup(x => x.GetById(id)).Returns(categoryEntity);

            _mapperMock.Setup(x => x.Map<Category>(categoryEntity))
                .Returns(category);

            // Act
            var result = _categoryService.GetCategory(id);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Id.ShouldBe(id)
            );
        }

        [Test]
        public void GetCategories_ReturnsCategories()
        {
            List<CategoryEntity> categoriesEntities = new List<CategoryEntity>();

            _ecommerceUnitOfWorkMock.Setup(x => x.Categories)
                .Returns(_categoryRepositoryMock.Object);

            _categoryRepositoryMock.Setup(x => x.GetAll()).Returns(categoriesEntities);

            // Act
            var result = _categoryService.GetCategories();

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull()
            );

        }

        [Test]
        public void GetCategories_ReturnCategoriesTotalTotalDisplay()
        {
            int pageIndex = 1;
            int pageSize = 1;
            string orderBy = string.Empty;

            List<CategoryEntity> categoryEntities = new List<CategoryEntity>();

            int Total = 5;
            int TotalDisplay = 10;

            _ecommerceUnitOfWorkMock.Setup(x => x.Categories)
               .Returns(_categoryRepositoryMock.Object);

            _categoryRepositoryMock.Setup(s => s.GetDynamic(
              It.IsAny<Expression<Func<CategoryEntity, bool>>>(), orderBy,
              string.Empty, pageIndex, pageSize, true)).Returns((categoryEntities, Total, TotalDisplay)); //How to return multiple values??


            // Act
            var result = _categoryService.GetCategories(pageIndex, pageSize, string.Empty, string.Empty);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.total.ShouldBe(Total),
                () => result.totalDisplay.ShouldBe(TotalDisplay)

            );
        }

        #endregion
    }
}
