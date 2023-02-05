using System.Security.Cryptography;
using System.Text;
using EBird.Infrastructure.Context;
using EBird.Domain.Entities;

namespace EBird.Api.Configurations
{
    public static class DatabaseInitializer
    {
        
        public static async Task InitializeAsync(ApplicationDbContext dbContext)
        {
            await dbContext.FeedAccounts();
            await dbContext.FeedBirdTypes();

        }
        public static async Task FeedAccounts(this ApplicationDbContext context)
        {
            if (context.Accounts.Any())
            {
                return;
            }
            dynamic accounts = SeedingServices.LoadJson("ACCOUNTS_MOCK_DATA.json");
            int accountLength = accounts.Count;
            for (int i = 0; i < accountLength; i++)
            {
                var account = accounts[i];
                await context.Accounts.AddAsync(
                    new AccountEntity()
                    {
                        Email = account.Email,
                        Password = HashPassword(account.Password.ToString()),
                        FirstName = account.FirstName,
                        LastName = account.LastName,
                        Username = account.Username,
                        Role = account.Role,
                        CreateDateTime = DateTime.Now,
                        Description = account.Description
                    }
                );
            }
            await context.SaveChangesAsync();
        }
        public static async Task FeedBirdTypes(this ApplicationDbContext context)
        {
            if (context.BirdTypes.Any())
            {
                return;
            }
            dynamic birdTypes = SeedingServices.LoadJson("BIRD_TYPES_MOCK_DATA.json");
            int birdTypeLength = birdTypes.Count;
            for (int i = 0; i < birdTypeLength; i++)
            {
                var birdType = birdTypes[i];
                await context.BirdTypes.AddAsync(
                    new BirdTypeEntity()
                    {
                        TypeCode = birdType.TypeCode,
                        TypeName = birdType.TypeName,
                        CreatedDatetime = DateTime.Now
                    }
                );
            }
            await context.SaveChangesAsync();
        }



        public static string HashPassword(string password)
        {
            var sha = SHA256.Create();
            var asByteArrray = Encoding.Default.GetBytes(password);
            var hasedPassword = sha.ComputeHash(asByteArrray);
            return Convert.ToBase64String(hasedPassword);
        }

    }
}