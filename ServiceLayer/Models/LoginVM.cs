using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Mail Required")]
        [EmailAddress(ErrorMessage = "You Must Enter Valid Mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Min Len 8")]
        public string Password { get; set; }

        public bool RemomberMe { get; set; }


    }
}