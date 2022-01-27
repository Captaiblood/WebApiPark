using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkiAPI.Models
{
    public class NationalPark
    {//https://www.c-sharpcorner.com/UploadFile/af66b7/data-annotations-for-mvc/
        [Key]
        public int Id { get; set; }
        [Required]
        public string  Name { get; set; }
        [Required]
        public string State { get; set; }

        public DateTime Created { get; set; }
        public byte[] ParkPicture { get; set; }

        public string Picture { get; set; }
        public DateTime Establisehd { get; set; }
    }
}
