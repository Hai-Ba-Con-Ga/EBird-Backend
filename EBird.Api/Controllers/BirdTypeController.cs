using AutoMapper;
using EBird.Application.Model;
using EBird.Application.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace EBird.Api.Controllers
{
    [Route("bird-type")]
    [ApiController]
    public class BirdTypeController : ControllerBase
    {
        private IBirdTypeService _birdTypeService;

        public BirdTypeController(IBirdTypeService birdTypeService)
        {
            _birdTypeService = birdTypeService;
        }

        // GET all
        [HttpGet("find-all")]
        public async Task<ActionResult<Response<List<BirdTypeDTO>>>> Get()
        {
            var birdTypeResponeList = await _birdTypeService.GetAllBirdType();
            return StatusCode((int) birdTypeResponeList.StatusCode, birdTypeResponeList);
        }

        // GET within id
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<BirdTypeDTO>>> Get(Guid id)
        {
            var birdTypeRespone = await _birdTypeService.GetBirdType(id);

            return StatusCode((int)birdTypeRespone.StatusCode, birdTypeRespone);
        }

        // POST : create new bird type
        [HttpPost("create")]
        public async Task<ActionResult<Response<BirdTypeDTO>>> Post([FromBody] BirdTypeDTO birdTypeDTO)
        {
            var birdTypeReponse = await _birdTypeService.AddBirdType(birdTypeDTO);
            return StatusCode((int)birdTypeReponse.StatusCode, birdTypeReponse);
        }

        // PUT : update exist bird type
        [HttpPatch("update/{id}")]
        public async Task<ActionResult<Response<BirdTypeDTO>>> Put(Guid id, [FromBody] BirdTypeDTO birdTypeDTO)
        {
            var birdTypeReponse = await _birdTypeService.UpdateBirdType(id, birdTypeDTO);
            return StatusCode((int)birdTypeReponse.StatusCode, birdTypeReponse);
        }

        // DELETE: delete bird type
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Response<BirdTypeDTO>>> Delete(Guid id)
        {
            var birdTypeReponse = await _birdTypeService.DeleteBirdType(id);
            return StatusCode((int) birdTypeReponse.StatusCode, birdTypeReponse);
        }
    }
}
