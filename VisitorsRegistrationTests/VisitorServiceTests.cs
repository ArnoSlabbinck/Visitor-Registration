using BL.Services;
using BLL.Helper;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System.Threading;
using VisitorRegistrationApp.Data;

namespace VisitorsRegistrationTests
{
    [TestFixture]
    public class VisitorServiceTests
    {
        private readonly InjectFixture injectFixture;

        public VisitorServiceTests()
        {
            injectFixture = new InjectFixture();
        }

        [SetUp]
        public void Setup()
        {
            //Opzetten van alle Users 
        }

        [Test]
        [TestCase("Arno Slabbinck", ExpectedResult = "slabbinck")]
        [TestCase("Laura van den Hoeven", ExpectedResult = "van den hoeven ")]
        [TestCase("Bart De Wever", ExpectedResult = "de wever ")]
        public string SplitFullnameInFirstAndLast_ShouldReturnAFirstAndLastname_FromAFullName(string name)
        {
            
            //Arrange 
            var splitName = injectFixture.visitorService.SplitFullnameInFirstAndLast(name);
          
            var lastName = splitName[1];

            //Assert
            return lastName;
        }


        [Test]
        public async string  SearchSpecificUsers_CheckIfUsersInDatbase_IsNotNull()
        {
            // Testing if i can get the right User from the in memory Database using search 
            //Arrange
            var users = injectFixture.visitorService;
            var userOne = await users.userManager.Users
            //Assert
            Assert.IsEmpty();
        }



        [Test]
        [TestCase("Arno Slabbinck")]
        public void SearchSpecificUsers_ShouldReturnListOfUsers_BasedOnSearchTermFullName(string search)
        {
            // Testing if i can get the right User from the in memory Database using search 
            //Arrange
            var users = injectFixture.visitorService.SearchSpecificUsers(search);

            //Act 
            string Username = "Arno Slabbinck";

            //Assert
            Assert.AreEqual(Username, users[0].Fullname);
        }


        [Test]
        [TestCase("Arno.Slabbinck@hotmail.com")]
        public void SearchSpecificUsers_ShouldReturnListOfUsers_BasedOnSearchTermEmail(string search)
        {
            // Testing if i can get the right User from the in memory Database using search 
            // Making a 
            //Arrange
            var users = injectFixture.visitorService.SearchSpecificUsers(search);

            //Act 
            string UserEmail = "Arno.Slabbinck@hotmail.com";
            
            //Assert
            Assert.AreEqual(UserEmail, users[0].Email);
        }
    }
}