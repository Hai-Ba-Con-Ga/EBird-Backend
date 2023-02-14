using EBird.Application.Exceptions;
using EBird.Application.Model.NotificationType;
using EBird.Application.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Response;
using System.Net;

namespace EBird.Api.Controllers
{
    [Route("notification-type")]
    [ApiController]
    public class NotificationTypeController : ControllerBase
    {
        private INotificationTypeService _notificationTypeService;

        public NotificationTypeController(INotificationTypeService notificationTypeService)
        {
            _notificationTypeService = notificationTypeService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<Response<List<NotificationTypeDTO>>>> Get()
        {
            Response<List<NotificationTypeDTO>> response;
            try
            {
                var responseData = await _notificationTypeService.GetNotificationTypes();
                response = new Response<List<NotificationTypeDTO>>()
                            .SetData(responseData)
                            .SetStatusCode((int)HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Get notification type is successful");
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<List<NotificationTypeDTO>>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode(((BaseHttpException)ex).StatusCode)
                        .SetMessage(ex.Message);
                    return StatusCode((int)response.StatusCode, response);
                }
                response = Response<List<NotificationTypeDTO>>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");
            }
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<NotificationTypeDTO>>> Get(Guid id)
        {
            Response<NotificationTypeDTO> response;
            try
            {
                var responseData = await _notificationTypeService.GetNotificationType(id);
                response = new Response<NotificationTypeDTO>()
                            .SetData(responseData)
                            .SetStatusCode((int)HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Get notification type is successful");
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<NotificationTypeDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode(((BaseHttpException)ex).StatusCode)
                        .SetMessage(ex.Message);
                    return StatusCode((int)response.StatusCode, response);
                }
                response = Response<NotificationTypeDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");
            }
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost]
        public async Task<ActionResult<Response<string>>> Post([FromBody] NotificationTypeRequestDTO notificationTypeRequestDTO)
        {
            Response<string> response = null;
            try
            {
                await _notificationTypeService.AddNotificationType(notificationTypeRequestDTO);

                response = new Response<string>()
                            .SetData("")
                            .SetStatusCode((int)HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Create notification type is successful");
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<string>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode(((BaseHttpException)ex).StatusCode)
                        .SetMessage(ex.Message);
                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<string>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");
            }
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response<string>>> Put(Guid id, [FromBody] NotificationTypeRequestDTO ntDTO)
        {
            Response<string> response = null;
            try
            {
                await _notificationTypeService.UpdateNotificationType(id, ntDTO);
                response = new Response<string>()
                            .SetData("")
                            .SetStatusCode((int)HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Update notification type is successful");
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<string>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode(((BaseHttpException)ex).StatusCode)
                        .SetMessage(ex.Message);
                    return StatusCode((int)response.StatusCode, response);
                }
                response = Response<string>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");
            }
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> Delete(Guid id)
        {
            Response<string> response = null;
            try
            {
                await _notificationTypeService.DeleteNotificationType(id);
                response = new Response<string>()
                            .SetData("")
                            .SetStatusCode((int)HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Delete notification type is successful");
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<string>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode(((BaseHttpException)ex).StatusCode)
                        .SetMessage(ex.Message);
                    return StatusCode((int)response.StatusCode, response);
                }
                response = Response<string>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");
            }
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
