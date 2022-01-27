using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkiWeb.Models
{
    public class Trail
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Distance is required")]       
        [Range(0, double.MaxValue)]
        public double Distance { get; set; }
                      
        [Range(0, double.MaxValue)]
        public double Elivation { get; set; }

        public enum DifficultyType { Easy, Moderate, Difficult, Expert };
        public DifficultyType Difficulty { get; set; }

        //Forign Key realtion
        [Required]
        public int NationalParkID { get; set; }
        public NationalPark NationalPark { get; set; }
    }
}
