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
    [Route("api/v{version:apiVersion}/nationalparksv2")]
    [ApiVersion("2.0")]
    //[Route("api/NationalParkV2")]
    [ApiController]
    public class NationalParkV2Controller : ControllerBase
    {
        private readonly INationalParkRepository _nP;
        private readonly IMapper _mapper;

        public NationalParkV2Controller(INationalParkRepository nPRepository, IMapper mapper)
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

            var ParkList = _nP.GetNationalParks();

            var nPDto = new List<NationalParkDto>();

            //foreach (var item in ParkList)
            //{
            //    nPDto.Add(_mapper.Map<NationalParkDto>(item));
            //}
            nPDto.AddRange(from item in ParkList
                           select _mapper.Map<NationalParkDto>(item));

            return Ok(nPDto);
        }

      


    }
}
