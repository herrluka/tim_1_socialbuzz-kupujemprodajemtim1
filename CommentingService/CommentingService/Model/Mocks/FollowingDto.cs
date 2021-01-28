using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentingService.Model.Mocks
{
    /// <summary>
    /// Mock koji predstavlja model pracenja korisnika
    /// </summary>
    public class FollowingDto
    {
        /// <summary>
        /// ID Following-a
        /// </summary>
        public int FollowingID;

        /// <summary>
        /// ID korisnika koji prati drugog korisnika(followed-a)
        /// </summary>
        public int FollowerID;

        /// <summary>
        /// ID korisnika koji je zapracen od strane drugog korisnika (follower-a)
        /// </summary>
        public int FollowedID;
    }
}
