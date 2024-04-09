using System.ComponentModel.DataAnnotations;

namespace INTEX_AURORA_BRICKS.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? CountryOfResidence { get; set; }

        [StringLength(1)]
        public string? Gender { get; set; }

        //public int? Age
        //{
        //    get
        //    {
        //        if (BirthDate == null)
        //        {
        //            return 0; // Handle this case based on your application logic
        //        }

        //        DateTime now = DateTime.Today;
        //        int age = now.Year - BirthDate.Year;
        //        if (BirthDate > now.AddYears(-age))
        //        {
        //            age--;
        //        }

        //        return age;
        //    }
        //    // Note: Age is typically calculated based on BirthDate, so no need for a setter in this case
        //}
    }
}
