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
    [Authorize]
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
        /*
         [1] Method to post an comments for a question(int questionID, string commentText)
            - Take questionID and commentText from User, then get SheikhId from The authentication 
            - get userId from questions dataBase, Post the answer and if the answer posted from sheikh
            - mark is Sheikh bool = 1, else mark bool sheikh = 0
         */
        
        [HttpPost("/PostComment"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult PostComment(commentsVM newComment)
        {
            try
            {   
                int questionID = _context.Question.Where(x => x.Id == newComment.questionID).Select(x => x.Id).FirstOrDefault();
                string? userID = _context.User.Where(x => x.UserName == User.Identity.Name).Select(x => x.Id).FirstOrDefault();
                if (questionID == 0)
                    return BadRequest(new { message = "This question not found", status = false });

                Comment comment = new Comment
                {
                    CommentText = newComment.comment,
                    UserId = userID,
                    QuestionId = newComment.questionID,
                    IsAnswer = false
                };
                _context.Comment.Add(comment);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message, status = false });
            }
            return Ok(new { message = "Comment Posted Successfuly !", status = true});
        }
        [HttpGet("/GetRelatedComments"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult getQuestionComments(int requestQuestionID) 
        {
            int questionID = _context.Question.Where(x => x.Id == requestQuestionID).Select(x => x.Id).FirstOrDefault();
            if (questionID == 0)
                return BadRequest(new { message = "Question not found", status = false});

            var relatedComments = _context.Comment.Where(x => x.QuestionId == questionID);
            return Ok(new { message = "Related comments fitched successfully", data = relatedComments, status = true });
        }
    }
}
