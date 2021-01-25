using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public interface IReactionRepository
    {
        List<Reactions> GetReactions();

        List<Reactions> GetRectionByProductID(int productID);

        public Reactions GetReactionByID(Guid reactionID);

        void AddReaction(Reactions reaction);

        public void UpdateReaction(Reactions reaction);

        public void DeleteReaction(Guid reactionID);

        bool SaveChanges();
    }
}
