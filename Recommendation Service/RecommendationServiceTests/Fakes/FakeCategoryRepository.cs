using Recommendation_Service.Data.Interfaces;
using Recommendation_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationServiceTests.Fakes
{
    class FakeCategoryRepository : ICategoryRepository
    {
        public void CreateNewCategory(Category category)
        {
            
        }

        public List<Category> GetAllCategories()
        {
            return new List<Category>
            {
                new Category()
                {
                    Id = 1,
                    CategoryName = "Mobilni telefoni",
                    CategoryRank = 3
                },
                new Category()
                {
                    Id = 2,
                    CategoryName = "Laptopovi i racunari",
                    CategoryRank = 3
                },
                new Category()
                {
                    Id = 3,
                    CategoryName = "Knjige",
                    CategoryRank = 2
                },
                new Category()
                {
                    Id = 1,
                    CategoryName = "Automobili",
                    CategoryRank = 5
                },
            };
        }
    }
}
