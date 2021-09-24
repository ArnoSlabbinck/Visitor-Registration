using BL.Services;
using BLL.Helper;
using Bogus;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data;
using VisitorRegistrationApp.Data.Entities;
using VisitorRegistrationApp.Data.Repository;

namespace VisitorsRegistrationTests
{
    ///Instantiate for mock objects
    public class InjectFixture : IDisposable
    {
        public readonly Mock<ICompanyRespository> _companyRepoMock = new Mock<ICompanyRespository>();
        public readonly Mock<IPhotoService> _photoMock = new Mock<IPhotoService>();
        public readonly Building building = new Building { Id = 1, Name = "FourWings" };
        public readonly CompanyService companyService;
        public readonly UserManager<ApplicationUser> userManager;
        public readonly ApplicationDbContext applicationDbContext;
        public readonly ApplicationDbContext DbContext;

        public readonly VisitorService visitorService;
        public readonly Mock<IVisitorRepository> _visitorRepoMock = new Mock<IVisitorRepository>();


        public readonly IEnumerable<Company> secondCompanies = new List<Company>() {
                new Faker<Company>().RuleFor(x => x.Name, x => x.Person.Company.Name)
                .RuleFor(x => x.Id, x => x.IndexFaker)
                .RuleFor(x => x.Description, x => x.Random.Words()),
                new Faker<Company>().RuleFor(x => x.Name, x => x.Person.Company.Name)
                .RuleFor(x => x.Id, x => x.IndexFaker)
                .RuleFor(x => x.Description, x => x.Random.Words()),
                new Faker<Company>().RuleFor(x => x.Name, x => x.Person.Company.Name)
                .RuleFor(x => x.Id, x => x.IndexFaker)
                .RuleFor(x => x.Description, x => x.Random.Words()),
                new Faker<Company>().RuleFor(x => x.Name, x => x.Person.Company.Name)
                .RuleFor(x => x.Id, x => x.IndexFaker)
                .RuleFor(x => x.Description, x => x.Random.Words()),
                new Faker<Company>().RuleFor(x => x.Name, x => x.Person.Company.Name)
                .RuleFor(x => x.Id, x => x.IndexFaker)
                .RuleFor(x => x.Description, x => x.Random.Words()),
            };


        public InjectFixture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "FakeDatabase")
                .Options;

            DbContext = new ApplicationDbContext(options);
            var users = new List<ApplicationUser>()
            {
                new ApplicationUser { FirstName = "Arno", LastName = "Slabbinck",  Email = "Arno.Slabbinck@hotmail.com", Id = Guid.NewGuid().ToString()}
            }.AsQueryable();

            var fakeUserManager = new Mock<FakeUserManager>();

            fakeUserManager.Setup(x => x.Users).Returns(users);

            fakeUserManager.Setup(x => x.DeleteAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);
            fakeUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            fakeUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            companyService = new CompanyService(_companyRepoMock.Object, _photoMock.Object);
            visitorService = new VisitorService(fakeUserManager.Object,_visitorRepoMock.Object, _photoMock.Object);
        }
        public void Dispose()
        {
            userManager?.Dispose();
           
            
        }
    }
}
