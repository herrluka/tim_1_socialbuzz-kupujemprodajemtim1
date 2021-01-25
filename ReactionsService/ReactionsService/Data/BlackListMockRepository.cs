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
            BlackList.AddRange(new List<BlackListDto>
            {
                new BlackListDto
                {
                    BlackListID = Guid.Parse("CFD7FA84-8A27-4119-B6DB-5CFC1B0C94E1"),
                    BlockerID = 1,
                    BlockedID = 2

                },
                new BlackListDto
                {
                    BlackListID = Guid.Parse("CFD7FA84-8A27-4119-B6DB-5CFC1B0C94E2"),
                    BlockerID = 5,
                    BlockedID = 6
                },
                   new BlackListDto
                {
                    BlackListID = Guid.Parse("CFD7FA84-3A27-4119-B6DB-5CFC1B0C94E2"),
                    BlockerID = 6,
                    BlockedID = 3
                },
                 new BlackListDto
                {
                    BlackListID = Guid.Parse("CFD7FA84-8A27-4119-B6DB-5CFC1B0C94E3"),
                    BlockerID = 7,
                    BlockedID = 3
                }
            });

        }
        public List<int> GetListOfBlockedUsers(int UserID)
        {
            List<int> usersID = new List<int>();

            var query = from l1 in BlackList
                        select l1; 

            foreach (var v in query)
            {
               if(v.BlockedID == UserID)
                {
                    usersID.Add(v.BlockerID);
                }
               else if(v.BlockerID == UserID)
                {
                    usersID.Add(v.BlockedID);
                }
            }

            return usersID;
        }
    }
}
