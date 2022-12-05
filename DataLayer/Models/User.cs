using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class User : IdentityUser
    {

        public string PhotoName { get; set; }
        public string? SocialAccount { get; set; }

        [StringLength(11)]
        public string PhoneNumber { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<Comment> Comments { get; set; }
        
    }


 }