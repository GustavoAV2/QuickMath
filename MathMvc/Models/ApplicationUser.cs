using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MathMvc.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }
        [PersonalData]
        [Required]
        [StringLength(20)]
        public string LastName { get; set; }
        [PersonalData]
        public int NumberResolvedAccounts { get; set; } = 0;
        [PersonalData]
        public int NumberUnresolvedAccounts { get; set; } = 0;
    }
}
