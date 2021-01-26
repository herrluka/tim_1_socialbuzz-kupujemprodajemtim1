using AutoMapper;
using ReactionsService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace ReactionsService.Profiles
{
    public class TypeOfReactionProfile : Profile
    {

        public TypeOfReactionProfile()
        {
            CreateMap<TypeOfReactionUpdateDto, TypeOfReaction>();

            CreateMap<TypeOfReactionCreateDto, TypeOfReaction>();

            CreateMap<TypeOfReaction, TypeOfReactionDto>();

            CreateMap<TypeOfReaction, TypeOfReaction>();
        }
    }
}
