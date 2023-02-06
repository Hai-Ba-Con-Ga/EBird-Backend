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

        // GET all
        [HttpGet("all")]
        public async Task<ActionResult<Response<List<NotificationTypeDTO>>>> Get()
        {
            Response<List<NotificationTypeDTO>> response;
            try
            {
                var notificationTypeResponeList = await _notificationTypeService.GetNotificationTypes();

                response = new Response<List<NotificationTypeDTO>>()
                            .SetData(notificationTypeResponeList)
                            .SetStatusCode((int)HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Get notification type by id is successful");

                return StatusCode((int)response.StatusCode, response);
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

                return StatusCode((int)response.StatusCode, response);
            }

        }

        // GET by id
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

                return StatusCode((int)response.StatusCode, response);
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

                return StatusCode((int)response.StatusCode, response);
            }
        }

        // POST  create new notification type
        [HttpPost]
        public async Task<ActionResult<Response<NotificationTypeRequestDTO>>> Post([FromBody] NotificationTypeRequestDTO notificationTypeRequestDTO)
        {
            Response<NotificationTypeRequestDTO> response = null;
            try
            {
                var responseData = await _notificationTypeService.AddNotificationType(notificationTypeRequestDTO);

                response = new Response<NotificationTypeRequestDTO>()
                            .SetData(responseData)
                            .SetStatusCode((int)HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Create notification type is successful");

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<NotificationTypeRequestDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode(((BaseHttpException)ex).StatusCode)
                        .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<NotificationTypeRequestDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        // Put : update exist notification type
        [HttpPut("{id}")]
        public async Task<ActionResult<Response<NotificationTypeRequestDTO>>> Put(Guid id, [FromBody] NotificationTypeRequestDTO notificationTypeRequestDTO)
        {
            Response<NotificationTypeRequestDTO> response = null;
            try
            {
                var responseData = await _notificationTypeService.UpdateNotificationType(id, notificationTypeRequestDTO);

                response = new Response<NotificationTypeRequestDTO>()
                            .SetData(responseData)
                            .SetStatusCode((int)HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Update notification type is successful");

                return response;
            }

            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<NotificationTypeRequestDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode(((BaseHttpException)ex).StatusCode)
                        .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<NotificationTypeRequestDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        // DELETE: delete notification type
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<NotificationTypeDTO>>> Delete(Guid id)
        {
            Response<NotificationTypeDTO> response = null;
            try
            {
                var notificationTypeReponse = await _notificationTypeService.DeleteNotificationType(id);

                response = new Response<NotificationTypeDTO>()
                            .SetData(notificationTypeReponse)
                            .SetStatusCode((int)HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Delete notification type is successful");

                return StatusCode((int)response.StatusCode, response);
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

                return StatusCode((int)response.StatusCode, response);
            }
        }
    }
}
