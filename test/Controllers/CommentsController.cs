using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using ServiceLayer.Models;

namespace Fatwa.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CommentsController : Controller
    {
        UnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ILogger<Controller> logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationContext _context;

        private readonly IConfigurationSection _jwtSettings;
        public CommentsController(RoleManager<IdentityRole> roleManager, ApplicationContext db, UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger, IConfiguration configuration, ApplicationContext context)
        {
            unitOfWork = new UnitOfWork(db);
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            _jwtSettings = configuration.GetSection("JwtSettings");
            this._roleManager = roleManager;
            _context = context;
        }
        [HttpPost("/PostComment")]
        public IActionResult PostComment(commentsVM newComment)
        {
            try
            {
                bool questionFound = unitOfWork.QuestionRep.Is_Found(newComment.questionID);
                if (!questionFound)
                    return BadRequest(new { message = "This question not found", status = false });

                unitOfWork.CommentRep.create(newComment, User.Identity.Name);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message, status = false });
            }
            return Ok(new { message = "Comment Posted Successfuly !", status = true});
        }

        //[HttpGet("/GetRelatedComments")]
        //public IActionResult getQuestionComments(int requestQuestionID) 
        //{
        //    try
        //    {
        //        bool questionFound = unitOfWork.QuestionRep.Is_Found(requestQuestionID);
        //        if (!questionFound)
        //            return BadRequest(new { message = "Question not found", status = false });

        //        List<Comment> relatedComments = unitOfWork.CommentRep.getRelated(requestQuestionID);
        //        return Ok(new { message = "Related comments fitched successfully", data = relatedComments, status = true });
        //    }catch(Exception e)
        //    {
        //        return BadRequest(new { message = e.Message, status = false});
        //    }
        //}


        [HttpDelete("/Delete")]
        public IActionResult Delete(int commentID)
        {
            try
            {
                if (!unitOfWork.CommentRep.is_found(commentID))
                    return BadRequest(new { message = "comment not found", status = false });

                unitOfWork.CommentRep.remove(commentID);
                return Ok(new { message = "comment deleted successfully", status = true});
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
