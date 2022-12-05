using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Models
{
    public class RegistrationVM
    {

        [Required(ErrorMessage = "Mail Required")]
        [EmailAddress(ErrorMessage = "You Must Enter Valid Mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Min Length 8")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Min Length 8")]
        [Compare("Password", ErrorMessage = "Not Matching")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        public string Name { get; set; }
        // public IFormFile PhotoUrl { get; set; }
        // public string PhotoName { get; set; }
        [MaxLength(11, ErrorMessage = "MaxLength 11")]
        public string MobileNumber { get; set; }


    }
}