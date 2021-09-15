using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data;
using VisitorRegistrationApp.Data.Entities;

namespace DAL
{
    /// <summary>
    /// Invullen van Data in de database bij opstart
    /// </summary>
    public static class DbInitializer
    {
        
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Maken van nieuwe users Arno Slabbinck, Ben Wuyts 
            // Maken van bedrijven La source, Quaest, Allphi, Aigendens 
            // Employees in de bedrijven zetten
            if (context.Companies.Any())
            {
                return;// The database has been seeded
            }


            //Invullen van Building
            Building building = new Building() { Name = "4Wings" };
            context.Building.Add(building);
            
            //Invullen van Companies 
            var Companies = new Company[]
            {
                new Company { Building = building, Name = "Allphi"},
                new Company { Building = building, Name = "La Source"},
                new Company { Building = building, Name = "Agidens"},
                new Company { Building = building, Name = "Queast"}
            };

            foreach(var company in Companies)
            {
                context.Companies.Add(company);
            }
            //Invullen van Employees$
            context.SaveChanges();
            var Employees = new Employee[]
            {
                new Employee { Name = "Arno Slabbinck", Job = "Junior Developer",  BirthDay = new DateTime(1994, 09, 29), Company = Companies[0], AtWorkStatus = true, Salary = 2000  },
                new Employee { Name = "Joeri Ceulemans", Job = "Junior Developer",  BirthDay = new DateTime(1985, 01, 01), Company = Companies[0], AtWorkStatus = true, Salary = 2000  },
                new Employee { Name = "Roel VaneerdeWegh", Job = "CEO",  BirthDay = new DateTime(1980, 01, 01), Company = Companies[0], AtWorkStatus = true, Salary = 2000  },
                new Employee { Name = "Dorus Schauwaegers", Job = "CFO",  BirthDay = new DateTime(1985, 01, 01), Company = Companies[0], AtWorkStatus = true, Salary = 2000  },
                new Employee { Name = "Angelo Dajaeghere", Job = "RUM Gent",  BirthDay = new DateTime(1980, 01, 01), Company = Companies[0], AtWorkStatus = true, Salary = 2000  },
                new Employee { Name = "Wim Simons", Job = "RUM Westerlo",  BirthDay = new DateTime(1980, 01, 01), Company = Companies[0], AtWorkStatus = true, Salary = 2000  }
            };


            foreach(var employee in Employees)
            {
                context.Employees.Add(employee);

            }

            //Add van administrator
            var admin = new ApplicationUser
            {
                FirstName = "Arno",
                LastName = "Slabbinck",
                VisitStatus = VisitStatus.Admin,
                UserName = "Arno94",
                NormalizedEmail = "Arno.Slabbinck@hotmail.com",
                Email = "Arno.Slabbinck@hotmail.com",
                NormalizedUserName = "Arno94",
                EmailConfirmed = true,
                PhoneNumber = "0472741605",
                PhoneNumberConfirmed = true,
                PasswordHash = "f3b28ccae31636b3a12540bf58f40e378b2d9e5524311055f8cbde329d723b3b", 
                Gender = true, 
                TwoFactorEnabled = false, 
                AccessFailedCount = 0, 
                LockoutEnabled = false, 
                VisitingCompany = Companies[0]

            };

            context.Users.Add(admin);
            context.SaveChanges();
        }

    }
}
