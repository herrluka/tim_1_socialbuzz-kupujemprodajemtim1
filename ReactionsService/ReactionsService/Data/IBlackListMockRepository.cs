using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionsService.Data
{
    public interface IBlackListMockRepository
    {

        List<int> GetListOfBlockedUsers(int userID);
    }
}
