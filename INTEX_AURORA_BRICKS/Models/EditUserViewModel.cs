using System.ComponentModel.DataAnnotations;

namespace INTEX_AURORA_BRICKS.Models
{
    public class EditUserViewModel
    {
       
        public string Id { get; set; }

        [Required]
        public string first_name { get; set; }

        public string last_name { get; set; }

        public string country_of_residence { get; set; }

        [StringLength(1)]
        public string? gender { get; set; }

        public int? age { get; set; }


    }
}
