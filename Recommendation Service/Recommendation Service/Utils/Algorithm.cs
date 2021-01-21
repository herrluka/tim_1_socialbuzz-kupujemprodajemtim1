using Recommendation_Service.Data;
using Recommendation_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendation_Service.Utils
{
    public class Algorithm
    {
        
        private static readonly int MaxPoints = 250;
        public static int? FindRecommendedCategory(ApplicationDbContext dbContext, int categoryId, double price)
        {
            List<Category> categories = dbContext.Category.ToList<Category>();
            var category = categories.FirstOrDefault(c => c.Id == categoryId);
            if (category == null)
            {
                return null;
            }

            var totalPoints = CalculateCategoryPoints(category.CategoryRank) + CalculatePricePoints(price);
            var maxRank = categories.Max(c => c.CategoryRank);
            var step = MaxPoints / maxRank;
            var counter = 0;
            foreach (var c in categories)
            {
                if (counter < totalPoints && totalPoints < 2 * step)
                {
                    return c.CategoryRank;
                }
            }

            return maxRank;
        }

        public static int CalculateCategoryPoints(int categoryRank)
        {
            if (categoryRank <= 2)
            {
                return 10;
            } else if (2 < categoryRank && categoryRank <= 4)
            {
                return 20;
            } else if (5 < categoryRank && categoryRank <=6)
            {
                return 30;
            } else
            {
                return 50;
            }
        }

        public static int CalculatePricePoints(double categoryPrice)
        {
            if (categoryPrice <= 1000)
            {
                return 5;
            }
            else if (1000 < categoryPrice && categoryPrice <= 5000)
            {
                return 15;
            }
            else if (5000 < categoryPrice && categoryPrice <= 20000)
            {
                return 40;
            }
            else if (20000 < categoryPrice && categoryPrice <= 100000)
            {
                return 100;
            } 
            else
            {
                return 200;
            }
        }
    }
}
