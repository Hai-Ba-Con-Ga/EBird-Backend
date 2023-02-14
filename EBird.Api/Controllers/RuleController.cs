
using AutoMapper;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Response;
using EBird.Application.Exceptions;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using EBird.Domain.Enums;
using System.Security.Claims;
using EBird.Application.Model.Rule;

namespace EBird.Api.Controllers;
[Route("rule")]
[ApiController]
public class RuleController : ControllerBase
{
    private readonly IRuleService _ruleService;
    private readonly IMapper _mapper;
    public RuleController(IRuleService ruleService, IMapper mapper)
    {
        _ruleService = ruleService;
        _mapper = mapper;
    }
    [HttpGet("{ruleID}")]
    public async Task<ActionResult<Response<RuleEntity>>> GetRule(Guid ruleID)
    {
        var response = new Response<RuleEntity>();
        try
        {
            var rule = await _ruleService.GetRule(ruleID);
            response = Response<RuleEntity>.Builder()
                        .SetSuccess(true)
                        .SetStatusCode((int)HttpStatusCode.OK)
                        .SetMessage("Rule is retrieved successfully")
                        .SetData(rule);
        }
        catch (NotFoundException ex)
        {
            response = Response<RuleEntity>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.BadRequest)
                        .SetMessage(ex.Message);
        }
        catch (Exception ex)
        {
            response = Response<RuleEntity>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.InternalServerError)
                        .SetMessage(ex.Message);
        }
        return response;
    }
    [HttpGet("all")]
    public async Task<ActionResult<Response<List<RuleEntity>>>> GetRules()
    {
        var response = new Response<List<RuleEntity>>();
        try
        {
            var rules = await _ruleService.GetRules();
            response = Response<List<RuleEntity>>.Builder()
                        .SetSuccess(true)
                        .SetStatusCode((int)HttpStatusCode.OK)
                        .SetMessage("Rules are retrieved successfully")
                        .SetData(rules);
        }
        catch (NotFoundException ex)
        {
            response = Response<List<RuleEntity>>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.BadRequest)
                        .SetMessage(ex.Message);
        }
        catch (Exception ex)
        {
            response = Response<List<RuleEntity>>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.InternalServerError)
                        .SetMessage(ex.Message);
        }
        return response;
    }
    [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(Role.Admin))]
    [HttpPost]
    public async Task<Response<string>> CreateRule(CreateRuleRequest request)
    {
        var response = new Response<string>();
        string rawId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (rawId == null)
        {
            response = Response<string>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.BadRequest)
                        .SetMessage("Account not found");
        }
        try
        {
            Guid id = Guid.Parse(rawId);
            await _ruleService.CreateRule(id, request);
            response = Response<string>.Builder()
                        .SetSuccess(true)
                        .SetStatusCode((int)HttpStatusCode.OK)
                        .SetMessage("Rule is created successfully");
        }
        catch (Exception ex)
        {
            response = Response<string>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.InternalServerError)
                        .SetMessage(ex.Message);
        }
        return response;

    }
    [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(Role.Admin))]
    [HttpPut("{ruleID}")]
    public async Task<ActionResult<Response<string>>> UpdateRule(Guid ruleID, UpdateRuleRequest updateRule)
    {
        var response = new Response<string>();
        try
        {
            await _ruleService.UpdateRule(ruleID, updateRule);
            response = Response<string>.Builder()
                        .SetSuccess(true)
                        .SetStatusCode((int)HttpStatusCode.OK)
                        .SetMessage("Rule is updated successfully");
        }
        catch (NotFoundException ex)
        {
            response = Response<string>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.BadRequest)
                        .SetMessage(ex.Message);
        }
        catch (Exception ex)
        {
            response = Response<string>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.InternalServerError)
                        .SetMessage(ex.Message);
        }
        return response;
    }
    [HttpDelete("{ruleID}")]
    public async Task<ActionResult<Response<string>>> DeleteRule(Guid ruleID)
    {
        var response = new Response<string>();
        try
        {
            await _ruleService.DeleteRule(ruleID);
            response = Response<string>.Builder()
                        .SetSuccess(true)
                        .SetStatusCode((int)HttpStatusCode.OK)
                        .SetMessage("Rule is deleted successfully");
        }
        catch (NotFoundException ex)
        {
            response = Response<string>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.BadRequest)
                        .SetMessage(ex.Message);
        }
        catch (Exception ex)
        {
            response = Response<string>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.InternalServerError)
                        .SetMessage(ex.Message);
        }
        return response;
    }

}