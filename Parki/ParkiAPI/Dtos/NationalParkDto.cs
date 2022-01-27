using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkiAPI.Dtos
{
    public class NationalParkDto
    {
        //https://www.c-sharpcorner.com/UploadFile/af66b7/data-annotations-for-mvc/
        public int Id { get; set; }
       
        [Required]
        public string Name { get; set; }
        [Required]
        public string State { get; set; }

        public DateTime Created { get; set; }

        public byte[] ParkPicture { get; set; }
        public IFormFile Picture { get; set; }

        public DateTime Establisehd { get; set; }
    }
}
