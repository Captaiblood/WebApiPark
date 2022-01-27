using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkiWeb.Models;
using ParkiWeb.Models.ViewModels;
using ParkiWeb.Repository.Contract;
using Service.Library.ServiceResponder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ParkiWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _nParkRepo;
        private readonly ITrailRepository _trailRepo;

        public HomeController(ILogger<HomeController> logger, INationalParkRepository nationalParkRepository, ITrailRepository trailRepository)
        {
            _logger = logger;
            _nParkRepo = nationalParkRepository;
            _trailRepo = trailRepository;
        }

        public async Task<IActionResult> Index()
        {

            ServiceResponse<List<NationalPark>> _serviceResponce = new();
            ServiceResponse<NationalPark> _serviceResponceById = new();

            _serviceResponce = await _nParkRepo.GetAllAsyncServiceWrapper<ServiceResponse<List<NationalPark>>>(StaticDetils.NationalParkApiPath + "GetWithWarpper");
            _serviceResponceById = await _nParkRepo.GetAsyncServiceWrapper<ServiceResponse<NationalPark>>(StaticDetils.NationalParkApiPath + "GetWithWarpper", 2);
            if (_serviceResponce != null && _serviceResponce.Success)
            {
           
            }


            IndexVM listOFParksAndTrails = new IndexVM()
            {
                //NationalParkList = await _nParkRepo.GetAllAsync(StaticDetils.NationalParkApiPath),
                NationalParkList = _serviceResponce.Data,
                TrailList = await _trailRepo.GetAllAsync(StaticDetils.TrailApiPath),
            };

            return View(listOFParksAndTrails);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
