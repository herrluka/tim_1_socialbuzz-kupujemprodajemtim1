using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Entities;

namespace WebApplication1.Data
{
    public class ProductMockRepository : IProductMockRepository
    {
        public static List<ProductDto> Products { get; set; } = new List<ProductDto>();

        public ProductMockRepository()
        {
            FillData();
        }


        private void FillData()
        {
            Products.AddRange(new List<ProductDto>
            {
                new ProductDto
                {
                    ProductID = 2,
                    ProductName = "Polica",
                    SellerID = 1,
                    Description = "Polica za knjige",
                    Weight = "1 kg",
                    PriceID = 1,
                    CurrencyID = 1,
                    ProductConditionID = 1,
                    CategoryID = 492,
                    Quantity = 1,
                    PublicationDate = DateTime.Parse("2020-11-15T09:00:00")
                },
                new ProductDto
                {
                    ProductID = 4,
                    ProductName = "Sto",
                    SellerID = 1,
                    Description = "Sto za kancelariju",
                    Weight = "5 kg",
                    PriceID = 6,
                    CurrencyID = 1,
                    ProductConditionID = 2,
                    CategoryID = 223,
                    Quantity = 2,
                    PublicationDate = DateTime.Parse("2020-09-15T09:00:00")
                },
                 new ProductDto
                {
                    ProductID = 7,
                    ProductName = "Solje",
                    SellerID = 19,
                    Description = "Solje za kafu",
                    Weight = "300 g",
                    PriceID = 3,
                    CurrencyID = 1,
                    ProductConditionID = 2,
                    CategoryID = 197,
                    Quantity = 10,
                    PublicationDate = DateTime.Parse("2020-02-15T09:00:00")
                }
            });



        }

        public ProductDto GetProductByID(int ID)
        {
            return Products.FirstOrDefault(e => e.ProductID == ID);

        }

    }
}
