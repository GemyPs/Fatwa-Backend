using DataLayer;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Data
{
    public class CommentRep : Repository<Comment>
    {
        private UnitOfWork unitOfWork;

        public CommentRep(ApplicationContext context) : base(context)
        {
            unitOfWork = new UnitOfWork(context);
        }

        public void remove(int commentId)
        {
            Comment comment = GetById(commentId);
            Delete(comment);
        }

        public void removebyQuestionId(int questionId)
        {
            List<Comment> comments = AsQuerable().Where(x => x.QuestionId == questionId).ToList();
            RemoveRange(comments);
        }
    }
}
