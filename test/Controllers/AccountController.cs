using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using ServiceLayer.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Fatwa.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        UnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ILogger<Controller> logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationContext _context;
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSettings;
        public AccountController(RoleManager<IdentityRole> roleManager, ApplicationContext db, UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger, IConfiguration configuration, ApplicationContext context)
        {
            unitOfWork = new UnitOfWork(db);
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            _jwtSettings = configuration.GetSection("JwtSettings");
            this._roleManager = roleManager;
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("/Users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = unitOfWork.UserLogin.GetAll();
            return Ok(new { message = "Users Fithced successfully!", data = users, status = true });
        }

        [HttpPost("/AddRole")]
        public async Task<IActionResult> AddRoleToUser(string Role_Name, string User_Id)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync(Role_Name);
                if (role == null)
                    return BadRequest("Role is not found");

                var user = await userManager.FindByIdAsync(User_Id);
                if (user == null)
                    return BadRequest("User is not found !");

                await userManager.AddToRoleAsync(user, Role_Name);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(new { message = "Role Added !", status = true });
        }



        [HttpPost("/Registration")]

        public async Task<IActionResult> Registration(RegistrationVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {


                    var user = new User()
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        PhoneNumber = model.MobileNumber,
                        //  PhotoName=model.PhotoName

                    };

                    //CreateAsync //hashes the password
                    IdentityResult result = userManager.CreateAsync(user, model.Password).Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Admin").Wait();
                        //return RedirectToAction("Login");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }

                }
                return Ok(new { message = "User Created Successfully !", status = true });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = "INTERNAL_ERROR", status = false, error_meesage = e.Message });
            }
        }

        [HttpPost("/AddtoRole")]
        public async Task<IActionResult> AddUserToRole(string username, string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            var user = await userManager.FindByNameAsync(username);
            IdentityResult result = null;
            result = await userManager.AddToRoleAsync(user, role.Name);
            return Ok(new { message = "User added to role Successfully !", status = true });
          

        }

        [HttpPost]
        [HttpPost("/Login")]
        public async Task<IActionResult> Login(LoginVM model)
        {
            User user;
            string currentRole, token;
            Fatwa.GeneralMethods.JwtToken jwtToken = new GeneralMethods.JwtToken(_configuration);
            try
            {
                object obj = unitOfWork.UserLogin.Login(model, userManager, signInManager);
                user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return NotFound($"Unable to load user with email '{model.Email}'.");
                }
                token = jwtToken.CreateToken(user);
                var role = await userManager.GetRolesAsync(user);
                currentRole = role.FirstOrDefault();

            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
            return Ok(new { message = "User logged in successfully ! ", status = true, data = new { Email = user.Email, UserId = user.Id, Role = currentRole, Token = token } });
        }


        [HttpGet("/CreateRole")]
        public async Task<IActionResult> CreateRolesandUsers()
        {

            var role = new IdentityRole();
            role.Name = "Admin";
            await _roleManager.CreateAsync(role);


            role = new IdentityRole();
            role.Name = "User";
            await _roleManager.CreateAsync(role);
            role = new IdentityRole();
            role.Name = "Sheikh";
            await _roleManager.CreateAsync(role);



            return Ok(new { message = "User Created Successfully !", status = true });
        }


    }
}