using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using VisitorRegistrationApp.Models;

namespace VisitorRegistrationApp.Data.Profile
{
    public class UserProfile : AutoMapper.Profile
    {
        public UserProfile() 
        {
            this.CreateMap<ApplicationUser, VisitorViewModel>();
        }
    }
}
