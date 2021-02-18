using ReactionsService.Model.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionsService.Data.FollowingMock
{
    public class FollowingRepository : IFollowingRepository
    {
        public static List<FollowingDto> FollowingList { get; set; } = new List<FollowingDto>();

        public FollowingRepository()
        {
            FillData();
        }


        private void FillData()
        {
            FollowingDto f = new FollowingDto();
            f.FollowingID = 1;
            f.FollowerID = 4;
            f.FollowedID = 1;

            FollowingList.Add(f);


        }
        public List<int> GetListOfFollowedUsers(int followerID)
        {
            List<int> listOfFollowedUsers = new List<int>();

            var query = from l1 in FollowingList
                        select l1;

            foreach (var v in query)
            {
                if (v.FollowerID == followerID)
                {
                    listOfFollowedUsers.Add(v.FollowedID);
                }
                
            }

            return listOfFollowedUsers;
        }


        public bool DoIFolloweSeler(int userID, int sellerID)
        {

            var query = from l1 in FollowingList
                        select l1;

            foreach (var v in query)
            {
                if (v.FollowerID == userID && v.FollowedID == sellerID)
                {
                        return true;
                    
                }

            }

            return false;
        }
    }
}
