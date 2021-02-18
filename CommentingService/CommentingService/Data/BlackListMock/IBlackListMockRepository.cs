using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentingService.Data
{
    public interface IBlackListMockRepository
    {

        public bool DidIBlockedSeler(int userID, int sellerID);

        List<int> GetListOfBlockedUsers(int userID);
    }
}
