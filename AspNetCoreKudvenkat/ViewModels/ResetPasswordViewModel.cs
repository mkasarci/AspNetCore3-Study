using System.ComponentModel.DataAnnotations;

namespace AspNetCoreKudvenkat.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string UserId { get; set; }
        public string Token { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage="Password and Confirm Password must match!")]
        [Display(Name="Confirm Password")]
        public string PasswordConfirmation { get; set; }
    }
}