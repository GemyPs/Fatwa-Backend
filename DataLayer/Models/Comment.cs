using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public string UserId { get; set; }
        public bool IsAnswer { get; set; }


        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }
        public int? QuestionId { get; set; }

        public User User { get; set; }
    }
}