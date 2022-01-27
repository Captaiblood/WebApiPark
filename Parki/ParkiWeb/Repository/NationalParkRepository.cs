using ParkiWeb.Models;
using ParkiWeb.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParkiWeb.Repository
{
    public class NationalParkRepository : Repository<NationalPark>, INationalParkRepository
    {
        private readonly IHttpClientFactory _httpClient;

        public NationalParkRepository(IHttpClientFactory httpClient):base(httpClient)
        {
            _httpClient = httpClient;
        }


        
    }
}
