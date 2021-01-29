using ReactionsService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionsService.Data
{
    public class BlackListMockRepository : IBlackListMockRepository
    {
        public static List<BlackListDto> BlackList { get; set; } = new List<BlackListDto>();

        public BlackListMockRepository()
        {
            FillData();
        }


        private void FillData()
        {
            BlackListDto b = new BlackListDto();
            b.BlackListID = Guid.Parse("CFD7FA84-8A27-4119-B6DB-5CFC1B0C94E1");
            b.BlockerID = 4;
            b.BlockedID = 2;

            BlackList.Add(b);

        }
        public List<int> GetListOfBlockedUsers(int userID)
        {
            List<int> usersID = new List<int>();

            var query = from l1 in BlackList
                        select l1; 

            foreach (var v in query)
            {
               if(v.BlockedID == userID)
                {
                    usersID.Add(v.BlockerID);
                }
               else if(v.BlockerID == userID)
                {
                    usersID.Add(v.BlockedID);
                }
            }

            return usersID;
        }

        public bool DidIBlockedSeler(int userID, int sellerID)
        {

            var query = from l1 in BlackList
                        select l1;

            foreach (var v in query)
            {
                if (v.BlockedID == userID && v.BlockerID == sellerID)
                {
                   
                        return true;
                    
                }
                else if (v.BlockerID == userID && v.BlockedID == sellerID)
                {
                   
                        return true;
                    
                }
            }

            return false;
        }
    }
}
