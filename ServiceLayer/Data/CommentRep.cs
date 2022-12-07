using DataLayer;
using DataLayer.Models;
using ServiceLayer.Models;

namespace ServiceLayer.Data
{
    public class CommentRep : Repository<Comment>
    {
        private UnitOfWork unitOfWork;

        public CommentRep(ApplicationContext context) : base(context)
        {
            unitOfWork = new UnitOfWork(context);
        }

        public void create(commentsVM newComment, string userName)
        {
            string? userID = unitOfWork.UserLogin.GetId(userName);
            
            Comment comment = new Comment { 
                CommentText = newComment.comment,
                UserId = userID,
                QuestionId = newComment.questionID,
                IsAnswer = false
            };
            Insert(comment);
            unitOfWork.Commit();
        }

        //public List<Comment> getRelated(int questionId)
        //{
        //    return AsQuerable().Where(x => x.QuestionId == questionId).ToList();
        //}
        public void remove(int commentId)
        {
            Comment comment = GetById(commentId);
            Delete(comment);
            unitOfWork.Commit();
        }

        public bool is_found(int commentId)
        {
            return IsFound(commentId);
        }
    }
}
