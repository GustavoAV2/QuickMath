using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MathMvc.Models
{
    public class Friends
    {
        [PersonalData]
        [Required]
        public string RequesterId { get; set; }
        [PersonalData]
        [Required]
        public string ReceiverId { get; set; }
        [PersonalData]
        [Required]
        public DateTime RequestDate { get; set; }
        [PersonalData]
        [Required]
        public DateTime ConfirmationDate { get; set; }
        [PersonalData]
        [Required]
        public char Status { get; set; }
    }
}
