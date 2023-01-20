using AutoMapper;
using EBird.Application.Model;
using EBird.Application.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace EBird.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BirdTypeController : ControllerBase
    {
        private IBirdTypeService _birdTypeService;

        public BirdTypeController(IBirdTypeService birdTypeService)
        {
            _birdTypeService = birdTypeService;
        }

        // GET: api/<BirdTypeController>
        [HttpGet]
        public async Task<ActionResult<Response<List<BirdTypeDTO>>>> Get()
        {
            var birdTypeResponeList = await _birdTypeService.GetAllBirdType();
            return Ok(birdTypeResponeList);
        }

        // GET api/<BirdTypeController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<BirdTypeDTO>>> Get(Guid id)
        {
            var birdTypeRespone = await _birdTypeService.GetBirdType(id);
           
            return Ok(birdTypeRespone);
        }

        // POST api/<BirdTypeController>
        [HttpPost]
        public async Task<ActionResult<Response<BirdTypeDTO>>> Post([FromBody] BirdTypeDTO birdTypeDTO)
        {
           var birdTypeReponse = await _birdTypeService.InsertBirdType(birdTypeDTO);
            return Ok(birdTypeReponse);
        }

        // PUT api/<BirdTypeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            
        }

        // DELETE api/<BirdTypeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
