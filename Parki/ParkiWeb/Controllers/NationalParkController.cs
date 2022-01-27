using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ParkiWeb.Models;
using ParkiWeb.Repository.Contract;
using Service.Library.ServiceResponder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkiWeb.Controllers
{
    public class NationalParkController: Controller
    {
        private readonly INationalParkRepository _nPRepo;

        public NationalParkController(INationalParkRepository parkRepository)
        {
            _nPRepo = parkRepository;
        }

        public IActionResult Index()
        {
            return View(new NationalPark() { });
        }

        /// <summary>
        /// will update if ID is not null else will create a new record
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Upsert(int? Id)
        {
            var _naionalpark = new NationalPark();
            //If ID is null return empty view
            //to Insert/Create a new record
            if(Id == null)
            {
                return View(_naionalpark);
            }

            //else update exising record
            _naionalpark = await _nPRepo.GetAsync(StaticDetils.NationalParkApiPath, Id.GetValueOrDefault());

            if (_naionalpark == null)
            {
                return NotFound();
            }

            //else return updated record
            return View(_naionalpark);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark nationalPark)
        {
            if (ModelState.IsValid)
            {               
                var _files = HttpContext.Request.Form.Files;

                if (_files.Count > 0) // Image was uploaded 
                {
                    //Convert it to byte array
                    byte[] _parkPicture = null;
                    using (var fileStream = _files[0].OpenReadStream())
                    {
                        using (var _memoryStream = new MemoryStream())
                        {
                            fileStream.CopyTo(_memoryStream);
                            _parkPicture = _memoryStream.ToArray();
                        }
                    }

                    nationalPark.ParkPicture = _parkPicture;
                }
                else // else get picture form db
                {
                    if (nationalPark.Id > 0)
                    {
                        var _existingPark = await _nPRepo.GetAsync(StaticDetils.NationalParkApiPath, nationalPark.Id);
                        nationalPark.ParkPicture = _existingPark.ParkPicture;
                    }

                       
                }

                if (nationalPark.Id ==0)// create a new record
                {
                    // pass the opbject to create a new record
                    await _nPRepo.CreateAsync(StaticDetils.NationalParkApiPath, nationalPark);
                }
                else //update existing record
                {
                    // pass the object with ID to update exisitng record
                    await _nPRepo.UpdateAsync(StaticDetils.NationalParkApiPath+nationalPark.Id, nationalPark);

                }

                return RedirectToAction(nameof(Index));
            }
            else // if model is not vaild
            {
                return View(nationalPark);

            }
        }

        public async Task<IActionResult> GetAllNationalPark()
        {
            //List<ServiceResponse<T>> _nationalParkDto = new();

            return Json(new { Data = await _nPRepo.GetAllAsync(StaticDetils.NationalParkApiPath) });
            //var response = await _nPRepo.GetAllAsyncServiceWrapper<ServiceResponse<>>(StaticDetils.NationalParkApiPath);
            //if (response != null && response.Success)
            //{
            //    //_nationalParkDto = JsonConvert.DeserializeObject<List<NationalPark>>(Convert.ToString(response.Data));
            //    return Json(new { response.Data });
            //}
            //else
            //{
            //    return Json(new { response });
            //}
        }

        [HttpDelete]      
        public async Task<IActionResult> Delete(int Id)
        {
            var status = await _nPRepo.DeleteAsync(StaticDetils.NationalParkApiPath, Id);

            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success=false, message="Delete Not Succesful" });
        }
        
    }
}
