using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentingService.Models
{
    /// <summary>
    ///Dto model Black liste
    /// </summary>
    public class BlackListDto
    {
        /// <summary>
        /// ID BlackListe
        /// </summary>
        private Guid blackListID;

        public Guid BlackListID
        {
            get { return blackListID; }
            set { blackListID = value; } 
        }


        /// <summary>
        /// ID korisnika koji je izvrsio blokiranje
        /// </summary>
        private int blockerID;
        public int BlockerID
        {
            get { return blockerID; }
            set { blockerID = value; }
        }

        /// <summary>
        /// ID korisnika koji je blokiran od strane drugog korisnika
        /// </summary>
        private int blockedID;
        public int BlockedID
        {
            get { return blockedID; }
            set { blockedID = value; }
        }
    }
}
