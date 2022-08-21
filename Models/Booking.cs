using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ZPool.Models;

namespace ZPool.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        public DateTime Date { get; set; }

        [Required]
        public string PickUpLocation { get; set; }
        [Required]
        public string DropOffLocation { get; set; }

        public string BookingStatus { get; set; }
        
        //Foreign Keys
        public int RideID { get; set; }//FK
        public int AppUserID { get; set; }//FK


        //Navigation Properties
        public AppUser AppUser { get; set; }
        public Ride Ride { get; set; }
    }
}
