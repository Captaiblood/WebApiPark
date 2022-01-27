using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkiAPI.Dtos;
using ParkiAPI.Models;
using ParkiAPI.Repository;
using Service.Library.ServiceResponder;
//using Service.Library.FileUpload.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ParkiAPI.Controllers
{
    //[Route("api/v{version:apiVersion}/nationalparks")]
    [Route("api/v{version:apiVersion}/NationalPark")]
    //[Route("api/[controller]")]
    [ApiController]
    public class NationalParkController : ControllerBase
    {
        private readonly INationalParkRepository _nP;
        private readonly IMapper _mapper;
        //private readonly IFileStorageService fileStorageService;
        //private readonly string containerName = "Parks";
        public NationalParkController(INationalParkRepository nPRepository, IMapper mapper)
        {
            _nP = nPRepository;
            _mapper = mapper;
         
        }

        /// <summary>
        /// Get List of National Parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<NationalParkDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<NationalParkDto> Get()
        {
           ServiceResponse<List<NationalParkDto>> serviceResponce = new();

            var ParkList = _nP.GetNationalParks();

            var nPDto = new List<NationalParkDto>();

            //foreach (var item in ParkList)
            //{
            //    nPDto.Add(_mapper.Map<NationalParkDto>(item));
            //}
            nPDto.AddRange(from item in ParkList
                           select _mapper.Map<NationalParkDto>(item));

            if (nPDto.Count > 0)
            {
                serviceResponce.Data = nPDto;
                serviceResponce.Success = true;
                serviceResponce.Message = "ok";
            }                     
            return Ok(nPDto);
        }

        [HttpGet("[action]/", Name = "GetWithWarpper")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<NationalParkDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<NationalParkDto> GetWithWarpper()
        {
            ServiceResponse<List<NationalParkDto>> serviceResponce = new();

            var ParkList = _nP.GetNationalParks();

            var nPDto = new List<NationalParkDto>();

            //foreach (var item in ParkList)
            //{
            //    nPDto.Add(_mapper.Map<NationalParkDto>(item));
            //}
            nPDto.AddRange(from item in ParkList
                           select _mapper.Map<NationalParkDto>(item));

            if (nPDto.Count > 0)
            {
                serviceResponce.Data = nPDto;
                serviceResponce.Success = true;
                serviceResponce.Message = "ok";
            }
            return Ok(serviceResponce);
        }

        /// <summary>
        /// Get individual national park
        /// </summary>
        /// <param name="NationalParkId"></param> Id of national park
        /// <returns></returns>       
        [HttpGet("{NationalParkId:int}", Name ="GetNationalPark")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [Authorize]
        [ProducesDefaultResponseType]
        public ActionResult<NationalParkDto> Get(int NationalParkId)
        {
            //UserId Issued by Identity server.
            //(Example how UUID isuued by Identity Server which is encrypted inside JWT token is
            //passed across ecosystem to maintain data intigrity and user transactions)
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var email = User.FindFirst("sub")?.Value;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId2 = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            

            var Park = _nP.GetNationalPark(NationalParkId);

            if (Park == null)
            {
                return NotFound();
            }
                                             
            var  nPDto =_mapper.Map<NationalParkDto>(Park);           

            return Ok(nPDto);
        }

        /// <summary>
        /// Create a new national park.
        /// </summary>
        /// <param name="nationalParkDto"></param> National park json
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreatenationalPark([FromBody] NationalParkDto nationalParkDto)
        {

            if (nationalParkDto == null)
            {

                return BadRequest(ModelState);

            }

            if (_nP.NationalParkExist(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park Exist");

                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var _nationalPark = _mapper.Map<NationalPark>(nationalParkDto);

            if(!_nP.CreateNationalPark(_nationalPark)){
                ModelState.AddModelError("", $"Some thing went wrong when adding park{nationalParkDto}");
                return StatusCode(500,ModelState);
            }

            //if (nationalParkDto.ParkPicture != null)
            //{
            //    _nationalPark.Picture =  await fileStorageService.SaveFile(containerName, nationalParkDto.Picture);
            //}


            return CreatedAtRoute("GetNationalPark", new { nationalParkId = _nationalPark.Id }, _nationalPark);           

        }

        /// <summary>
        /// Update national park
        /// </summary>
        /// <param name="NationalParkId"></param> National Park Id
        /// <param name="nationalParkDto"></param> National park json
        /// <returns></returns>
        [HttpPatch ("{NationalParkId:int}", Name ="UpdateNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateNationalPark(int NationalParkId, [FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || nationalParkDto.Id != NationalParkId )
            {

                return BadRequest(ModelState);

            }

            var _nationaPark = _mapper.Map<NationalPark>(nationalParkDto);

            if (!_nP.UpdateNationalPark(_nationaPark))
            {
                ModelState.AddModelError("", $"Some thing went wrong when updating park record {nationalParkDto}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        ///// <summary>
        ///// Delete National Park
        ///// </summary>
        ///// <param name="NationalParkId"></param> National park Id
        ///// <param name="nationalParkDto"></param>  National park json
        ///// <returns></returns>
        //[HttpDelete("{NationalParkId:int}", Name = "DeleteNationalPark")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        //[ProducesResponseType(StatusCodes.Status409Conflict)] //Can not be removed 
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public IActionResult DeleteNationalPark(int NationalParkId, [FromBody] NationalParkDto nationalParkDto)
        //{
        //    if (nationalParkDto == null || nationalParkDto.Id != NationalParkId)
        //    {

        //        return BadRequest(ModelState);

        //    }

        //    var _nationaPark = _mapper.Map<NationalPark>(nationalParkDto);

        //    if (!_nP.UpdateNationalPark(_nationaPark))
        //    {
        //        ModelState.AddModelError("", $"Some thing went wrong when updating park record {nationalParkDto}");
        //        return StatusCode(500, ModelState);
        //    }

        //    return NoContent();

        //}


        /// <summary>
        /// Delete National Park
        /// </summary>
        /// <param name="nationalParkId"></param>
        /// <returns></returns>
        [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_nP.NationalParkExist(nationalParkId))
            {
                return NotFound();
            }

            var nationalParkObj = _nP.GetNationalPark(nationalParkId);
            if (!_nP.DeleteNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }


    }
}
