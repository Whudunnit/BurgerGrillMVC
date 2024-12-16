﻿using Microsoft.AspNetCore.Identity;

namespace BurgerGrill.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Order>? Orders { get; set; }
    }
}
