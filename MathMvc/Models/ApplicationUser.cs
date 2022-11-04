using Microsoft.AspNetCore.Identity;

namespace MathMvc.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public int NumberResolvedAccounts { get; set; } = 0;
        [PersonalData]
        public int NumberUnresolvedAccounts { get; set; } = 0;
    }
}
