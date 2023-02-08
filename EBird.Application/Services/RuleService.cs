using System.Net;
using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model.Rule;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using EBird.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace EBird.Application.Services
{
    public class RuleService : IRuleService
    {
        private readonly IGenericRepository<RuleEntity> _ruleRepository;
        private readonly IMapper _mapper;
        public RuleService(IGenericRepository<RuleEntity> ruleRepository, IMapper mapper)
        {
            _ruleRepository = ruleRepository;
            _mapper = mapper;
        }
        

        public async Task CreateRule(Guid createById, CreateRuleRequest createRule)
        {
            var newRule = new RuleEntity(){
                Title = createRule.Title,
                Content = createRule.Content,
                CreateById = createById,
            };
            await _ruleRepository.CreateAsync(newRule);
            
        }

        public async Task<RuleEntity> GetRule(Guid ruleID)
        {
            var rule = await _ruleRepository.GetByIdActiveAsync(ruleID);
            if (rule == null)
            {
                throw new NotFoundException("Rule is not exist");
            }
            return rule;
        }
        public async Task<List<RuleEntity>> GetRules()
        {
            var rules = await _ruleRepository.GetAllActiveAsync();
            if (rules == null)
            {
                throw new NotFoundException("Rule is not exist");
            }
            return rules;
        }
        
        public async Task UpdateRule(Guid ruleID, UpdateRuleRequest updateRule)
        {
            var rule = await _ruleRepository.GetByIdActiveAsync(ruleID);
            if (rule == null)
            {
                throw new NotFoundException("Rule is not exist");
            }
            
            await _ruleRepository.UpdateAsync(_mapper.Map<UpdateRuleRequest, RuleEntity>(updateRule, rule));
        }
        public async Task DeleteRule(Guid ruleID)
        {
            var rule = await _ruleRepository.DeleteSoftAsync(ruleID);
            if (rule == null)
            {
                throw new NotFoundException("Rule is not exist");
            }
        }
    }
}