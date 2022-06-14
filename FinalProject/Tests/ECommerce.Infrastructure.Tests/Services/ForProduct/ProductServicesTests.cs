using Autofac.Extras.Moq;
using AutoMapper;
using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.Exceptions;
using ECommerce.Infrastructure.Repositories.ForProduct;
using ECommerce.Infrastructure.Services.ForProduct;
using ECommerce.Infrastructure.UnitOfWorks;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ProductEntity = ECommerce.Core.Entities.Products.Product;

namespace ECommerce.Infrastructure.Tests
{
    public class ProductServicesTests
    {
        private AutoMock _mock;
        private Mock<IEcommerceUnitOfWork> _ecommerceUnitOfWorkMock;
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private IProductService _productService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose(); // Initializing the mock object 
        }

        [SetUp]
        public void Setup()
        {
            _ecommerceUnitOfWorkMock = _mock.Mock<IEcommerceUnitOfWork>(); //Mocking dependency
            _productRepositoryMock = _mock.Mock<IProductRepository>(); //Mocking dependency
            _mapperMock = _mock.Mock<IMapper>(); //Mocking dependency

            _productService = _mock.Create<ProductService>();// Will test this service class's methods 

        }
        [TearDown]
        public void TearDown()
        {
            //Resetting mock after each test
            _ecommerceUnitOfWorkMock.Reset();
            _productRepositoryMock.Reset();
            _mapperMock.Reset();
        }
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose(); // Disposing the object 
        }

        #region Tests of Asynchronus Methods

        [Test]
        public async Task CreateProductAsync_SameProductNotExist_CreateProduct()
        {
            #region Arrange
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

            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(s => s.Products)
               .Returns(_productRepositoryMock.Object);
            });

            _productRepositoryMock.Setup(s => s.GetCountAsync(
              It.IsAny<Expression<Func<ProductEntity, bool>>>())).ReturnsAsync(0);

            await Task.Run(() =>
            {
                _mapperMock.Setup(s => s.Map<ProductEntity>(product))
                .Returns(new ProductEntity()
                {
                    Name = product.Name,


                    SalePrice = product.SalePrice,
                    CostPrice = product.CostPrice,

                    Model = product.Model,
                    Brand = product.Brand,
                    ExpireDate = product.ExpireDate,
                    ManufactureDate = product.ManufactureDate,
                    Weight = product.Weight,
                    Size = product.Size,
                    ProductDetails = product.ProductDetails
                });
            });

            _productRepositoryMock.Setup(x => x.AddAsync(It.Is<ProductEntity>
               (y => y.Name == product.Name
               && y.SalePrice == product.SalePrice
               && y.CostPrice == product.CostPrice
               && y.Model == product.Model
               && y.Brand == product.Brand
               && y.ExpireDate == product.ExpireDate
               && y.ManufactureDate == product.ManufactureDate
               && y.Weight == product.Weight
               && y.Size == product.Size
               && y.ProductDetails == product.ProductDetails
               ))).Returns(Task.FromResult(true)).Verifiable();


            #endregion


            #region Act
            await _productService.CreateProductAsync(product);
            #endregion


            #region Assert

            this.ShouldSatisfyAllConditions(

               () => _productRepositoryMock.VerifyAll(),
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );


            #endregion
        }

        [Test]
        public async Task CreateProductAsync_SameProductExist_CreateProduct()
        {
            //Arrange
            Product product = new Product
            {
                Name = "Mobiles",
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

            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(x => x.Products)
                    .Returns(_productRepositoryMock.Object);
            });

            _productRepositoryMock.Setup(x => x.GetCountAsync(
                It.IsAny<Expression<Func<ProductEntity, bool>>>())).ReturnsAsync(1);

            //Act
            Should.Throw<DuplicateDataException>(
                () => _productService.CreateProductAsync(product));

        }

        [Test]
        public async Task UpdateProductAsync_SameProductNotExist_UpdateProduct()
        {
            #region Arrange
            Product product = new Product
            {
                Id = 1,
                Name = "Mobiles",
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
            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(s => s.Products)
                    .Returns(_productRepositoryMock.Object);
            });

            _productRepositoryMock.Setup(s => s.GetCountAsync(
              It.IsAny<Expression<Func<ProductEntity, bool>>>())).ReturnsAsync(0);

            _productRepositoryMock.Setup(s => s.GetByIdAsync(product.Id))
                .Returns(Task.FromResult(new ProductEntity
                {
                    Name = product.Name,
                    SalePrice = product.SalePrice,
                    CostPrice = product.CostPrice,

                    Model = product.Model,
                    Brand = product.Brand,
                    ExpireDate = product.ExpireDate,
                    ManufactureDate = product.ManufactureDate,
                    Weight = product.Weight,
                    Size = product.Size,
                    ProductDetails = product.ProductDetails
                }));

            await Task.Run(() =>
            {
                _mapperMock.Setup(s => s.Map<ProductEntity>(product))
                .Returns(new ProductEntity()
                {
                    Name = product.Name,
                    SalePrice = product.SalePrice,
                    CostPrice = product.CostPrice,

                    Model = product.Model,
                    Brand = product.Brand,
                    ExpireDate = product.ExpireDate,
                    ManufactureDate = product.ManufactureDate,
                    Weight = product.Weight,
                    Size = product.Size,
                    ProductDetails = product.ProductDetails
                });
            });


            #endregion


            #region Act
            await _productService.UpdateProductAsync(product);
            #endregion


            #region Assert

            this.ShouldSatisfyAllConditions(

               () => _productRepositoryMock.VerifyAll(),
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );


            #endregion
        }

        [Test]
        public async Task UpdateProductAsync_SameProductExist_UpdateProduct()
        {
            //Arrange
            Product product = new Product
            {
                Name = "Mobiles",

            };

            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(x => x.Products)
                    .Returns(_productRepositoryMock.Object);
            });

            _productRepositoryMock.Setup(x => x.GetCountAsync(
                It.IsAny<Expression<Func<ProductEntity, bool>>>())).ReturnsAsync(1);

            //Act
            Should.Throw<DuplicateDataException>(
                () => _productService.UpdateProductAsync(product));

        }

        [Test]
        public async Task DeleteProductAsync_DeleteCategory()
        {
            #region Arrange
            Product product = new Product
            {
                Id = 1,
                Name = "Mobiles",
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
            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(s => s.Products)
                    .Returns(_productRepositoryMock.Object);
            });

            _productRepositoryMock.Setup(x => x.RemoveAsync(
                It.IsAny<Expression<Func<ProductEntity, bool>>>())).Returns(Task.FromResult(true)).Verifiable();

            #endregion


            #region Act
            await _productService.DeleteProductAsync(product.Id);
            #endregion


            #region Assert

            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );


            #endregion
        }

        [Test]
        public async Task GetProductAsync_ValidId_ReturnsProduct()
        {
            // Arrange
            var id = 3;

            ProductEntity productEntity = new ProductEntity
            {
                Id = 3
            };
            Product product = new Product() { Id = productEntity.Id };

            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(x => x.Products)
                .Returns(_productRepositoryMock.Object);
            });

            _ecommerceUnitOfWorkMock.Setup(x => x.Products)
                .Returns(_productRepositoryMock.Object);

            _productRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(productEntity);

            await Task.Run(() =>
            {
                _mapperMock.Setup(x => x.Map<Product>(productEntity))
                .Returns(product);
            });


            // Act
            var result = await _productService.GetProductAsync(id);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Id.ShouldBe(id)
            );
        }

        [Test]
        public async Task GetSubcategoriesAsync_ReturnsProducts()
        {
            List<ProductEntity> subCategoriesEntity = new List<ProductEntity>();

            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(x => x.Products)
                    .Returns(_productRepositoryMock.Object);
            });

            _productRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(subCategoriesEntity);

            // Act
            var result = await _productService.GetProductsAsync();

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull()
            );

        }

        [Test]
        public async Task GetProductsAsync_ReturnProductsTotalTotalDisplay()
        {
            int pageIndex = 1;
            int pageSize = 1;
            string orderBy = string.Empty;

            List<ProductEntity> productEntities = new List<ProductEntity>();

            int Total = 5;
            int TotalDisplay = 10;

            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(x => x.Products)
                   .Returns(_productRepositoryMock.Object);
            });

            _productRepositoryMock.Setup(s => s.GetDynamicAsync(
              It.IsAny<Expression<Func<ProductEntity, bool>>>(), orderBy,
              string.Empty, pageIndex, pageSize, true)).ReturnsAsync((productEntities, Total, TotalDisplay)); //How to return multiple values??


            // Act
            var result = await _productService.GetProductsAsync(pageIndex, pageSize, string.Empty, string.Empty);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.total.ShouldBe(Total),
                () => result.totalDisplay.ShouldBe(TotalDisplay)

            );
        }
        #endregion



        #region Tests of Non-AsynchronusMethods
        [Test]
        public void CreateProduct_SameProductNotExist_CreateProduct()
        {
            #region Arrange
            Product product = new Product
            {
                Name = "Mobiles",
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
            _ecommerceUnitOfWorkMock.Setup(s => s.Products)
           .Returns(_productRepositoryMock.Object);

            _productRepositoryMock.Setup(s => s.GetCount(
              It.IsAny<Expression<Func<ProductEntity, bool>>>())).Returns(0);


            _mapperMock.Setup(s => s.Map<ProductEntity>(product))
            .Returns(new ProductEntity()
            {
                Name = product.Name,
                SalePrice = product.SalePrice,
                CostPrice = product.CostPrice,

                Model = product.Model,
                Brand = product.Brand,
                ExpireDate = product.ExpireDate,
                ManufactureDate = product.ManufactureDate,
                Weight = product.Weight,
                Size = product.Size,
                ProductDetails = product.ProductDetails
            });

            _productRepositoryMock.Setup(x => x.Add(It.Is<ProductEntity>
                (y => y.Name == product.Name
                && y.SalePrice == product.SalePrice
                && y.CostPrice == product.CostPrice
                && y.Model == product.Model
                && y.Brand == product.Brand
                && y.ExpireDate == product.ExpireDate
                && y.ManufactureDate == product.ManufactureDate
                && y.Weight == product.Weight
                && y.Size == product.Size
                && y.ProductDetails == product.ProductDetails
                ))).Verifiable();


            #endregion


            #region Act
            _productService.CreateProduct(product);
            #endregion


            #region Assert

            this.ShouldSatisfyAllConditions(

               () => _productRepositoryMock.VerifyAll(),
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );


            #endregion
        }

        [Test]
        public void CreateProduct_SameProductExist_CreateProduct()
        {
            //Arrange
            Product product = new Product
            {
                Name = "Mobiles",
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
            _ecommerceUnitOfWorkMock.Setup(x => x.Products)
                .Returns(_productRepositoryMock.Object);

            _productRepositoryMock.Setup(x => x.GetCount(
                It.IsAny<Expression<Func<ProductEntity, bool>>>())).Returns(1);

            //Act
            Should.Throw<DuplicateDataException>(
                () => _productService.CreateProduct(product));

        }

        [Test]
        public void UpdateProduct_SameProductNotExist_UpdateProduct()
        {
            #region Arrange
            Product product = new Product
            {
                Id = 1,
                Name = "Mobiles",
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
            _ecommerceUnitOfWorkMock.Setup(s => s.Products)
                .Returns(_productRepositoryMock.Object);

            _productRepositoryMock.Setup(s => s.GetCount(
              It.IsAny<Expression<Func<ProductEntity, bool>>>())).Returns(0);

            _productRepositoryMock.Setup(s => s.GetById(product.Id))
                .Returns(new ProductEntity
                {
                    Name = product.Name,
                    SalePrice = product.SalePrice,
                    CostPrice = product.CostPrice,

                    Model = product.Model,
                    Brand = product.Brand,
                    ExpireDate = product.ExpireDate,
                    ManufactureDate = product.ManufactureDate,
                    Weight = product.Weight,
                    Size = product.Size,
                    ProductDetails = product.ProductDetails
                });

            _mapperMock.Setup(s => s.Map<ProductEntity>(product))
            .Returns(new ProductEntity()
            {
                Name = product.Name,
                SalePrice = product.SalePrice,
                CostPrice = product.CostPrice,

                Model = product.Model,
                Brand = product.Brand,
                ExpireDate = product.ExpireDate,
                ManufactureDate = product.ManufactureDate,
                Weight = product.Weight,
                Size = product.Size,
                ProductDetails = product.ProductDetails
            });


            #endregion


            #region Act
            _productService.UpdateProduct(product);
            #endregion


            #region Assert

            this.ShouldSatisfyAllConditions(

               () => _productRepositoryMock.VerifyAll(),
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );


            #endregion
        }

        [Test]
        public void UpdateProduct_SameProductExist_UpdateProduct()
        {
            //Arrange
            Product product = new Product
            {
                Name = "Mobiles",
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


            _ecommerceUnitOfWorkMock.Setup(x => x.Products)
                .Returns(_productRepositoryMock.Object);

            _productRepositoryMock.Setup(x => x.GetCount(
                It.IsAny<Expression<Func<ProductEntity, bool>>>())).Returns(1);

            //Act
            Should.Throw<DuplicateDataException>(
                () => _productService.UpdateProduct(product));

        }

        [Test]
        public void DeleteProduct_DeleteCategory()
        {
            #region Arrange
            Product product = new Product
            {
                Id = 1,
                Name = "Mobiles",
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
            _ecommerceUnitOfWorkMock.Setup(s => s.Products)
                .Returns(_productRepositoryMock.Object);

            _productRepositoryMock.Setup(x => x.Remove(
                It.IsAny<Expression<Func<ProductEntity, bool>>>())).Verifiable();


            #endregion


            #region Act
            _productService.DeleteProduct(product.Id);
            #endregion


            #region Assert

            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );


            #endregion
        }

        [Test]
        public void GetProduct_ValidId_ReturnsProduct()
        {
            // Arrange
            var id = 3;

            ProductEntity productEntity = new ProductEntity
            {
                Id = 3
            };
            Product product = new Product() { Id = productEntity.Id };

            _ecommerceUnitOfWorkMock.Setup(x => x.Products)
                .Returns(_productRepositoryMock.Object);

            _productRepositoryMock.Setup(x => x.GetById(id)).Returns(productEntity);

            _mapperMock.Setup(x => x.Map<Product>(productEntity))
                .Returns(product);

            // Act
            var result = _productService.GetProduct(id);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Id.ShouldBe(id)
            );
        }

        [Test]
        public void GetProducts_ReturnsProducts()
        {
            List<ProductEntity> subCategoriesEntities = new List<ProductEntity>();

            _ecommerceUnitOfWorkMock.Setup(x => x.Products)
                .Returns(_productRepositoryMock.Object);

            _productRepositoryMock.Setup(x => x.GetAll()).Returns(subCategoriesEntities);

            // Act
            var result = _productService.GetProducts();

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull()
            );

        }

        [Test]
        public void GetProducts_ReturnProductsTotalTotalDisplay()
        {
            int pageIndex = 1;
            int pageSize = 1;
            string orderBy = string.Empty;

            List<ProductEntity> productEntities = new List<ProductEntity>();

            int Total = 5;
            int TotalDisplay = 10;

            _ecommerceUnitOfWorkMock.Setup(x => x.Products)
               .Returns(_productRepositoryMock.Object);

            _productRepositoryMock.Setup(s => s.GetDynamic(
              It.IsAny<Expression<Func<ProductEntity, bool>>>(), orderBy,
              string.Empty, pageIndex, pageSize, true)).Returns((productEntities, Total, TotalDisplay)); //How to return multiple values??


            // Act
            var result = _productService.GetProducts(pageIndex, pageSize, string.Empty, string.Empty);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.total.ShouldBe(Total),
                () => result.totalDisplay.ShouldBe(TotalDisplay)

            );
        }

        #endregion
    }
}