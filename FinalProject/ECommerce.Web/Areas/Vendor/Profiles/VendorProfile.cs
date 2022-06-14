using ECommerce.Infrastructure.BusinessObjects.Products; 
﻿using MapperProfile = AutoMapper.Profile; 
using ECommerce.Infrastructure.BusinessObjects.Common; 
using ECommerce.Web.Areas.StoreAdmin.Models.CategoryModels;
using ECommerce.Web.Areas.StoreAdmin.Models.ProductModels; 
using ECommerce.Web.Areas.StoreAdmin.Models.SubCategoryModdels; 
using ECommerce.Web.Areas.Vendor.Models.ProductModels;  
using ECommerce.Web.Areas.Vendor.Models.BillPayModels; 
using ECommerce.Web.Areas.Vendor.Models.ForDiscount; 

namespace ECommerce.Web.Areas.Vendor.Profiles
{
    public class VendorProfile : MapperProfile
    {
        public VendorProfile()
        { 
            CreateMap<CategoryCreateViewModel, Category>()
                .ReverseMap();
            CreateMap<ProductUpdateViewModel, Product>()
                .ReverseMap();
            CreateMap<SubCategoryCreateModel, SubCategory>()
                .ReverseMap();
            CreateMap<ProductListViewModel,Product>()
                .ReverseMap();
            CreateMap<SubCategoryListViewModel, SubCategory>()
                .ReverseMap();
            CreateMap<CategoryListViewModel, Category>()
                .ReverseMap();
            CreateMap<CreateStorePaymentModel, StorePayments>()
                .ReverseMap();  
            CreateMap<CategoryCreateViewModel, Category>()
                .ReverseMap();
            CreateMap<ProductUpdateViewModel, Product>()
                .ReverseMap();
            CreateMap<SubCategoryCreateModel, SubCategory>()
                .ReverseMap();
            CreateMap<ProductListViewModel,Product>()
                .ReverseMap();
            CreateMap<SubCategoryListViewModel, SubCategory>()
                .ReverseMap();
            CreateMap<CategoryListViewModel, Category>()
                .ReverseMap();
            CreateMap<DiscountListViewModel, Discount>()
                .ReverseMap();
            CreateMap<DiscountCreateModel, Discount>()
                .ReverseMap();
            CreateMap<DiscountUpdateModel, Discount>()
                .ReverseMap();
            CreateMap<InventoryAlertCountModel, InventoryAlert>()
                .ReverseMap();
        }
    }
}
