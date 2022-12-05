using DataLayer;
using ServiceLayer.Data;

namespace ServiceLayer
{
    public class UnitOfWork
    {
        ApplicationContext dbContext;
        public UnitOfWork(ApplicationContext context)
        {
            dbContext = context;
        }

        public bool Commit()
        {
            bool result = true;
            var dbContextPlugin = dbContext.Database.BeginTransactionAsync().Result;
            try
            {
                dbContext.SaveChangesAsync().Wait();
                dbContextPlugin.Commit();
            }
            catch (Exception ex)
            {
                result = false;
                dbContextPlugin.Rollback();
            }
            return result;
        }
        public UserLogin UserLogin { get { return new UserLogin(dbContext); } }
        public QuestionRep QuestionRep { get { return new QuestionRep(dbContext); } }
        public CommentRep CommentRep { get { return new CommentRep(dbContext); } }
    }
}