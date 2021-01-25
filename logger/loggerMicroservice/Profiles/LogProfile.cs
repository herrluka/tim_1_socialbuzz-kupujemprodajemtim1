using AutoMapper;
using loggerMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace loggerMicroservice.Profiles
{
    public class LogProfile:Profile
    {
        public LogProfile()
        {
            CreateMap<Log, LogDto>();
        }
    }
}
