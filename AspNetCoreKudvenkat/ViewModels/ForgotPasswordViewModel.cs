using System.ComponentModel.DataAnnotations;

namespace AspNetCoreKudvenkat.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}