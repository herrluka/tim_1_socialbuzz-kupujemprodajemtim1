using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Entities;

namespace WebApplication1.Data
{
    public interface IProductMockRepository
    {
        ProductDto GetProductByID(int ID);
    }
}
