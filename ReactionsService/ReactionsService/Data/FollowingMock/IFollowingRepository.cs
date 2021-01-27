using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionsService.Data.FollowingMock
{
    public interface IFollowingRepository
    {
        List<int> GetListOfFollowedUsers(int followerID);

        bool DoIFolloweSeler(int userID, int sellerID);


    }
}
