using CommentingService.Data.FollowingMock;
using CommentingService.Entities;
using CommentingService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentingService.Data.Comment
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ContextDB context;
        private readonly IFollowingRepository followingRepository;
        private readonly IBlackListMockRepository blackListMockRepository;

        public CommentRepository(IBlackListMockRepository blackListMockRepository, ContextDB contextDB, IFollowingRepository followingRepository)
        {
            context = contextDB;
            this.followingRepository = followingRepository;
            this.blackListMockRepository = blackListMockRepository;
        }
        public List<Comments> GetAllComments()
        {
            return context.Comment.ToList();
        }

        public List<Comments> GetCommentsByProductID(int productID, int userID)
        {
            var query = from comment in context.Comment
                        where !(from o in blackListMockRepository.GetListOfBlockedUsers(userID)
                               select o).Contains(comment.UserID)
                        where comment.ProductID == productID
                        select comment;

            return query.ToList();
        }

        public Comments GetCommentByID(Guid commentID)
        {
            return context.Comment.FirstOrDefault(e => e.CommentID == commentID);
        }

        public void AddComment(Comments comment)
        {
            context.Comment.Add(comment);
        }

        public void DeleteComment(Guid commentID)
        {
            var comment = GetCommentByID(commentID);
            context.Remove(comment);
        }

     

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public void UpdateComment(Comments comment)
        {
        }

        public bool CheckDoIFollowSeller(int userID, int sellerID)
        {
            return followingRepository.DoIFolloweSeler(userID, sellerID);
        }

        public bool CheckDidIBlockedSeller(int userID, int sellerID)
        {
            return blackListMockRepository.DidIBlockedSeler(userID, sellerID);
        }

    }
}
