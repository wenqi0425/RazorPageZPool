using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ZPool.Models;

namespace ZPool.Models
{
    public class Car
    {
        public int CarID { get; set; } 
        [Required]
        [Display(Name = "Brand:")]
        public string Brand { get; set; }
        [Required]
        [Display(Name = "Model:")]
        public string Model { get; set; }

        [Required]
        [Display(Name = "Car:")]
        public string NumberPlate { get; set; }

        [Display(Name = "Color:")]
        public string Color { get; set; }

        [Display(Name = "Number of Seats:")]
        [Range(typeof(int), "1", "9")]
        public int NumberOfSeats { get; set; }


        //Foreign Keys
        public int AppUserID { get; set; }//FK

        //Navigation Properties
        public AppUser AppUser { get; set; }
        public ICollection<Ride> Rides { get; set; }

        public override string ToString()
        {
            return $"{Brand} {Model}, {Color}";
        }
    }
}
