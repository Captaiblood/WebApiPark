using ParkiAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkiAPI.Repository
{
   public  interface ITrailRepository
    {

        ICollection<Trail> GetTrails();
        ICollection<Trail> GetTrailsInNationalPark(int NationalParkID);
        Trail GetTrail(int Id);

        bool TrailExist(string Name);

        bool TrailExist(int Id);

        bool CreateTrail(Trail trail);

        bool UpdateTrail(Trail trail);

        bool DeleteTrail(Trail trail);

        bool Save();
    }
}
