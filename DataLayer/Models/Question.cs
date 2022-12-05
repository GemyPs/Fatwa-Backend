using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Question
    {
        public int Id { get; set; }

        public string QuestionText { get; set; }

        public string SheikhId { get; set; }

        public string UserId { get; set; }


        public User User { get; set; }


        public virtual ICollection<Comment> Comments { get; set; }
    }
}
