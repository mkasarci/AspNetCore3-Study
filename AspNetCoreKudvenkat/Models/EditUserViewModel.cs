using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AspNetCoreKudvenkat.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreKudvenkat.Models
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }

        [Required]
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name="E-Mail")]
        [ValidEmailDomain("mkcode.com", ErrorMessage= "Email domain must be mkcode.com")]
        public string Email { get; set; }

        [Display(Name="User Name")]
        public string UserName { get; set; }

        public string City { get; set; }
        public List<string> Roles { get; set; } 
        public List<string> Claims { get; set; }
    }
}