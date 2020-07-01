using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreKudvenkat.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role Name")]
        [Remote(action: "isValidName", controller: "Administration")]
        public string RoleName { get; set; }
    }
}