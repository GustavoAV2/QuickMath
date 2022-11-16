using System.ComponentModel.DataAnnotations;

namespace MathMvc.Models
{
    public class RegisterViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
<<<<<<< HEAD
        public string UserName { get; set; }

=======
        
>>>>>>> 035134c6a68bf7d4d7657daf1daa2d172cc3f91c
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
