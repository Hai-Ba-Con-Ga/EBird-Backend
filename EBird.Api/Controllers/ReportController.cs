

using System.Net;
using EBird.Application.Exceptions;
using EBird.Application.Model.Report;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Response;

namespace EBird.Api.Controllers
{
    [Route("report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportServices _reportServices;
        public ReportController(IReportServices reportServices)
        {
            _reportServices = reportServices;
        }
        [HttpGet]
        public async Task<ActionResult<Response<List<ReportEntity>>>> GetAllReport()
        {
            var response = new Response<List<ReportEntity>>();
            try
            {
                var reports = await _reportServices.GetAllReport();
                response = Response<List<ReportEntity>>.Builder().SetData(reports).SetMessage("Success").SetStatusCode((int)HttpStatusCode.OK);
            }
            catch(NotFoundException ex){
                response = Response<List<ReportEntity>>.Builder().SetMessage(ex.Message).SetStatusCode((int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = Response<List<ReportEntity>>.Builder().SetMessage(ex.Message).SetStatusCode((int)HttpStatusCode.InternalServerError);
              
            }
            return StatusCode((int)response.StatusCode, response);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<ReportEntity>>> GetReportById(Guid id)
        {
            var response = new Response<ReportEntity>();
            try
            {
                var report = await _reportServices.GetReportById(id);
                response = Response<ReportEntity>.Builder().SetData(report).SetMessage("Success").SetStatusCode((int)HttpStatusCode.OK);
            }
            catch(NotFoundException ex){
                response = Response<ReportEntity>.Builder().SetMessage(ex.Message).SetStatusCode((int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = Response<ReportEntity>.Builder().SetMessage(ex.Message).SetStatusCode((int)HttpStatusCode.InternalServerError);
            }
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpPost]
        public async Task<ActionResult<Response<ReportEntity>>> CreateReport([FromBody] CreateReport report)
        {
            var response = new Response<ReportEntity>();
            try
            {
                await _reportServices.CreateReport(report);
                response = Response<ReportEntity>.Builder().SetMessage("Success").SetStatusCode((int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = Response<ReportEntity>.Builder().SetMessage(ex.Message).SetStatusCode((int)HttpStatusCode.InternalServerError);
            }
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Response<ReportEntity>>> UpdateReport(Guid id, [FromBody] UpdateReport updateReport)
        {
            var response = new Response<ReportEntity>();
            try
            {
                await _reportServices.UpdateReport(id, updateReport);
                response = Response<ReportEntity>.Builder().SetMessage("Success").SetStatusCode((int)HttpStatusCode.OK);
            }
            catch(NotFoundException ex){
                response = Response<ReportEntity>.Builder().SetMessage(ex.Message).SetStatusCode((int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = Response<ReportEntity>.Builder().SetMessage(ex.Message).SetStatusCode((int)HttpStatusCode.InternalServerError);
            }
            return StatusCode((int)response.StatusCode, response);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<ReportEntity>>> DeleteReport(Guid id)
        {
            var response = new Response<ReportEntity>();
            try
            {
                await _reportServices.DeleteReport(id);
                response = Response<ReportEntity>.Builder().SetMessage("Success").SetStatusCode((int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = Response<ReportEntity>.Builder().SetMessage(ex.Message).SetStatusCode((int)HttpStatusCode.InternalServerError);
            }
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
