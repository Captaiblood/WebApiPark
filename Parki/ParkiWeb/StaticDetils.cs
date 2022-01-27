using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkiWeb
{
    public class StaticDetils
    {
        public static string ApiBaseUrl = "https://localhost:44308/";
        public static string NationalParkApiPath = ApiBaseUrl + "api/v1/NationalPark/";
        public static string TrailApiPath = ApiBaseUrl + "api/v1/trails/";
    }
}
