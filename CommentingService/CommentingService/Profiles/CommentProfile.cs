using AutoMapper;
using CommentingService.Model;
using CommentingService.Model.CreateDTO;
using CommentingService.Model.UpdateDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentingService.Profiles
{
    public class CommentProfile : Profile 
    {
        public CommentProfile()
        {
            CreateMap<CommentUpdateDto, Comments>();

            CreateMap<CommentCreateDto, Comments>();

            CreateMap<Comments, CommentsDto>();

            CreateMap<Comments, Comments>();
        }

    }
}
