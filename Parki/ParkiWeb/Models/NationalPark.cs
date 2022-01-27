using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParkiWeb.Models
{
    public class NationalPark
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string State { get; set; }

       // public DateTime Created { get; set; }

        public byte[] ParkPicture { get; set; }
        public IFormFile Picture { get; set; }
        public DateTime Establisehd { get; set; }
    }
}
