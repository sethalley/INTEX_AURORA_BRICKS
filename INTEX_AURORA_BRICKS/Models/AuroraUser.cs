using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace INTEX_AURORA_BRICKS.Models


{
    public class AuroraUser: IdentityUser
    {

        // Hidden in the IdentityUser class
        // public string Id { get; set; }
        // public string Email { get; set; }
        // public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


    }
}
