using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Entities;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using ReactionsService.Data;

namespace WebApplication1.Data
{
    public class ReactionRepository : IReactionRepository
    {
        private readonly ContextDB context;
        private readonly IBlackListMockRepository blackListRepository;

        public ReactionRepository(ContextDB contextDB, IBlackListMockRepository blackListRepository)
        {
            context = contextDB;
            this.blackListRepository = blackListRepository;
        }

        public List<Reactions> GetReactions(int userID)
        {        
           var query = from reaction in context.Reactions
                       where !(from o in blackListRepository.GetListOfBlockedUsers(userID)
                               select o).Contains(reaction.UserID) 
                       select reaction;

           // return context.Reactions.FromSqlRaw("select * from Reactions where UserID not in (select blocked from blacklist where bloker = 6) and UserID not in (select bloker from blacklist where blocked= 6)").ToList();
            //return context.Reactions.ToList();
           // return context.Reactions.FromSqlRaw("select * from Reactions where productID = {0}", productID).ToList();

            return query.ToList();

        }

        public List<Reactions> GetRectionByProductID(int productID, int userID)
        {

            var query = from reaction in context.Reactions
                        where !(from o in blackListRepository.GetListOfBlockedUsers(userID)
                                select o).Contains(reaction.UserID)
                        where reaction.ProductID == productID
                        select reaction;

            return query.ToList();

            //return context.Reactions.FromSqlRaw("select * from Reactions where productID = {0}", productID).ToList();

        }



        public Reactions GetReactionByID(Guid reactionID)
        {
            return context.Reactions.FirstOrDefault(e => e.ReactionID == reactionID);

        }


        public void AddReaction(Reactions reaction)
        {
            context.Add(reaction);
        }

        public void UpdateReaction(Reactions reaction)
        {

        }

        public void DeleteReaction(Guid reactionID)
        {
            var reaction = GetReactionByID(reactionID);
            context.Remove(reaction);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

    }
}
