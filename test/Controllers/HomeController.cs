using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;

namespace Fatwa.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        UnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ILogger<Controller> logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationContext _context;

        private readonly IConfigurationSection _jwtSettings;
        public HomeController(RoleManager<IdentityRole> roleManager, ApplicationContext db, UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger, IConfiguration configuration, ApplicationContext context)
        {
            unitOfWork = new UnitOfWork(db);
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            _jwtSettings = configuration.GetSection("JwtSettings");
            this._roleManager = roleManager;
            _context = context;
        }
        //[HttpGet("/Home/Index"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<IActionResult> Index()
        //{
            
        //}

    }
}
