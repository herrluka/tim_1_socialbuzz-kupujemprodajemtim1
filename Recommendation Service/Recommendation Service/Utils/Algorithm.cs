using Recommendation_Service.Data;
using Recommendation_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendation_Service.Utils
{
    public static class Algorithm
    {
        
        private static readonly int MaxPoints = 250;
        public static int? FindRecommendedCategory(List<Category> categories, int categoryId, double price)
        {

            var category = categories.FirstOrDefault(c => c.Id == categoryId);
            if (category == null)
            {
                return null;
            }

            var maxRank = categories.Max(c => c.CategoryRank);
            var totalPoints = CalculateCategoryPoints(category.CategoryRank, maxRank) + CalculatePricePoints(price);
            var step = MaxPoints / maxRank;
            var counter = 0;
            foreach (var c in categories)
            {
                if (counter < totalPoints && totalPoints < counter + step)
                {
                    return c.CategoryRank;
                }
                counter += step;
            }

            return maxRank;
        }

        public static int CalculateCategoryPoints(int categoryRank, int maxRank)
        {
            if (categoryRank <= maxRank * 0.2)
            {
                return 10;
            } else if (maxRank * 0.2 < categoryRank && categoryRank <= maxRank * 0.5)
            {
                return 20;
            } else if (maxRank * 0.5 < categoryRank && categoryRank <= maxRank * 0.7)
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
