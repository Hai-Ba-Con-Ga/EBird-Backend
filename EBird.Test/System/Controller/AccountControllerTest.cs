using System.Net;
using AutoMapper;
using EBird.Api.Controllers;
using EBird.Application.Model.Auth;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Response;

namespace EBird.Test.System.Controller;
public class AccountControllerTest
{
    private readonly Mock<IAccountServices> _mockAccountServices;
    private readonly AccountController _accountController;
    private readonly IMapper _mapper;

    public AccountControllerTest()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<AccountEntity, AccountResponse>();
        });
        _mapper = configuration.CreateMapper();
        _mockAccountServices = new Mock<IAccountServices>();

        _accountController = new AccountController(_mockAccountServices.Object, _mapper);
    }

    [Test]
    public async Task GetAllAccount_ReturnOkResponseWithData()
    {
        var expectedData = MockData.AccountMockData.GetAccountResponsesList();
        _mockAccountServices.Setup(x => x.GetAllAccount()).ReturnsAsync(expectedData);

        // Act
        var result = await _accountController.GetAllAccount();

        // Assert
        var okObjectResult = result.Result as OkObjectResult;
        Assert.NotNull(okObjectResult);
        Assert.AreEqual((int)HttpStatusCode.OK, okObjectResult.StatusCode);

        var responseData = okObjectResult.Value as Response<List<AccountResponse>>;
        Assert.NotNull(responseData);
        Assert.IsTrue(responseData.Success);
        Assert.AreEqual((int)HttpStatusCode.OK, responseData.StatusCode);
        Assert.AreEqual(expectedData, responseData.Data);
    }


}
