using EBird.Application.Model.Auth;
using EBird.Domain.Entities;
using Response;

namespace EBird.Application.Services.IServices
{
    public interface IAccountServices
    {
        Task<AccountResponse> GetAccountById(Guid id);
        Task<List<AccountResponse>> GetAllAccount();
        Task UpdateAccount(AccountEntity updateAccount);
        Task DeleteAccount(Guid id);
        Task ChangePassword(Guid id, ChangePasswordModel model);
        Task ForgotPassword(string username);
        Task ResetPassword(ResetPasswordModel model);
        Task CheckEmail(string email);
        Task ChangeRoleAdmin(Guid id);
    }
}
