using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Api.Products.Profile
{
    public class ProductProfile : AutoMapper.Profile
    {
        public ProductProfile()
        {
            CreateMap<Db.Product, Models.Product>();
        }
    }
}
