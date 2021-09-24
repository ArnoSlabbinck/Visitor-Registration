using BL.Services;
using BLL.Helper;
using Bogus;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;
using VisitorRegistrationApp.Data.Repository;

namespace VisitorsRegistrationTests
{
    [TestFixture]
    public class CompanyServiceTests
    {
        private readonly Mock<ICompanyRespository> _companyRepoMock = new Mock<ICompanyRespository>();
        private readonly Mock<IPhotoService> _photoMock = new Mock<IPhotoService>();
        private readonly Building building = new Building { Id = 1, Name = "FourWings" };
        private readonly CompanyService companyService;

        private readonly IEnumerable<Company> secondCompanies = new List<Company>() {
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
        public CompanyServiceTests()
        {
            companyService = new CompanyService(_companyRepoMock.Object, _photoMock.Object);
            IEnumerable<Company> companies = new List<Company>() {
                new Company { Id = 1, Building = building, Name = "Allphi", Description = "Consultancy software" },
                new Company { Id = 2, Building  = building, Name = "Quaest", Description = "Accountancy company"},
                new Company { Id = 3, Building = building, Name = "Quroum", Description="AI company"},
                new Company { Id  = 4, Building = building, Name  = "Axiom", Description = "Drone company"}
                };

         

        }
        [Test]
        public void CompanyRepositoryGetAll_ShouldReturnAllCompanies()
        {

            _companyRepoMock.Setup(x => x.GetAll()).Returns(Task.FromResult(secondCompanies)); 
            _companyRepoMock.Object.GetAll();
            _companyRepoMock.Verify(x => x.GetAll());
        
        }

        [Test]
        public void CompanyRepositoryGet_ShouldReturnOneCompany()
        {

        }
    }
}
