using ECommerce.Core.DbContexts;
using ECommerce.Core.Entities.Common;
using ECommerce.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories.ForProduct
{
    public class ImageRepository : Repository<Image, int>, IImageRepository
    {
        public ImageRepository(ICoreDbContext context) : base((DbContext)context)
        {
        }
    }
}
