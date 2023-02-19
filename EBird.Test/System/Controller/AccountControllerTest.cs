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
    public async Task GetAllAccount_ReturnOkResponseWithData_WhenDataFound()
    {
        var expectedData = MockData.AccountMockData.GetAccountResponsesList();
        _mockAccountServices.Setup(x => x.GetAllAccount()).ReturnsAsync(expectedData);

        // Act
        var result = await _accountController.GetAllAccount();

        // Assert
        var response = result.Result as ObjectResult;
        Assert.NotNull(response);

        var responseData = response.Value as Response<List<AccountResponse>>;
        Assert.NotNull(responseData);

        Assert.True(responseData.Success);
        Assert.AreEqual((int)HttpStatusCode.OK, responseData.StatusCode);

        Assert.AreEqual(expectedData, responseData.Data);

        Assert.True(responseData.Success);
    }

    [Test]
    public async Task GetAllAccount_ReturnOkResponseWithNoData_WhenNoDataFound()
    {
        var expectedData = new List<AccountResponse>();
        _mockAccountServices.Setup(x => x.GetAllAccount()).ReturnsAsync(expectedData);

        // Act
        var result = await _accountController.GetAllAccount();

        // Assert
        var response = result.Result as ObjectResult;
        Assert.NotNull(response);

        var responseData = response.Value as Response<List<AccountResponse>>;
        Assert.NotNull(responseData);

        Assert.True(responseData.Success);
        Assert.AreEqual((int)HttpStatusCode.OK, responseData.StatusCode);

        Assert.AreEqual(expectedData, responseData.Data);

        Assert.True(responseData.Success);
    }

    [Test]
    public async Task GetAllAccount_ReturnInternalServerErrorResponse_WhenExceptionThrown()
    {
        _mockAccountServices.Setup(x => x.GetAllAccount()).ThrowsAsync(new Exception());

        // Act
        var result = await _accountController.GetAllAccount();

        // Assert
        var response = result.Result as ObjectResult;
        Assert.NotNull(response);

        var responseData = response.Value as Response<List<AccountResponse>>;
        Assert.NotNull(responseData);

        Assert.False(responseData.Success);
        Assert.AreEqual((int)HttpStatusCode.InternalServerError, responseData.StatusCode);
    }

    [Test]
    public async Task GetAccountById_ReturnOkResponseWithData_WhenDataFound()
    {
        var expectedData = MockData.AccountMockData.GetAccountResponsesList()[0];
        _mockAccountServices.Setup(x => x.GetAccountById(expectedData.Id)).ReturnsAsync(expectedData);

        // Act
        var result = await _accountController.GetAccountById(expectedData.Id);

        // Assert
        var response = result.Result as ObjectResult;
        Assert.NotNull(response);

        var responseData = response.Value as Response<AccountResponse>;
        Assert.NotNull(responseData);

        Assert.True(responseData.Success);
        Assert.AreEqual((int)HttpStatusCode.OK, responseData.StatusCode);

        Assert.AreEqual(expectedData, responseData.Data);

        Assert.True(responseData.Success);
    }

    [Test]
    public async Task GetAccountById_ReturnOkResponseWithNoData_WhenNoDataFound()
    {
        var expectedData = new AccountResponse();
        var accountId = Guid.NewGuid();
        _mockAccountServices.Setup(x => x.GetAccountById(accountId)).ReturnsAsync(expectedData);

        // Act
        var result = await _accountController.GetAccountById(accountId);

        // Assert
        var response = result.Result as ObjectResult;
        Assert.NotNull(response);

        var responseData = response.Value as Response<AccountResponse>;
        Assert.NotNull(responseData);

        Assert.True(responseData.Success);
        Assert.AreEqual((int)HttpStatusCode.OK, responseData.StatusCode);

        Assert.AreEqual(expectedData, responseData.Data);

        Assert.True(responseData.Success);
    }   


}
