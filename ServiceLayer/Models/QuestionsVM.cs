using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Models
{
    public class QuestionsVM
    {
        [Required]
        [RegularExpression("[a-zA-Z]{2,500}", ErrorMessage = "Your question must contain only charachters")]
        public string QuestionText { get; set; }
        public string SheikhID { get; set; }
    }
}
