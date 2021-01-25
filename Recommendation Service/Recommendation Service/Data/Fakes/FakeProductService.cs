using Recommendation_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Recommendation_Service.Data
{
    public class FakeProductService : IProductService
    {
        public async Task<List<ProductDto>> GetProductByCategoryRankAndCeilingPrice(int categoryRank, double productCeilingPrice)
        {
            await Task.Run(() =>
                Thread.Sleep(500)
            );

            var products = new List<ProductDto>()
            {
                new ProductDto()
                {
                    Id = 1124,
                    DateCreated = new DateTime(2020, 7, 21),
                    Location = "Novi Sad",
                    Name = "Addidas duks kao nov",
                    CategoryId = 2,
                    Price = 2400
                },
                new ProductDto()
                {
                    Id = 1222,
                    DateCreated = new DateTime(2020, 1, 12),
                    Location = "Kraljevo",
                    Name = "Nike Air Jordan 2017",
                    CategoryId = 1,
                    Price = 5500
                },
                new ProductDto()
                {
                    Id = 1245,
                    DateCreated = new DateTime(2020, 8, 1),
                    Location = "Novi Sad",
                    Name = "Roleri muski 45",
                    CategoryId = 4,
                    Price = 3400
                },
                new ProductDto()
                {
                    Id = 1333,
                    DateCreated = new DateTime(2020, 9, 3),
                    Location = "Beograd",
                    Name = "Strunjača 5cm",
                    CategoryId = 4,
                    Price = 2299
                },
                new ProductDto()
                {
                    Id = 1288,
                    DateCreated = new DateTime(2020, 8, 27),
                    Location = "Beograd",
                    Name = "Nike Air Max 2018",
                    CategoryId = 1,
                    Price = 4500
                },
                new ProductDto()
                {
                    Id = 1111,
                    DateCreated = new DateTime(2020, 7, 10),
                    Location = "Požarevac",
                    Name = "McKinley Aktivni veš NOV",
                    CategoryId = 2,
                    Price = 2300
                },
                new ProductDto()
                {
                    Id = 1289,
                    DateCreated = new DateTime(2020, 8, 27),
                    Location = "Novi Sad",
                    Name = "Arduino Uno Ploča + komponente",
                    CategoryId = 9,
                    Price = 2900
                },
                new ProductDto()
                {
                    Id = 1122,
                    DateCreated = new DateTime(2020, 9, 2),
                    Location = "Novi Sad",
                    Name = "Punjač za iPhone ORIGINAL",
                    CategoryId = 9,
                    Price = 2500
                },
                new ProductDto()
                {
                    Id = 1411,
                    DateCreated = new DateTime(2020, 7, 11),
                    Location = "Niš",
                    Name = "Addidas Superstar broj 41",
                    CategoryId = 1,
                    Price = 2300
                },
                new ProductDto()
                {
                    Id = 1333,
                    DateCreated = new DateTime(2020, 9, 3),
                    Location = "Beograd",
                    Name = "Medicinska lopta 5kg",
                    CategoryId = 4,
                    Price = 2800
                },
                new ProductDto()
                {
                    Id = 1521,
                    DateCreated = new DateTime(2020, 11, 7),
                    Location = "Požega",
                    Name = "Muški kaput London Fogg XL",
                    CategoryId = 2,
                    Price = 5000
                },

        }; 

            return products;
        }
    }
}
