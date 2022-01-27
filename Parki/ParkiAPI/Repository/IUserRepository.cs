using ParkiAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkiAPI.Repository
{
    public interface IUserRepository
    {
        bool IsUniqueuser(string UserName);

        User Authinticate(string Username, string Password);

        User Register(string UserName, string Password);
    }
}
