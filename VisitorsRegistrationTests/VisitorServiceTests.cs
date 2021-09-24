using BL.Services;
using BLL.Helper;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using VisitorRegistrationApp.Data;

namespace VisitorsRegistrationTests
{
    [TestFixture]
    public class VisitorServiceTests
    {
        private readonly VisitorService visitorService;
        private readonly Mock<IVisitorRepository> _visitorRepoMock = new Mock<IVisitorRepository>();
        private readonly Mock<IPhotoService> _photoMock = new Mock<IPhotoService>();



        public VisitorServiceTests()
        {
            visitorService = new VisitorService(_visitorRepoMock.Object, _photoMock.Object);
        }

        [Test]
        [TestCase("Arno Slabbinck", ExpectedResult = "slabbinck ")]
        [TestCase("Laura van den Hoeven", ExpectedResult = "van den hoeven ")]
        [TestCase("Bart De Wever", ExpectedResult = "de wever ")]
        public string SplitFullnameInFirstAndLast_ShouldReturnAFirstAndLastname_FromAFullName(string name)
        {
            
            //Arrange 
            var splitName = visitorService.SplitFullnameInFirstAndLast(name);
          
            var lastName = splitName[1];


            //Assert
            return lastName;
        }
    }
}