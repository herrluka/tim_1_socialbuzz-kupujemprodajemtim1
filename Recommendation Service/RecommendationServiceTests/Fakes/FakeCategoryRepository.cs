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
                    Id = 4,
                    CategoryName = "Automobili",
                    CategoryRank = 5
                },
            };
        }

        public void CreateNewCategory(Category category)
        {
            var allCategories = GetAllCategories();
            foreach (var cat in allCategories)
            {
                if (cat.Id == category.Id)
                {
                    throw new Exception();
                }
            }
        }
        public Category GetCategoryById(int categoryId)
        {
            var categories = GetAllCategories();
            return categories.FirstOrDefault(c => c.Id == categoryId);
        }

        public void DeleteCategory(Category category)
        {
            
        }

        public void UpdateCategory(Category category)
        {
            
        }
    }
}
