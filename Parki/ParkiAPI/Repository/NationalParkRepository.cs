using ParkiAPI.Data;
using ParkiAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkiAPI.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext _DbContext;

        public NationalParkRepository(ApplicationDbContext applicationDbContext)
        {
            _DbContext = applicationDbContext;
        }
        public bool CreateNationalPark(NationalPark nationalPark)
        {
            _DbContext.NationalParks.Add(nationalPark);
            return Save();
        }

        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            _DbContext.Remove(nationalPark);
            return Save();
           
        }

        public NationalPark GetNationalPark(int Id)
        {
            var _nationalPark = _DbContext.NationalParks.SingleOrDefault(N => N.Id==Id);

            return _nationalPark;
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            var _nationalParkList = _DbContext.NationalParks.OrderBy(a=> a.Name ).ToList();



            //var _nationalParkList2 = _DbContext.NationalParks.OrderBy(on => on.Name)
            //        .Skip((ownerParameters.PageNumber - 1) * ownerParameters.PageSize)
            //        .Take(ownerParameters.PageSize)
            //        .ToList();
          

            return _nationalParkList;
        }

        public bool NationalParkExist(string Name)
        {
            var _nationalPark = _DbContext.NationalParks.Any(N => N.Name.ToLower().Trim() == Name.ToLower().Trim());
            
            return _nationalPark;
        }

        public bool NationalParkExist(int Id)
        {
            var _nationalPark = _DbContext.NationalParks.Any(N => N.Id == Id);

            return _nationalPark;
        }

        public bool Save()
        {
            //return _DbContext.SaveChanges() >= 0 ? true:false;
            return _DbContext.SaveChanges() >= 0;
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            
             _DbContext.NationalParks.Update(nationalPark);
            
            return Save();         

        }
    }
}
