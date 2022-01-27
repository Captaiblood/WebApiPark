using Microsoft.EntityFrameworkCore;
using ParkiAPI.Data;
using ParkiAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkiAPI.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _DbContext;

        public TrailRepository(ApplicationDbContext applicationDbContext)
        {
            _DbContext = applicationDbContext;
        }
        public bool CreateTrail(Trail trail)
        {
            _DbContext.Trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _DbContext.Remove(trail);
            return Save();
           
        }

        public Trail GetTrail(int Id)
        {
            //var _trail = _DbContext.Trails.SingleOrDefault(N => N.Id==Id);
            var _trail = _DbContext.Trails.Include(c => c.NationalPark).FirstOrDefault(a => a.Id == Id);

            return _trail;
        }

        public ICollection<Trail> GetTrails()
        {
            // var _trailList = _DbContext.Trails.OrderBy(a=> a.Name ).ToList();
            var _trailList =  _DbContext.Trails.Include(c => c.NationalPark).OrderBy(a => a.Name).ToList();
            return _trailList;
        }

        public bool TrailExist(string Name)
        {
            var _trail = _DbContext.Trails.Any(N => N.Name.ToLower().Trim() == Name.ToLower().Trim());
            
            return _trail;
        }

        public bool TrailExist(int Id)
        {
            var _trail = _DbContext.Trails.Any(N => N.Id == Id);

            return _trail;
        }

        public bool Save()
        {
            //return _DbContext.SaveChanges() >= 0 ? true:false;
            return _DbContext.SaveChanges() >= 0;
        }

        public bool UpdateTrail(Trail trail)
        {
            
             _DbContext.Trails.Update(trail);
            
            return Save();         

        }

        public ICollection<Trail> GetTrailsInNationalPark(int NationalParkID)
        {
            var TrailsInPark = _DbContext.Trails.Include(NP => NP.NationalPark).Where(C => C.NationalParkID == NationalParkID).ToList();

            return TrailsInPark;
        }
    }
}
