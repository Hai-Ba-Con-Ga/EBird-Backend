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


        // GET: get all
        [HttpGet("find-all")]
        public async Task<ActionResult<Response<List<BirdDTO>>>> Get()
        {
            var responseData = await _birdService.GetBirds();
            return StatusCode((int) responseData.StatusCode, responseData);
        }

        // GET : get bird by id 
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<BirdDTO>>> Get(Guid id)
        {
            var responseData = await _birdService.GetBird(id);
            return StatusCode((int) responseData.StatusCode, responseData);
        }

        // POST : create bird
        [HttpPost("create")]
        public async Task<ActionResult<Response<BirdDTO>>> Post([FromBody] BirdDTO birdDTO)
        {
            var responseData = await _birdService.AddBird(birdDTO);
            return StatusCode((int) responseData.StatusCode, responseData);
        }

        // PUT : update bird
        [HttpPatch("update/{id}")]
        public async Task<ActionResult<Response<BirdDTO>>> Patch(Guid id, [FromBody] BirdDTO birdDTO)
        {
            var responseData = await _birdService.UpdateBird(id, birdDTO);
            return StatusCode((int) responseData.StatusCode, responseData);
        }
        
        // DELETE : delete bird
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Response<BirdDTO>>> Delete(Guid id)
        {
            var responseData = await _birdService.DeleteBird(id);
            return StatusCode((int) responseData.StatusCode, responseData);
        }
    }
}
