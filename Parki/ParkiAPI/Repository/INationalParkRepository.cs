using ParkiAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkiAPI.Repository
{
   public  interface INationalParkRepository
    {

        ICollection<NationalPark> GetNationalParks();

        NationalPark GetNationalPark(int Id);

        bool NationalParkExist(string Name);

        bool NationalParkExist(int Id);

        bool CreateNationalPark(NationalPark nationalPark);

        bool UpdateNationalPark(NationalPark nationalPark);

        bool DeleteNationalPark(NationalPark nationalPark);

        bool Save();
    }
}
