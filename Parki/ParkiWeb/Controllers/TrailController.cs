using Microsoft.AspNetCore.Mvc;
using ParkiWeb.Models;
using ParkiWeb.Models.ViewModels;
using ParkiWeb.Repository.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkiWeb.Controllers
{
    public class TrailController : Controller
    {
        private readonly INationalParkRepository _nPRepo;
        private readonly ITrailRepository _trailRepo;

        public TrailController(INationalParkRepository parkRepository, ITrailRepository trailRepository)
        {
            _nPRepo = parkRepository;
            _trailRepo = trailRepository;
        }

        public IActionResult Index()
        {
            return View(new Trail() { });
        }

        /// <summary>
        /// will update if ID is not null else will create a new record
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Upsert(int? Id)
        {

            IEnumerable<NationalPark> nParkList = await _nPRepo.GetAllAsync(StaticDetils.NationalParkApiPath);

            TrailVM _trailVM = new TrailVM
            {
                NationalParkList = nParkList.Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()

                }),

                //create an new object of trail 
                //when create request is sent
                  Trail = new Trail()
            };

            
            //If ID is null return empty view
            //to Insert/Create a new record
            if(Id == null)
            {
                return View(_trailVM); // return the view model which contain drop down and trail model
            }

            //else update exising record
            _trailVM.Trail = await _trailRepo.GetAsync(StaticDetils.TrailApiPath, Id.GetValueOrDefault());

            if (_trailVM.Trail == null)
            {
                return NotFound();
            }

            //else return updated record
            return View(_trailVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailVM trailVM)
        {
            if (ModelState.IsValid)
            {               
                               
                if (trailVM.Trail.Id==0)// create a new record
                {
                    // pass the opbject to create a new record
                    await _trailRepo.CreateAsync(StaticDetils.TrailApiPath, trailVM.Trail);
                }
                else //update existing record
                {
                    // pass the object with ID to update exisitng record
                    await _trailRepo.UpdateAsync(StaticDetils.TrailApiPath+trailVM.Trail.Id, trailVM.Trail);

                }

                return RedirectToAction(nameof(Index));
            }
            else // if model is not vaild
            {

                IEnumerable<NationalPark> nParkList = await _nPRepo.GetAllAsync(StaticDetils.NationalParkApiPath);

                TrailVM _trailVM = new TrailVM()
                {
                    NationalParkList = nParkList.Select(i => new  Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),
                    Trail = trailVM.Trail // pass the exisitng object created in Get
                };
               
                return View(_trailVM);

            }
        }

        public async Task<IActionResult> GetAllTrail()
        {
            return Json(new { Data = await _trailRepo.GetAllAsync(StaticDetils.TrailApiPath) });
            
        }

        [HttpDelete]       
        public async Task<IActionResult> Delete(int Id)
        {
            var status = await _trailRepo.DeleteAsync(StaticDetils.TrailApiPath, Id);

            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success=false, message="Delete Not Succesful" });
        }
        
    }
}
