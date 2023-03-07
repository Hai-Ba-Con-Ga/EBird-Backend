using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Response;

namespace EBird.Api.Controllers
{
    [ApiController]
    [Route("generate")]
    public class GenerateDataController : ControllerBase
    {

        [HttpPost("room")]
        public ActionResult<Response<string>> GenerateRoomData()
        {
            try
            {
                //action to do  here
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string> { Data = ex.Message });
            }
        }

        [HttpPost("account")]
        public ActionResult<Response<string>> GenerateAccountData()
        {
            try
            {
                //action to do  here
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string> { Data = ex.Message });
            }
        }

        [HttpPost("bird")]
        public ActionResult<Response<string>> GenerateBirdData()
        {
            try
            {
                //action to do  here
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string> { Data = ex.Message });
            }
        }

        [HttpPost("request")]
        public ActionResult<Response<string>> GenerateRequestData()
        {
            try
            {
                //action to do  here
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string> { Data = ex.Message });
            }
        }

        [HttpPost("match")]
        public ActionResult<Response<string>> GenerateMatchData()
        {
            try
            {
                //action to do  here
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string> { Data = ex.Message });
            }
        }

        [HttpPost("payment")]
        public ActionResult<Response<string>> GeneratePaymentData()
        {
            try
            {
                //action to do  here
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string> { Data = ex.Message });
            }
        }
    }
}