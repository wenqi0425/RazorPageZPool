using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZPool.Models
{
    public class Review
    {
        public  int ReviewId { get; set; }
        public DateTime ReviewDate { get; set; }
        [Required(ErrorMessage = "A message text is required.")]
        [StringLength(240)]
        public string MessageBody { get; set; }
        public int RateValue { get; set; }


        [Required]
        public int ReviewerId { get; set; }
        [ForeignKey(nameof(ReviewerId))]
        [ValidateNever]
        public AppUser Reviewer { get; set; }

        [Required]
        public int RevieweeId { get; set; }
        [ForeignKey(nameof(RevieweeId))]
        [ValidateNever]
        public AppUser Reviewee { get; set; }

        [Required]
        public int RideId { get; set; }
        [ForeignKey(nameof(RideId))]
        [ValidateNever]
        public Ride Ride { get; set; }




        



    }
}
