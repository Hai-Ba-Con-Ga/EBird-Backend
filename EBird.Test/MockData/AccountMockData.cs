using EBird.Application.Model.Auth;
using EBird.Domain.Entities;
using EBird.Test.Configurations;

namespace EBird.Test.MockData;
public class AccountMockData
{
    public static List<AccountEntity> GetAccountList()
    {
        //expected values are in ACCOUNTS_EXCEPTED.json
        dynamic accounts = SeedingServices.LoadJson("ACCOUNTS_EXPECTED.json");
        var accountList = new List<AccountEntity>();
        for (int i = 0; i < accounts.Count; i++)
        {
            var account = accounts[i];
            accountList.Add(new AccountEntity()
            {
                Email = account.Email,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Username = account.Username,
                CreateDateTime = DateTime.Now,
                Description = account.Description
            });
        }
        return accountList;
    }
    public static List<AccountResponse> GetAccountResponsesList()
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
        return accountList;
    }
    

}