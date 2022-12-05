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
    [ApiController]
    [Route("[controller]")]
    public class QuestionsController : Controller
    {
        UnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ILogger<Controller> logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationContext _context;

        private readonly IConfigurationSection _jwtSettings;
        public QuestionsController(RoleManager<IdentityRole> roleManager, ApplicationContext db, UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger, IConfiguration configuration, ApplicationContext context)
        {
            unitOfWork = new UnitOfWork(db);
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            _jwtSettings = configuration.GetSection("JwtSettings");
            this._roleManager = roleManager;
            _context = context;
        }


        [HttpPost("/SendQuestion"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateQuestion(QuestionsVM newQuestion)
        {
            try
            {
                unitOfWork.QuestionRep.Create(newQuestion, User.Identity.Name);
                unitOfWork.Commit();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(new { message = "Question added !", status = true });
        }

        [HttpGet("/GetAllQuestions"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetAllQuestions()
        {
            List<Question> questionList = new List<Question>();
            try
            {
                questionList = unitOfWork.QuestionRep.GetAll();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = "INTERNAL_ERROR", status = false, error_message = e.Message });
            }
            return Ok(new { message = "Questions Fitched Successfully!", status = true, data = questionList });
        }

        [HttpGet("/GetQuestionsBySheikhID"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetBySheikhId(string SheikhId)
        {
            try
            {
                var IsSheikhFound = userManager.FindByIdAsync(SheikhId);
                if (IsSheikhFound == null)
                {
                    return BadRequest(new { message = "Sheikh is not found !", status = false });
                }
                List<Question> question = unitOfWork.QuestionRep.GetBySheikhId(SheikhId); 
                return Ok(new { message = "Questions Fithced !", status = true, data = question });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = "INTERNAL_ERROR", status = false, error_meesage = e.Message });
            }
        }

        [HttpDelete("/DeleteQuestion")]
        public IActionResult Delete(int questionId)
        {
            try
            {
                if (!unitOfWork.QuestionRep.IsFound(questionId))
                    return BadRequest(new { message = "Question not found", status = false});
                unitOfWork.CommentRep.removebyQuestionId(questionId);
                unitOfWork.QuestionRep.Delete(questionId);
                return Ok(new { Message = "Question has been deleted!", status = true });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "There is an error while deleting the question", status = false, data = e.Message});
            }
        }
    }
}
