using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkiAPI.Dtos;
using ParkiAPI.Models;
using ParkiAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkiAPI.Controllers
{
    [Route("api/v{version:apiVersion}/trails")]
    //[Route("api/v{version:apiVersion}/Trails")]
    //[Route("api/Trails")]
    [ApiController]
    public class TrailController : ControllerBase
    {
        private readonly ITrailRepository _trailRepo;
        private readonly IMapper _mapper;

        public TrailController(ITrailRepository TrailRepository, IMapper mapper)
        {
            _trailRepo = TrailRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get List of  Trails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TrailDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<TrailDto> Get()
        {

            var TrailList = _trailRepo.GetTrails();

            var nPDto = new List<TrailDto>();

            //foreach (var item in TrailList)
            //{
            //    nPDto.Add(_mapper.Map<TrailDto>(item));
            //}
            nPDto.AddRange(from item in TrailList
                           select _mapper.Map<TrailDto>(item));

            return Ok(nPDto);
        }

        /// <summary>
        /// Get individual  Trail
        /// </summary>
        /// <param name="TrailId"></param> Id of  Trail
        /// <returns></returns>
        [HttpGet("{TrailId:int}", Name ="GetTrail")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesDefaultResponseType]
        public ActionResult<TrailDto> Get(int TrailId)
        {
            var Trail = _trailRepo.GetTrail(TrailId);

            if (Trail == null)
            {
                return NotFound();
            }
                                             
            var  nPDto =_mapper.Map<TrailDto>(Trail);           

            return Ok(nPDto);
        }

        //Example when you want two similar Endpoint in your
        //Api fo example two Get which accepts and ID
        //we use atribute routing
        [HttpGet("[action]/{nationalParkId:int}", Name = "GetTrailsByNationalPark")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesDefaultResponseType]
        public ActionResult<TrailDto> GetTrailsByNationalPark(int nationalParkId)
        {
            var _trailList = _trailRepo.GetTrailsInNationalPark(nationalParkId);



            if (_trailList == null)
            {
                return NotFound();
            }

            var _trailListDTO = new List<TrailDto>();

            foreach( var item in _trailList)
            {
                _trailListDTO.Add(_mapper.Map<TrailDto>(item));
            }

            //var nPDto = _mapper.Map<TrailDto>(Trail);

            return Ok(_trailListDTO);
        }

        /// <summary>
        /// Create a new  Trail.
        /// </summary>
        /// <param name="TrailDto"></param>  Trail json
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail([FromBody] TrailCreateDto TrailDto)
        {

            if (TrailDto == null)
            {

                return BadRequest(ModelState);

            }

            if (_trailRepo.TrailExist(TrailDto.Name))
            {
                ModelState.AddModelError("", " Trail Exist");

                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var _trail = _mapper.Map<Trail>(TrailDto);

            if(!_trailRepo.CreateTrail(_trail)){
                ModelState.AddModelError("", $"Some thing went wrong when adding Trail{TrailDto}");
                return StatusCode(500,ModelState);
            }

            return CreatedAtRoute("GetTrail", new { NationapTrailID = _trail.Id }, _trail);


        }

        /// <summary>
        /// Update  Trail
        /// </summary>
        /// <param name="TrailId"></param>  Trail Id
        /// <param name="TrailDto"></param>  Trail json
        /// <returns></returns>
        [HttpPatch ("{TrailId:int}", Name ="UpdateTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int TrailId, [FromBody] TrailUpdateDto TrailDto)
        {
            if (TrailDto == null || TrailDto.Id != TrailId )
            {

                return BadRequest(ModelState);

            }

            var _trail = _mapper.Map<Trail>(TrailDto);

            if (!_trailRepo.UpdateTrail(_trail))
            {
                ModelState.AddModelError("", $"Some thing went wrong when updating Trail record {TrailDto}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        ///// <summary>
        ///// Delete  Trail
        ///// </summary>
        ///// <param name="TrailId"></param>  Trail Id
        ///// <param name="TrailDto"></param>   Trail json
        ///// <returns></returns>
        //[HttpDelete("{TrailId:int}", Name = "DeleteTrail")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        //[ProducesResponseType(StatusCodes.Status409Conflict)] //Can not be removed 
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public IActionResult DeleteTrail(int TrailId, [FromBody] TrailDto TrailDto)
        //{
        //    if (TrailDto == null || TrailDto.Id != TrailId)
        //    {

        //        return BadRequest(ModelState);

        //    }

        //    var _trail = _mapper.Map<Trail>(TrailDto);

        //    if (!_trailRepo.DeleteTrail(_trail))
        //    {
        //        ModelState.AddModelError("", $"Some thing went wrong when updating Trail record {TrailDto}");
        //        return StatusCode(500, ModelState);
        //    }

        //    return NoContent();

        //}


        /// <summary>
        /// Delete  Trail
        /// </summary>
        /// <param name="TrailId"></param>  Trail Id      
        /// <returns></returns>
        [HttpDelete("{TrailId:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status409Conflict)] //Can not be removed 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int TrailId)
        {
            if (!_trailRepo.TrailExist(TrailId))
            {
                return NotFound();
            }

            var _exisitngTrail = _trailRepo.GetTrail(TrailId);
            if (!_trailRepo.DeleteTrail(_exisitngTrail))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {_exisitngTrail.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }


    }
}
