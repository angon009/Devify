using Autofac;
using AutoMapper;
using ECommerce.Fascet.ForProduct;
using ECommerce.Infrastructure.BusinessObjects.Common;
using ECommerce.Infrastructure.BusinessObjects.Products;
using ECommerce.Infrastructure.Services.ForProduct;
using ECommerce.Web.Areas.StoreAdmin.Models.CategoryModels;
using ECommerce.Web.Areas.Vendor.Models.ForDiscount;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.Areas.Vendor.Models.ProductModels
{
    public class ProductUpdateViewModel
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public double SalePrice { get; set; }

        public double CostPrice { get; set; }
        public int Quantity { get; set; }

        [Required]
        public string? Model { get; set; }

        [Required]
        public string? Brand { get; set; }

        [Required]
        public DateTime? ExpireDate { get; set; }

        public DateTime? ManufactureDate { get; set; }

        public string? Weight { get; set; }

        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? ProductDetails { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int SubCategoryId { get; set; }
        [Required]
        public int StoreId { get; set; }
        public int DiscountId { get; set; }
        public List<IFormFile>? Images { get; set; }
        public List<CategoryListViewModel>? categoryList { get; set; }
        public List<DiscountListViewModel>? discountList { get; set; }
        public List<Image>? ProductImages { get; set; }
        public SubCategory? SubCatetory { get; set; }
        public ProductUpdateViewModel()
        {

        }


        private IProductUnit _productUnit;
        private IProductService _productService;
        private IMapper _mapper;
        private ILifetimeScope _scope;
        private IWebHostEnvironment _webHostEnvironment;
        public ProductUpdateViewModel(IMapper mapper, IProductUnit productUnit, IProductService productService, IWebHostEnvironment webHostEnvironment)
        {
            _productUnit = productUnit;
            _mapper = mapper;
            _productService = productService;
            _webHostEnvironment = webHostEnvironment;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _productUnit = scope.Resolve<IProductUnit>();
            _productService = scope.Resolve<IProductService>();
            _mapper = scope.Resolve<IMapper>();
            _webHostEnvironment = scope.Resolve<IWebHostEnvironment>();
        }

        internal async Task LoadData(int id)
        {
            var product = await _productService.GetProductAsync(id);
            _mapper.Map(product, this);
            this.SubCatetory = product.SubCategory;
        }

        internal async Task UpdateProduct()
        {
            var product = _mapper.Map<Product>(this);
            if (Images != null)
            {
                foreach (IFormFile file in Images!)
                {
                    Image image = new Image();
                    image.Name = file.Name;
                    image.Url = await UploadImageAsync("Images/Stores/", file);
                    image.ImageFor = "Products";
                    image.ProductId = product.Id;
                    product!.ProductImages!.Add(image);
                };
            }
            await _productUnit.UpdateServiceAsync(product);
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
