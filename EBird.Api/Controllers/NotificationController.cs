using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Model.Notification;
using EBird.Application.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Response;
using System.Net;


namespace EBird.Api.Controllers
{
    [Route("/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private INotificationService _notificationService;
        private IMapper _mapper;

        public NotificationController(INotificationService notificationService, IMapper mapper)
        {
            this._notificationService = notificationService;
            this._mapper = mapper;
        }

        // GET all
        [HttpGet("all")]
        public async Task<ActionResult<Response<List<NotificationDTO>>>> Get()
        {
            Response<List<NotificationDTO>> response = null;
            try
            {
                var responseData = await _notificationService.GetNotifications();

                response = Response<List<NotificationDTO>>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int) HttpStatusCode.OK)
                    .SetMessage("Get Notifications is success")
                    .SetData(responseData);

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<List<NotificationDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.BadRequest)
                            .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }

                response = Response<List<NotificationDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int) response.StatusCode, response);
            }
        }

        // GET by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<NotificationDTO>>> Get(Guid id)
        {
            Response<NotificationDTO> response = null;
            try
            {
                var responseData = await _notificationService.GetNotification(id);

                response = Response<NotificationDTO>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int) HttpStatusCode.OK)
                    .SetMessage("Get Notification is success")
                    .SetData(responseData);

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<NotificationDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.BadRequest)
                            .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }

                response = Response<NotificationDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int) response.StatusCode, response);
            }
        }

        // POST create
        [HttpPost]
        public async Task<ActionResult<Response<Guid>>> Post([FromBody] NotificationCreateDTO notificationCreateDTO)
        {
            Response<Guid> response = null;
            try
            {
                Guid id = await _notificationService.AddNotification(notificationCreateDTO);

                response = Response<Guid>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Create Notification is success")
                    .SetData(id);

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<Guid>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.BadRequest)
                            .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }

                response = Response<Guid>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int) response.StatusCode, response);
            }
        }

        // PUT update 
        [HttpPut("{id}")]
        public async Task<ActionResult<Response<NotificationUpdateDTO>>> Put(Guid id, [FromBody] NotificationUpdateDTO notificationUpdateDTO)
        {
            Response<NotificationUpdateDTO> response = null;
            try
            {
                var responseData = await _notificationService.UpdateNotification(id, notificationUpdateDTO);

                response = Response<NotificationUpdateDTO>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int) HttpStatusCode.OK)
                    .SetMessage("Update Notification is success")
                    .SetData(responseData);

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<NotificationUpdateDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.BadRequest)
                            .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }

                response = Response<NotificationUpdateDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int) response.StatusCode, response);
            }
        }

        // DELETE 
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<NotificationDTO>>> Delete(Guid id)
        {
            Response<NotificationDTO> response = null;
            try
            {
                var responseData = await _notificationService.DeleteNotification(id);

                response = Response<NotificationDTO>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int) HttpStatusCode.OK)
                    .SetMessage("Delete Notification is success")
                    .SetData(responseData);

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<NotificationDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.BadRequest)
                            .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }

                response = Response<NotificationDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int) response.StatusCode, response);
            }
        }
    }
}
