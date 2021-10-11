using NUnit.Framework;
using System.Threading.Tasks;

namespace VisitorsRegistrationTests
{
    [TestFixture]
    public class CompanyServiceTests
    {
        private readonly InjectFixture _injectFixture;

        public CompanyServiceTests()
        {
         
            _injectFixture = new InjectFixture();//All mock object are setup in injectFixture class

         

        }
        [Test]
        public void CompanyRepositoryGetAll_ShouldReturnAllCompanies()
        {

            //_injectFixture._companyRepoMock.Setup(x => x.GetAll()).Returns(Task.FromResult(_injectFixture.secondCompanies)); 
            _injectFixture._companyRepoMock.Object.GetAll();
            _injectFixture._companyRepoMock.Verify(x => x.GetAll());
        
        }

        [Test]
        public void CompanyRepositoryGet_ShouldReturnOneCompany()
        {
            //Verify if i can get one specific O
        }
    }
}
