using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;



namespace INTEX_AURORA_BRICKS.Models
{
    public class Customers : IdentityUser
    {
        public Customers()
        {
            // Constructor logic, if any
        }

        public string first_name { get; set; }

        public string last_name { get; set; }

        public DateTime? birth_date { get; set; }

        public string country_of_residence { get; set; }

        [StringLength(1)]
        public string? gender { get; set; }

        public int? age { get; set; }

        public int? recId { get; set; }


    }
}
