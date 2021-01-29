using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionsService.Model.Mocks
{
    /// <summary>
    /// Mock koji predstavlja model pracenja korisnika
    /// </summary>
    public class FollowingDto
    {
        /// <summary>
        /// ID Following-a
        /// </summary>
        private int followingID;

        public int FollowingID
        {
            get { return followingID; }
            set { followingID = value; }
        }

        /// <summary>
        /// ID korisnika koji prati drugog korisnika(followed-a)
        /// </summary>
        private int followerID;
        public int FollowerID
        {
            get { return followerID; }
            set { followerID = value; }
        }

        /// <summary>
        /// ID korisnika koji je zapracen od strane drugog korisnika (follower-a)
        /// </summary>
        private int followedID;
        public int FollowedID
        {
            get { return followedID; }
            set { followedID = value; }
        }
    }
}
