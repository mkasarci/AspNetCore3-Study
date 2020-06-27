using System.ComponentModel.DataAnnotations;
using AspNetCoreKudvenkat.Models;
using Microsoft.AspNetCore.Http;

namespace AspNetCoreKudvenkat.ViewModels
{
    public class EmployeeCreateViewModel
    {
        [Required]
        [MinLength(2, ErrorMessage="Name cannot under 2 characters")]
        [MaxLength(50, ErrorMessage= "Name cannot exceed 50 characters")]
        public string Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage= "Invalid E-mail format")]
        [Display(Name = "E-Mail")]
        public string Email { get; set; }     
        [Required]
        public Department? Department { get; set; }
        public IFormFile Photo { get; set; }
    }
}