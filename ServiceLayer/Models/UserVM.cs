using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Models
{
    public class UserVM
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public string UserID { get; set; }
    }
}
