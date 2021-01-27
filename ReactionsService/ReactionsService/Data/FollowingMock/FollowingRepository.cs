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
            FollowingList.AddRange(new List<FollowingDto>
            {
                new FollowingDto
                {
                    FollowingID = 1,
                    FollowerID = 3,
                    FollowedID = 1

                },
                new FollowingDto
                {
                    FollowingID = 2,
                    FollowerID = 1,
                    FollowedID = 6
                }
            });

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
                if (v.FollowerID == userID)
                {
                    if(v.FollowedID == sellerID)
                    {
                        return true;
                    }
                }

            }

            return false;
        }
    }
}
