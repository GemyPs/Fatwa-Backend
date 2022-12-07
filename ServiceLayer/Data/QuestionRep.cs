using DataLayer;
using DataLayer.Models;
using ServiceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Data
{
    public class QuestionRep : Repository<Question>
    {
        private UnitOfWork unitOfWork;

        public QuestionRep(ApplicationContext context) : base(context)
        {
            unitOfWork = new UnitOfWork(context);
        }
        public void Create (QuestionsVM model , string userName) 
        {
            string userID = unitOfWork.UserLogin.GetId(userName);
            Question q = new Question
            {
                SheikhId = model.SheikhID,
                QuestionText = model.QuestionText,
                UserId = userID,

            };
            Insert(q);  
        }

        public List<Question> GetAll()
        {
            return Fetch();
        }

        public void Delete(int questionId)
        {
            Question question = GetById(questionId);
            Delete(question);
            unitOfWork.Commit();
        }

        public List<Question> GetBySheikhId(string sheikhId)
        {
            return AsQuerable().Where(x => x.SheikhId == sheikhId).ToList();
        }

        public bool Is_Found(int questionId)
        {
            return IsFound(questionId);
        }
    }
}
