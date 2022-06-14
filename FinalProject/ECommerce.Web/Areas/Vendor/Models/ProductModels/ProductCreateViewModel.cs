using Autofac;
using AutoMapper;
using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Fascet.ForProduct;
using System.ComponentModel.DataAnnotations;
using ECommerce.Infrastructure.BusinessObjects.Common;
using System.Collections.Generic;
using ECommerce.Web.Areas.StoreAdmin.Models.CategoryModels;
using ECommerce.Web.Areas.Vendor.Models.ForDiscount;

namespace ECommerce.Web.Areas.StoreAdmin.Models.ProductModels
{
    public class ProductCreateViewModel
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public double SalePrice { get; set; }
        [Required]
        public double CostPrice { get; set; }

        [Required]
        public string? Model { get; set; }

        [Required]
        public string? Brand { get; set; }

        [Required]
        public DateTime? ExpireDate { get; set; }

        [Required]
        public int? Quantity { get; set; }
        public DateTime? ManufactureDate { get; set; }

        [Required]
        public string? Weight { get; set; }

        [Required]
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? ProductDetails { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int SubCategoryId { get; set; }
        public int StoreId { get; set; }
        public int? DiscountId { get; set; }
        public List<IFormFile> Images { get; set; }
        public List<CategoryListViewModel>? categoryList { get; set; }
        public List<DiscountListViewModel>? discountList { get; set; }

        private IProductUnit _productUnit;
        private IMapper _mapper;
        private ILifetimeScope _scope;
        private IWebHostEnvironment _webHostEnvironment;
        public ProductCreateViewModel()
        {

        }
        public ProductCreateViewModel(IProductUnit productUnit,IMapper mapper
            ,ILifetimeScope scope,IWebHostEnvironment webHostEnvironment)
        {
            _productUnit = productUnit;
            _mapper = mapper;
            _scope = scope;
            _webHostEnvironment = webHostEnvironment;   
        }
        public void Resolve(ILifetimeScope scope)
        {
            _productUnit = scope.Resolve<IProductUnit>();
            _mapper = scope.Resolve<IMapper>();
            _scope = scope;
            _webHostEnvironment= scope.Resolve<IWebHostEnvironment>();
        }
        public async Task CreateProductAsync()
        {
            try
            {
                var product = await MapProductAsync();
                await _productUnit.CreateServiceAsync(product);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        private async Task<Product> MapProductAsync()
        {
            Product product = new Product()
            {
                Name = this.Name!,
                SalePrice = this.SalePrice,
                CostPrice = this.CostPrice,
                Model=this.Model,
                Brand = this.Brand,
                ExpireDate = this.ExpireDate,
                ManufactureDate = this.ManufactureDate,
                Weight = this.Weight,
                Size = this.Size,
                ProductDetails = this.ProductDetails,
                Color=this.Color,
                SubCategoryId=this.SubCategoryId,
                StoreId=this.StoreId,
                Quantity=this.Quantity,
                DiscountId=this.DiscountId,
                ProductImages=new List<Image>()
            };
            foreach(IFormFile file in Images)
            {
                Image image = new Image();
                image.Name = file.Name;
                image.Url = await UploadImageAsync("Images/Stores/", file);
                image.ImageFor = "Products";
                product.ProductImages.Add(image);
            };
            return product;
        }
        private async Task<string> UploadImageAsync(string folderPath, IFormFile file)
        {
            var fileUrl = Guid.NewGuid().ToString() + "_" + file.FileName;
            folderPath += fileUrl;

            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));



            return fileUrl;
        }
        private async Task DeleteImage(string folderPath)
        {
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
