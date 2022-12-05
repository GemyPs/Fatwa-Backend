using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Models
{
    public class commentsVM
    {
        [Required]
        [RegularExpression("[a-zA-Z]{2,400}", ErrorMessage = "Your comment must contain only charachters")]
        public string comment { get; set; }
        [Required]
        public int questionID { get; set; }
    }
}
