#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ZPool.Models;

namespace ZPool.Models
{
    public class AppUser : IdentityUser<int>
    {
        // Email and other props are members of the base class
        // and do not require a concrete implementation

        public override int Id { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Gender { get; set; }
        public string? Introduction { get; set; }
        public string? AvatarName { get; set; } 
    }
}
