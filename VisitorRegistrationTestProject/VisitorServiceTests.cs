using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using VisitorRegistrationApp.Helper;
using BL.Services;
using Moq;
using DAL.Repositories;
using BLL.Helper;
using Microsoft.AspNetCore.Identity;
using VisitorRegistrationApp.Data;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VisitorRegistrationTestProject
{
    
    public class VisitorServiceTests
    {
        private readonly VisitorService visitorService;
        private readonly Mock<IVisitorRepository> _visitorRepoMock = new Mock<IVisitorRepository>();
        private readonly Mock<IPhotoService> _photoMock = new Mock<IPhotoService>();
        private readonly Mock<UserManager<ApplicationUser>> userMock = new Mock<UserManager<ApplicationUser>>();


        public VisitorServiceTests()
        {
            visitorService = new VisitorService(userMock.Object, _visitorRepoMock.Object, _photoMock.Object); 
        }

        [Fact]
        public void  SplitFullnameInFirstAndLast_ShouldReturnAFirstAndLastname_FromAFullName()
        {
            //Arrange 
            var splitName = visitorService.SplitFullnameInFirstAndLast("Arno Slabbinck");
            var firstName = splitName[0];
            var lastName = splitName[1];

            //Act 
            string firstname = "arno";
            string lastname = "slabbinck";
            //Assert

            Assert.AreEqual(firstname, firstName);
            Assert.AreEqual(lastname, lastName);
        }
    }
}
