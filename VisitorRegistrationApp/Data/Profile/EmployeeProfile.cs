using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;
using VisitorRegistrationApp.Models;


namespace VisitorRegistrationApp.Data.Profile
{
    public class EmployeeProfile : AutoMapper.Profile
    {
        public EmployeeProfile()
        {
            this.CreateMap<Employee, EmployeeViewModel>().ReverseMap();

           
        }
    }
}
