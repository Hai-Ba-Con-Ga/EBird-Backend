using EBird.Application.Model;
using EBird.Domain.Entities;
using Response;

namespace EBird.Application.Services.IServices
{
    public interface IAccountServices
    {
        Task<Response<AccountResponse>> GetAccountById(Guid id);
        Task<Response<List<AccountResponse>>> GetAllAccount();
        Task<Response<string>> UpdateAccount(AccountEntity updateAccount);
        Task<Response<string>> DeleteAccount(Guid id);
        Task<Response<string>> ChangePassword(Guid id, ChangePasswordModel model);
        Task<Response<string>> ForgotPassword(string username);
        Task<Response<string>> ResetPassword(ResetPasswordModel model);
        Task<Response<string>> CheckEmail(string email);
    }
}
