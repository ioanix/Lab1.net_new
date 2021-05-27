﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab1_new.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Reservation> Reservations { get; set; }
    }
}
