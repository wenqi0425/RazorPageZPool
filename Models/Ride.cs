using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZPool.Models
{
    public class Ride
    {
        [Key]
        [Column("RideID")]
        public int RideID { get; set; }  // default 

        [Required]
        [Display(Name = "Date & Time:")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "From:")]
        public string DepartureLocation { get; set; }
        
        [Required]
        [Display(Name = "To:")]
        public string DestinationLocation { get; set; }

        [Required]
        [Display(Name = "Seats Available:")]
        [Range(typeof(int), "1", "9")]
        public int SeatsAvailable { get; set; }

        //Foreign Keys
        [ForeignKey("CarID")]
        [Display(Name = "Car:")]
        public int CarID { get; set; } //FK


        //Navigation properties
        public Car Car { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
