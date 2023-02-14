using EBird.Application.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using EBird.Domain.Entities;
using EBird.Application.Model.Auth;
using EBird.Test.Configurations;
using EBird.Domain.Enums;

namespace EBird.Test.ServicesTest
{
    [TestFixture]
    public class AccountServicesTest
    {
        private readonly IAccountServices _accountServices;

        private Mock<IAccountServices> mockAccountServices = new Mock<IAccountServices>();
        public AccountServicesTest()
        {
            // Create a mock instance of IAccountServices using Moq
            _accountServices = mockAccountServices.Object;
        }


        [Test]
        public async Task AccountServices_GetAll()
        {
            //expected values are in ACCOUNTS_EXCEPTED.json
            dynamic accounts = SeedingServices.LoadJson("ACCOUNTS_EXPECTED.json");
            var accountList = new List<AccountResponse>();
            for (int i = 0; i < accounts.Count; i++)
            {
                var account = accounts[i];
                accountList.Add(new AccountResponse()
                {
                    Email = account.Email,
                    FirstName = account.FirstName,
                    LastName = account.LastName,
                    Username = account.Username,
                    CreateDateTime = DateTime.Now,
                    Description = account.Description
                });  
            }
            mockAccountServices.Setup(x => x.GetAllAccount()).ReturnsAsync(accountList);

            // actual result
            var result = await _accountServices.GetAllAccount();

            Assert.AreEqual(result.Count, accounts.Count);
        }
        [Test]
        public async Task AccountServices_GetAccountById()
        {
            



        }

    }
}

