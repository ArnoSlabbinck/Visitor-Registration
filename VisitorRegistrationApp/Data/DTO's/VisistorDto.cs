using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using VisitorRegistrationApp.Models;

namespace VisitorRegistrationApp.Data.Profile
{
    public class VisistorDto : AutoMapper.Profile
    {
        public VisistorDto() 
        {
            this.CreateMap<ApplicationUser, VisitorViewModel>().ReverseMap();

            this.CreateMap<ApplicationUser, SignOutVisitorViewModel>().ReverseMap();
        }
    }
}
