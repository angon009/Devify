using Autofac.Extras.Moq;
using AutoMapper;
using ECommerce.Infrastructure.BusinessObjects.Stores;
using ECommerce.Infrastructure.Exceptions;
using ECommerce.Infrastructure.Repositories.ForStore;
using ECommerce.Infrastructure.Services.ForStore;
using ECommerce.Infrastructure.UnitOfWorks;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using StoreEntity = ECommerce.Core.Entities.Stores.Store;

namespace ECommerce.Infrastructure.Tests
{
    public class StoreServicesTests
    {
        private AutoMock _mock;
        private Mock<IEcommerceUnitOfWork> _ecommerceUnitOfWorkMock;
        private Mock<IStoreRepository> _storeRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private IStoreService _storeService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose(); // Initializing the mock object 
        }

        [SetUp]
        public void Setup()
        {
            _ecommerceUnitOfWorkMock = _mock.Mock<IEcommerceUnitOfWork>(); //Mocking dependency
            _storeRepositoryMock = _mock.Mock<IStoreRepository>(); //Mocking dependency
            _mapperMock = _mock.Mock<IMapper>(); //Mocking dependency

            _storeService = _mock.Create<StoreService>();// Will test this service class's methods 

        }
        [TearDown]
        public void TearDown()
        {
            //Resetting mock after each test
            _ecommerceUnitOfWorkMock.Reset();
            _storeRepositoryMock.Reset();
            _mapperMock.Reset();
        }
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose(); // Disposing the object 
        }

        #region Tests of Asynchronus Methods

        [Test]
        public async Task CreateStoreAsync_SameStoreNotExist_CreateStore()
        {
            #region Arrange
            Store store = new Store
            {
                StoreName = "Mehader Paner Dokan",
            };
            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(s => s.Stores)
               .Returns(_storeRepositoryMock.Object);
            });

            _storeRepositoryMock.Setup(s => s.GetCountAsync(
              It.IsAny<Expression<Func<StoreEntity, bool>>>())).ReturnsAsync(0);

            await Task.Run(() =>
            {
                _mapperMock.Setup(s => s.Map<StoreEntity>(store))
                .Returns(new StoreEntity()
                {
                    StoreName = store.StoreName
                });
            });

            _storeRepositoryMock.Setup(x => x.AddAsync(It.Is<StoreEntity>
                (y => y.StoreName == store.StoreName
                ))).Returns(Task.FromResult(true)).Verifiable();


            #endregion


            #region Act
            await _storeService.CreateStoreAsync(store);
            #endregion


            #region Assert

            this.ShouldSatisfyAllConditions(

               () => _storeRepositoryMock.VerifyAll(),
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );


            #endregion
        }

        [Test]
        public async Task CreateStoreAsync_SameStoreExist_CreateStore()
        {
            //Arrange
            Store store = new Store
            {
                StoreName = "Mobile Arena"
            };

            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(x => x.Stores)
                    .Returns(_storeRepositoryMock.Object);
            });

            _storeRepositoryMock.Setup(x => x.GetCountAsync(
                It.IsAny<Expression<Func<StoreEntity, bool>>>())).ReturnsAsync(1);

            //Act
            Should.Throw<DuplicateDataException>(
                () => _storeService.CreateStoreAsync(store));

        }

        [Test]
        public async Task UpdateStoreAsync_SameStoreNotExist_UpdateStore()
        {
            #region Arrange
            Store store = new Store
            {
                Id = 1,
                StoreName = "Variety Store"
            };
            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(s => s.Stores)
                    .Returns(_storeRepositoryMock.Object);
            });

            _storeRepositoryMock.Setup(s => s.GetCountAsync(
              It.IsAny<Expression<Func<StoreEntity, bool>>>())).ReturnsAsync(0);

            _storeRepositoryMock.Setup(s => s.GetByIdAsync(store.Id))
                .Returns(Task.FromResult(new StoreEntity
                {
                    StoreName = store.StoreName
                }));

            await Task.Run(() =>
            {
                _mapperMock.Setup(s => s.Map<StoreEntity>(store))
                .Returns(new StoreEntity()
                {
                    StoreName = store.StoreName
                });
            });

            #endregion


            #region Act
            await _storeService.UpdateStoreAsync(store);
            #endregion


            #region Assert

            this.ShouldSatisfyAllConditions(

               () => _storeRepositoryMock.VerifyAll(),
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );

            #endregion
        }

        [Test]
        public async Task UpdateStoreAsync_SameStoreExist_UpdateStore()
        {
            //Arrange
            Store store = new Store
            {
                StoreName = "Riader Kaporer Dokan"
            };

            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(x => x.Stores)
                    .Returns(_storeRepositoryMock.Object);
            });

            _storeRepositoryMock.Setup(x => x.GetCountAsync(
                It.IsAny<Expression<Func<StoreEntity, bool>>>())).ReturnsAsync(1);

            //Act
            Should.Throw<DuplicateDataException>(
                () => _storeService.UpdateStoreAsync(store));

        }

        [Test]
        public async Task DeleteStoreAsync_DeleteCategory()
        {
            #region Arrange
            Store store = new Store
            {
                Id = 1,
                StoreName = "Mistanno Vander"
            };
            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(s => s.Stores)
                    .Returns(_storeRepositoryMock.Object);
            });

            _storeRepositoryMock.Setup(x => x.RemoveAsync(
                It.IsAny<Expression<Func<StoreEntity, bool>>>())).
                Returns(Task.FromResult(true)).Verifiable();


            #endregion


            #region Act
            await _storeService.DeleteStoreAsync(store.Id);
            #endregion


            #region Assert

            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );

            #endregion
        }

        [Test]
        public async Task GetStoreAsync_ValidId_ReturnsStore()
        {
            // Arrange
            var id = 3;

            StoreEntity storeEntity = new StoreEntity
            {
                Id = 3
            };
            Store store = new Store() { Id = storeEntity.Id };

            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(x => x.Stores)
                .Returns(_storeRepositoryMock.Object);
            });

            _ecommerceUnitOfWorkMock.Setup(x => x.Stores)
                .Returns(_storeRepositoryMock.Object);

            _storeRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(storeEntity);

            await Task.Run(() =>
            {
                _mapperMock.Setup(x => x.Map<Store>(storeEntity))
                .Returns(store);
            });


            // Act
            var result = await _storeService.GetStoreAsync(id);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Id.ShouldBe(id)
            );
        }

        [Test]
        public async Task GetStoresAsync_ReturnsStores()
        {
            List<StoreEntity> storeEntity = new List<StoreEntity>();

            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(x => x.Stores)
                    .Returns(_storeRepositoryMock.Object);
            });

            _storeRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(storeEntity);

            // Act
            var result = await _storeService.GetStoresAsync();

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull()
            );

        }

        [Test]
        public async Task GetStoresAsync_ReturnStoresTotalTotalDisplay()
        {
            int pageIndex = 1;
            int pageSize = 1;
            string orderBy = string.Empty;

            List<StoreEntity> storeEntities = new List<StoreEntity>();

            int Total = 5;
            int TotalDisplay = 10;

            await Task.Run(() =>
            {
                _ecommerceUnitOfWorkMock.Setup(x => x.Stores)
                   .Returns(_storeRepositoryMock.Object);
            });

            _storeRepositoryMock.Setup(s => s.GetDynamicAsync(
              It.IsAny<Expression<Func<StoreEntity, bool>>>(), orderBy,
              string.Empty, pageIndex, pageSize, true)).
              ReturnsAsync((storeEntities, Total, TotalDisplay)); //How to return multiple values??


            // Act
            var result = await _storeService.GetStoresAsync(pageIndex, pageSize,
                string.Empty, string.Empty);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.total.ShouldBe(Total),
                () => result.totalDisplay.ShouldBe(TotalDisplay)

            );
        }
        #endregion



        #region Tests of Non-AsynchronusMethods
        [Test]
        public void CreateStore_SameStoreNotExist_CreateStore()
        {
            #region Arrange
            Store store = new Store
            {
                StoreName = "Sabbirer Mudikhanar Dokan"
            };
            _ecommerceUnitOfWorkMock.Setup(s => s.Stores)
           .Returns(_storeRepositoryMock.Object);

            _storeRepositoryMock.Setup(s => s.GetCount(
              It.IsAny<Expression<Func<StoreEntity, bool>>>())).Returns(0);


            _mapperMock.Setup(s => s.Map<StoreEntity>(store))
            .Returns(new StoreEntity()
            {
                StoreName = store.StoreName
            });

            _storeRepositoryMock.Setup(x => x.Add(It.Is<StoreEntity>
                (y => y.StoreName == store.StoreName))).Verifiable();


            #endregion


            #region Act
            _storeService.CreateStore(store);
            #endregion


            #region Assert

            this.ShouldSatisfyAllConditions(

               () => _storeRepositoryMock.VerifyAll(),
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );


            #endregion
        }

        [Test]
        public void CreateStore_SameStoreExist_CreateStore()
        {
            //Arrange
            Store store = new Store
            {
                StoreName = "Mobiles"
            };
            _ecommerceUnitOfWorkMock.Setup(x => x.Stores)
                .Returns(_storeRepositoryMock.Object);

            _storeRepositoryMock.Setup(x => x.GetCount(
                It.IsAny<Expression<Func<StoreEntity, bool>>>())).Returns(1);

            //Act
            Should.Throw<DuplicateDataException>(
                () => _storeService.CreateStore(store));

        }

        [Test]
        public void UpdateStore_SameStoreNotExist_UpdateStore()
        {
            #region Arrange
            Store store = new Store
            {
                Id = 1,
                StoreName = "Shakiler Osudher Dokan"
            };
            _ecommerceUnitOfWorkMock.Setup(s => s.Stores)
                .Returns(_storeRepositoryMock.Object);

            _storeRepositoryMock.Setup(s => s.GetCount(
              It.IsAny<Expression<Func<StoreEntity, bool>>>())).Returns(0);

            _storeRepositoryMock.Setup(s => s.GetById(store.Id))
                .Returns(new StoreEntity
                {
                    StoreName = store.StoreName
                });

            _mapperMock.Setup(s => s.Map<StoreEntity>(store))
            .Returns(new StoreEntity()
            {
                StoreName = store.StoreName
            });


            #endregion


            #region Act
            _storeService.UpdateStore(store);
            #endregion


            #region Assert

            this.ShouldSatisfyAllConditions(

               () => _storeRepositoryMock.VerifyAll(),
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );

            #endregion
        }

        [Test]
        public void UpdateStore_SameStoreExist_UpdateStore()
        {
            //Arrange
            Store store = new Store
            {
                StoreName = "Suraiyar Churir Dokan"
            };


            _ecommerceUnitOfWorkMock.Setup(x => x.Stores)
                .Returns(_storeRepositoryMock.Object);

            _storeRepositoryMock.Setup(x => x.GetCount(
                It.IsAny<Expression<Func<StoreEntity, bool>>>())).Returns(1);

            //Act
            Should.Throw<DuplicateDataException>(
                () => _storeService.UpdateStore(store));

        }

        [Test]
        public void DeleteStore_IfExist_DeleteStore()
        {
            #region Arrange
            Store store = new Store
            {
                Id = 1,
                StoreName = "Shawon Tea Stall"
            };
            _ecommerceUnitOfWorkMock.Setup(s => s.Stores)
                .Returns(_storeRepositoryMock.Object);

            _storeRepositoryMock.Setup(x => x.Remove(
                It.IsAny<Expression<Func<StoreEntity, bool>>>())).Verifiable();


            #endregion


            #region Act
            _storeService.DeleteStore(store.Id);
            #endregion


            #region Assert

            this.ShouldSatisfyAllConditions(
               () => _ecommerceUnitOfWorkMock.VerifyAll()
           );

            #endregion
        }

        [Test]
        public void GetStore_ValidId_ReturnsStore()
        {
            // Arrange
            var id = 3;

            StoreEntity storeEntity = new StoreEntity
            {
                Id = 3
            };
            Store store = new Store() { Id = storeEntity.Id };

            _ecommerceUnitOfWorkMock.Setup(x => x.Stores)
                .Returns(_storeRepositoryMock.Object);

            _storeRepositoryMock.Setup(x => x.GetById(id)).Returns(storeEntity);

            _mapperMock.Setup(x => x.Map<Store>(storeEntity))
                .Returns(store);

            // Act
            var result = _storeService.GetStore(id);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Id.ShouldBe(id)
            );
        }

        [Test]
        public void GetStores_ReturnsStores()
        {
            List<StoreEntity> storeEntities = new List<StoreEntity>();

            _ecommerceUnitOfWorkMock.Setup(x => x.Stores)
                .Returns(_storeRepositoryMock.Object);

            _storeRepositoryMock.Setup(x => x.GetAll()).Returns(storeEntities);

            // Act
            var result = _storeService.GetStores();

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull()
            );

        }

        [Test]
        public void GetStores_ReturnStoresTotalTotalDisplay()
        {
            int pageIndex = 1;
            int pageSize = 1;
            string orderBy = string.Empty;

            List<StoreEntity> storeEntities = new List<StoreEntity>();

            int Total = 5;
            int TotalDisplay = 10;

            _ecommerceUnitOfWorkMock.Setup(x => x.Stores)
               .Returns(_storeRepositoryMock.Object);

            _storeRepositoryMock.Setup(s => s.GetDynamic(
              It.IsAny<Expression<Func<StoreEntity, bool>>>(), orderBy,
              string.Empty, pageIndex, pageSize, true)).
              Returns((storeEntities, Total, TotalDisplay)); //How to return multiple values??


            // Act
            var result = _storeService.GetStores(pageIndex, pageSize,
                string.Empty, string.Empty);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.total.ShouldBe(Total),
                () => result.totalDisplay.ShouldBe(TotalDisplay)

            );
        }

        #endregion
    }
}