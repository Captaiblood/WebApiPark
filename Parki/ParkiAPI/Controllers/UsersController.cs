using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkiAPI.Models;
using ParkiAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkiAPI.Controllers
{
    [Authorize]
    // [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UsersController(IUserRepository userRepository)
        {
            this._userRepo = userRepository;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody] User userModel)
        {
            var _user = _userRepo.Authinticate(userModel.UserName, userModel.Password);
            if (_user == null)
            {
                return BadRequest(new {message = "user name or password incorrect." });
            }

            return Ok(_user);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromBody] User userModel)
        {
            bool _isUserUnique = _userRepo.IsUniqueuser(userModel.UserName);
            if (!_isUserUnique)
            {
                return BadRequest( new {message ="User already exist" });
            }


            var _user = _userRepo.Register(userModel.UserName, userModel.Password);
          
            if (_user == null)
            {
                return BadRequest(new { message = "Error while registering." });
            }

            return Ok(_user);
        }


    }
}
