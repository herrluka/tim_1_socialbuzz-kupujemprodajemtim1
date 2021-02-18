using CommentingService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentingService.Data.Comment
{
    public interface ICommentRepository
    {
        List<Comments> GetAllComments();

        List<Comments> GetCommentsByProductID(int productID, int userID);

        Comments GetCommentByID(Guid commentID);

        void AddComment(Comments comment);

        void UpdateComment(Comments comment);

        void DeleteComment(Guid commentID);

        bool CheckDoIFollowSeller(int userID, int sellerID);

        bool CheckDidIBlockedSeller(int userId, int sellerID);

        public bool SaveChanges();


    }
}
