using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;
using VisitorRegistrationApp.Models;


namespace VisitorRegistrationApp.Data.Profile
{
    public class EmployeeDto : AutoMapper.Profile
    {
        public EmployeeDto()
        {
            this.CreateMap<Employee, EmployeeViewModel>().ReverseMap();

           
        }
    }
}
