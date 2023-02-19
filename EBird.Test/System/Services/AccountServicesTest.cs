using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model.Auth;
using EBird.Application.Services;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using EBird.Infrastructure.Context;
using EBird.Infrastructure.Repositories;
using EBird.Test.MockData;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EBird.Test.System.Services
{
    // public class AccountServicesTest : IDisposable
    // {
    //     protected readonly ApplicationDbContext _context;
    //     private readonly IAccountServices _accountServices;
    //     private readonly IGenericRepository<AccountEntity> _accountRepository;
    //     private readonly IMapper _mapper;

    //     public AccountServicesTest()
    //     {
    //         //create db in memory
    //         var options = new DbContextOptionsBuilder<ApplicationDbContext>()
    //         .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
    //         _context = new ApplicationDbContext(options);
    //         _context.Database.EnsureCreated();
    //         _accountRepository = new GenericRepository<AccountEntity>(_context);

    //         // Initialize the IMapper object
    //         var configuration = new MapperConfiguration(cfg =>
    //         {
    //             cfg.CreateMap<AccountEntity, AccountResponse>();
    //         });
    //         _mapper = configuration.CreateMapper();

    //         _accountServices = new AccountServices(_accountRepository, _mapper);
    //     }

    //     [Test]
    //     public async Task GetAllAccount_ReturnData_WhenDataFound()
    //     {
    //         // Expected values are in ACCOUNTS_EXPECTED.json
    //         for (int i = 0; i < AccountMockData.GetAccountList().Count; i++)
    //         {
    //             await _accountRepository.CreateAsync(AccountMockData.GetAccountList()[i]);
    //         }

    //         // Actual values
    //         var result = await _accountServices.GetAllAccount();

    //         // Assert
    //         Assert.NotNull(result);
    //         Assert.AreEqual(AccountMockData.GetAccountList().Count, result.Count);

    //     }

    //     [Test]
    //     public async Task GetAccountById_ReturnData_WhenDataFound()
    //     {
            

    //     }

    //     //delete db when test is completed
    //     public void Dispose()
    //     {
    //         _context.Database.EnsureDeleted();
    //         _context.Dispose();
    //     }

    // }
    public class AccountServicesTest
    {
        private readonly Mock<IGenericRepository<AccountEntity>> _mockAccountRepository;
        private readonly Mock<IAccountServices> _mockAccountServices;
        private readonly IMapper _mapper;

        private readonly IAccountServices _accountServices;
        public AccountServicesTest()
        {
               // Initialize the IMapper object
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AccountEntity, AccountResponse>();
            });
            _mapper = configuration.CreateMapper();

            _mockAccountRepository = new Mock<IGenericRepository<AccountEntity>>();
            _mockAccountServices = new Mock<IAccountServices>();
            _accountServices = new AccountServices(_mockAccountRepository.Object, _mapper);
        }
        [Test]
        public async Task GetAllAccount_ReturnData_WhenDataFound()
        {
            // Expected values are in ACCOUNTS_EXPECTED.json
            _mockAccountRepository.Setup(x => x.GetAllActiveAsync()).ReturnsAsync(AccountMockData.GetAccountList());

            // Actual values
            var result = await _accountServices.GetAllAccount();

            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);

        }

        [Test]
        public async Task GetAllAccount_ReturnEmptyList_WhenDataNotFound()
        {
            // Expected values are in ACCOUNTS_EXPECTED.json
            _mockAccountRepository.Setup(x => x.GetAllActiveAsync()).ReturnsAsync(new List<AccountEntity>());

            // Actual values
            var result = await _accountServices.GetAllAccount();
            Assert.NotNull(result);
            Assert.AreEqual(0, result.Count); 
        }

        [Test]
        public async Task GetAccountById_ReturnData_WhenDataValid()
        {
           // Expected values are in ACCOUNTS_EXPECTED.json
           var account = AccountMockData.GetAccountList()[0];
           _mockAccountRepository.Setup(x => x.GetByIdAsync(account.Id)).ReturnsAsync(account);

           // Actual values
           var result = await _accountServices.GetAccountById(account.Id);
           Assert.NotNull(result);
           Assert.AreEqual(account.Id, result.Id);

        }
        [Test]
        public async Task GetAccountById_ThrowNotFoundException_WhenDataInvalid()
        {
            // Expected values are in ACCOUNTS_EXPECTED.json
            var accountId = Guid.NewGuid();
            _mockAccountRepository.Setup(x => x.GetByIdAsync(accountId)).ReturnsAsync(AccountMockData.GetAccountList()[0]);

            // Actual values
            Assert.ThrowsAsync<NotFoundException>(async () => await _accountServices.GetAccountById(accountId));

        }


    }
}

