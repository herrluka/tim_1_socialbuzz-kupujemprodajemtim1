using authorizationMicroservice.Entities;
using authorizationMicroservice.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authorizationMicroservice.Profiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, User>();
        }
    }
}
