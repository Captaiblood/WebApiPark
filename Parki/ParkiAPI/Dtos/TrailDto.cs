using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ParkiAPI.Models.Trail;

namespace ParkiAPI.Dtos
{
    public class TrailDto
    {
        //https://www.c-sharpcorner.com/UploadFile/af66b7/data-annotations-for-mvc/
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }
        public double Elivation { get; set; }
        public DifficultyType Difficulty { get; set; }

        //Forign Key realtion
        [Required]
        public int NationalParkID { get; set; }       
        public NationalParkDto NationalPark { get; set; }
    }
}
