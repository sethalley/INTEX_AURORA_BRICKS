﻿using System.ComponentModel.DataAnnotations;

namespace INTEX_AURORA_BRICKS.Models
{
    public class UserRoles
    {
        
        public string UserId { get; set; }
        public string Email { get; set; }

        public List<string> Roles { get; set; }
       
    }
}
