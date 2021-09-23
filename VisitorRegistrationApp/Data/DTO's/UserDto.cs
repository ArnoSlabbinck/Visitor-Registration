using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using VisitorRegistrationApp.Models;

namespace VisitorRegistrationApp.Data.Profile
{
    public class UserDto : AutoMapper.Profile
    {
        public UserDto() 
        {
            this.CreateMap<ApplicationUser, VisitorViewModel>();
        }
    }
}
