using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ParkiAPI.Models
{
    public class Trail
    {
        //https://www.c-sharpcorner.com/UploadFile/af66b7/data-annotations-for-mvc/
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }
        public double Elivation { get; set; }
        public enum DifficultyType { Easy, Moderate, Difficult, Expert };

        public DifficultyType  Difficulty { get; set; }

        //Forign Key realtion
        [Required]
        public int NationalParkID { get; set; }

        [ForeignKey("NationalParkID")]
        public NationalPark NationalPark { get; set; }

        public DateTime DateCreated { get; set; }

    }
}
