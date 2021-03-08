using Recommendation_Service.Data.Interfaces;
using Recommendation_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendation_Service.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreateNewCategory(Category category)
        {
            dbContext.Category.Add(category);
            dbContext.SaveChangesAsync();
        }

        public List<Category> GetAllCategories()
        {
            return dbContext.Category.ToList();
        }

        public List<Category> GetAllCategoriesOrderByRank()
        {
            return dbContext.Category.OrderBy(c => c.CategoryRank).ToList();
        }

        public Category GetCategoryById(int categoryId)
        {
            return dbContext.Category.FirstOrDefault(c => c.Id == categoryId);
        }

        public void UpdateCategory(Category category)
        {
            dbContext.Category.Update(category);
            dbContext.SaveChangesAsync();
        }
        public void DeleteCategory(Category category)
        {
            dbContext.Category.Remove(category);
            dbContext.SaveChangesAsync();
        }
    }
}
