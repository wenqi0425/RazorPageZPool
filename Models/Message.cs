using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ZPool.Models;

namespace ZPool.Models
{
    public class Message
    {
        public int Id { get; set; }
        public bool IsRead { get; set; }
        [Required(ErrorMessage = "A message text is required.")]
        [StringLength(240)]
        public string MessageBody { get; set; }
        public DateTime SendingDate { get; set; }

        [Required]
        public int SenderId { get; set; }
        [ForeignKey(nameof(SenderId))]
        [ValidateNever]
        public AppUser Sender { get; set; }

        [Required]
        public int ReceiverId { get; set; }
        [ForeignKey(nameof(ReceiverId))]
        [ValidateNever]
        public AppUser Receiver { get; set; }
    }
}
