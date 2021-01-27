using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Entities;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using ReactionsService.Data;
using ReactionsService.Data.FollowingMock;

namespace WebApplication1.Data
{
    public class ReactionRepository : IReactionRepository
    {
        private readonly ContextDB context;
        private readonly IBlackListMockRepository blackListRepository;
        private readonly IFollowingRepository followingRepository;


        public ReactionRepository(ContextDB contextDB, IBlackListMockRepository blackListRepository, IFollowingRepository followingRepository)
        {
            context = contextDB;
            this.followingRepository = followingRepository;
            this.blackListRepository = blackListRepository;
        }

        public List<Reactions> GetReactions()
        {        

            return context.Reactions.ToList();

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


        public bool CheckDoIFollowSeller(int userID, int sellerID)
        {
            return followingRepository.DoIFolloweSeler(userID, sellerID);
        }

        public bool CheckDidIBlockedSeller(int userID, int sellerID)
        {
            return blackListRepository.DidIBlockedSeler(userID, sellerID);
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

        public Reactions CheckUserWithProductID(int userID, int productID)
        {
            return context.Reactions.FirstOrDefault(e => e.UserID == userID && e.ProductID == productID);

        }


        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

    }
}
