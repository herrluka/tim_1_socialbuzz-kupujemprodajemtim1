using AutoMapper;
using ReactionsService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace ReactionsService.Profiles
{
    public class ReactionProfile : Profile
    {
        public ReactionProfile()
        {
            CreateMap<ReactionUpdateDto, Reactions>();

            CreateMap<ReactionCreateDto, Reactions>();

            CreateMap<Reactions, ReactionsDto>();

            CreateMap<Reactions, Reactions>();
        }

    }
}
