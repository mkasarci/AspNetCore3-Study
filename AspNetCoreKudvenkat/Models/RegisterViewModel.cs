using System.ComponentModel.DataAnnotations;
using AspNetCoreKudvenkat.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreKudvenkat.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller: "Account")]
        [ValidEmailDomain(allowedDomain: "mkcode.com",
         ErrorMessage = "Email domain must be mkcode.com")]  //Custom attribute
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match!")]
        public string ConfirmPassword { get; set; }

        public string City { get; set; }
    }
}