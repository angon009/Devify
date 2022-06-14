using AutoMapper;
using ECommerce.Infrastructure.BusinessObjects.Common;
using ECommerce.Infrastructure.BusinessObjects.MessageNotification;
using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.BusinessObjects.Stores;
using AddressEntity = ECommerce.Core.Entities.Stores.Address;
using CategoryEntity = ECommerce.Core.Entities.Products.Category;
using ColorEntity = ECommerce.Core.Entities.Products.Color;
using EmailEntity = ECommerce.Core.Entities.Stores.Email;
using ImageEntity = ECommerce.Core.Entities.Common.Image;
using MessageEntity = ECommerce.Core.Entities.MessageNotification.Message;
using PhoneEntity = ECommerce.Core.Entities.Stores.Phone;
using ProductColorEntity = ECommerce.Core.Entities.Products.ProductColor;
using ProductEntity = ECommerce.Core.Entities.Products.Product;
using StockDetailEntity = ECommerce.Core.Entities.Stores.StockDetail;
using StockEntity = ECommerce.Core.Entities.Stores.Stock;
using StoreEntity = ECommerce.Core.Entities.Stores.Store;
using StoreStatusEntity = ECommerce.Core.Entities.Stores.StoreStatus;
using SubCategoryEntity = ECommerce.Core.Entities.Products.SubCategory; 
using DiscountEntity = ECommerce.Core.Entities.Products.Discount;
using CartEntity = ECommerce.Core.Entities.Orders.Cart;
using OrderEntity = ECommerce.Core.Entities.Orders.Order;
using OrderDetailEntity = ECommerce.Core.Entities.Orders.OrderDetail; 
using ECommerce.Infrastructure.BusinessObjects.Orders;  
using StorePaymentsEntity = ECommerce.Core.Entities.Common.StorePayments;
using InventoryAlertEntity = ECommerce.Core.Entities.Products.InventoryAlert;

namespace ECommerce.Infrastructure.Profiles
{
    public class InfrastructureProfile : Profile
    {
        public InfrastructureProfile()
        {
            CreateMap<SubCategoryEntity, SubCategory>()
                .ReverseMap();  
            CreateMap<StoreEntity, Store>()
                .ReverseMap();
            CreateMap<StoreStatusEntity, StoreStatus>()
                .ReverseMap();
            CreateMap<EmailEntity, Email>()
                .ReverseMap();
            CreateMap<PhoneEntity, Phone>()
                .ReverseMap();
            CreateMap<AddressEntity, Address>()
                .ReverseMap();
            CreateMap<StoreStatusEntity, StoreStatus>()
                .ReverseMap(); 
            CreateMap<CategoryEntity, Category>()
                .ReverseMap(); 
            CreateMap<ProductEntity, Product>()
                .ReverseMap(); 
            CreateMap<StockEntity, Stock>()
                .ReverseMap(); 
            CreateMap<StockDetailEntity, StockDetail>()
                .ReverseMap(); 
            CreateMap<ImageEntity, Image>()
                .ReverseMap(); 
            CreateMap<ProductColorEntity, ProductColor>()
                .ReverseMap(); 
            CreateMap<ColorEntity, Color>()
                .ReverseMap();
            CreateMap<MessageEntity, Message>() 
             .ReverseMap();
            CreateMap<DiscountEntity, Discount>()
             .ReverseMap(); 
            CreateMap<CartEntity, Cart>()
             .ReverseMap(); 
            CreateMap<OrderEntity, Order>()
             .ReverseMap(); 
            CreateMap<OrderDetailEntity, OrderDetail>()
             .ReverseMap();  
            CreateMap<StorePaymentsEntity, StorePayments>()
                .ReverseMap();
            CreateMap<InventoryAlertEntity, InventoryAlert>()
                .ReverseMap();

        }
    }
}
