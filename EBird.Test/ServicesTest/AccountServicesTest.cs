using EBird.Application.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using EBird.Domain.Entities;
using EBird.Application.Model.Auth;

namespace EBird.Test.ServicesTest
{
    [TestFixture]
    public class AccountServicesTest
    {
        private readonly IAccountServices _accountServices;

        public AccountServicesTest()
        {
            // Create a mock instance of IAccountServices using Moq
            var mockAccountServices = new Mock<IAccountServices>();
            mockAccountServices.Setup(x => x.GetAllAccount()).ReturnsAsync(new List<AccountResponse> { new AccountResponse() });
            _accountServices = mockAccountServices.Object;
        }

        [SetUp]
        public void Setup()
        {
        }
        
        [Test]
        public void AccountServices_GetAll()
        {
            var result = _accountServices.GetAllAccount();
            Assert.IsNotNull(result);
        }
        
    }
}

