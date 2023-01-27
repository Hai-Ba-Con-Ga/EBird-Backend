using EBird.Application.Model;
using EBird.Application.Services.IServices;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EBird.Api.Controllers
{
    [Route("bird")]
    [ApiController]
    public class BirdController : ControllerBase
    {
        private IBirdService _birdService;

        public BirdController(IBirdService birdService)
        {
            this._birdService = birdService;
        }


        // GET: api/<BirdController>
        [HttpGet("find-all")]
        public async Task<ActionResult<Response<List<BirdDTO>>>> Get()
        {
            var responseData = await _birdService.GetBirds();
            return StatusCode((int) responseData.StatusCode, responseData);
        }

        // GET api/<BirdController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<BirdDTO>>> Get(Guid id)
        {
            var responseData = await _birdService.GetBird(id);
            return StatusCode((int) responseData.StatusCode, responseData);
        }

        // POST api/<BirdController>
        [HttpPost]
        public async Task<ActionResult<Response<BirdDTO>>> Post([FromBody] BirdDTO birdDTO)
        {
            var responseData = await _birdService.AddBird(birdDTO);
            return StatusCode((int) responseData.StatusCode, responseData);
        }

        // PUT api/<BirdController>/5
        [HttpPatch("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BirdController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
