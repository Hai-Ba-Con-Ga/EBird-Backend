using EBird.Application.Model.Resource;
using EBird.Domain.Enums;

namespace EBird.Application.Model.Match
{
    public class UpdateMatchResultDTO
    {
        public Guid MatchBirdId { get; set; }
        public MatchDetailResult Result { get; set; }
        public List<ResourceCreateDTO>? ListResource {get; set; }
    }
    
}