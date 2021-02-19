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
    }
}
