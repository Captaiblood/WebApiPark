using ParkiAPI.Data;
using ParkiAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkiAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly AppSettings _appSettings;

        public UserRepository(ApplicationDbContext applicationDbContext, Microsoft.Extensions.Options.IOptions<AppSettings> appSettings)
        {
            _db = applicationDbContext;
            _appSettings = appSettings.Value;
        }
        public User Authinticate(string Username, string Password)
        {
           var _user =_db.Users.SingleOrDefault(x => x.UserName.ToLower() == Username.ToLower() && x.Password == Password);
            if (_user == null)
            {
                return null;
            }
            else
            {
                //generate KWT token
                var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler(); // set JWT Token Handler
                var _secretKey = Encoding.ASCII.GetBytes(_appSettings.Secret); //Get secret key
                var tokenDiscriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
                {
                    //add new claims
                    Subject = new System.Security.Claims.ClaimsIdentity
                    (new System.Security.Claims.Claim[]
                    {
                        new System.Security.Claims.Claim(
                            System.Security.Claims.ClaimTypes.Name,
                            _user.Id.ToString()
                            ),
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials( 
                        new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(_secretKey), 
                        Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature
                        )
                };
                //now that we have created token Discriptor above 
                //lets create token from token Discriptor
                var token = tokenHandler.CreateToken(tokenDiscriptor);
                //now let us save that token in our Model
                _user.Token = tokenHandler.WriteToken(token);
                // we dont want to send password back in return model
                _user.Password = "";
                return _user;

            }
        }

        public bool IsUniqueuser(string UserName)
        {
            var _user = _db.Users.SingleOrDefault(x => x.UserName.ToLower() == UserName.ToLower() );
            if (_user == null)
            {
                return true;
            }
            return false;
        }

        public User Register(string UserName, string Password)
        {
            User _userEntity = new User()
            {
                UserName = UserName,
                Password=Password
            };

            _db.Users.Add(_userEntity);
            _db.SaveChanges();
            _userEntity.Password = "";
            return _userEntity;
        }
    }
}
