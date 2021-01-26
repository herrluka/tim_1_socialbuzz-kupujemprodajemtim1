using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionsService.Models
{
    /// <summary>
    ///Dto model Black liste
    /// </summary>
    public class BlackListDto
    {
        /// <summary>
        /// ID BlackListe
        /// </summary>
        public Guid BlackListID;

        /// <summary>
        /// ID korisnika koji je izvrsio blokiranje
        /// </summary>
        public int BlockerID;

        /// <summary>
        /// ID korisnika koji je blokiran od strane drugog korisnika
        /// </summary>
        public int BlockedID;
    }
}
