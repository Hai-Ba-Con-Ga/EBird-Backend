using EBird.Application.Model.Rule;
using EBird.Domain.Entities;

namespace EBird.Application.Services.IServices;
public interface IRuleService{
    Task CreateRule(Guid createById, CreateRuleRequest createRule);
    Task<RuleEntity> GetRule(Guid ruleID);
    Task<List<RuleEntity>> GetRules();
    Task UpdateRule(Guid ruleID, UpdateRuleRequest updateRule);
    Task DeleteRule(Guid ruleID);
    
}