
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace terminus.shared.models
{
    public class AppUser : IdentityUser
    {
        [MaxLength(100), Required]
        public string firstName { get; set; }

        [MaxLength(100), Required]
        public string lastName { get; set; }

        [MaxLength(100)]
        public string middleName { get; set; }

        public Company company { get; set; }

    }
}
