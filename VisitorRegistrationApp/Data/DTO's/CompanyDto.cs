using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;
using VisitorRegistrationApp.Models;
using BLL.Helper;

namespace VisitorRegistrationApp.Data.Profile
{
    public class CompanyDto : AutoMapper.Profile
    {
        public CompanyDto()
        {
            this.CreateMap<Company, CompanyViewModel>()
                .ReverseMap();


        }           
    }
}
