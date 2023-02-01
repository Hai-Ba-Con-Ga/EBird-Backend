using EBird.Domain.Common;

namespace EBird.Domain.Entities
{
    public class AccountResourceEntity : BaseEntity
    {
        public Guid AccountId { get; set; }
        //1-M relationship AccountEnity
        public AccountEntity Account { get; set; } = null!;
        public Guid ResourceId { get; set; }
        //1-M relationship ResourceEntity
        public ResourceEntity Resource { get; set; } = null!;

    }
}