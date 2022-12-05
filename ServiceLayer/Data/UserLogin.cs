using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Identity;
using ServiceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Data
{
    public class UserLogin : Repository<User>
    {

        UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private UnitOfWork unitOfWork;
        public UserLogin(ApplicationContext context) : base(context)
        {
            
            unitOfWork = new UnitOfWork(context);

        }

        public string GetId(string userName)
        {
            return AsQuerable().Where(a => a.UserName == userName).Select(a=>a.Id).FirstOrDefault();
        }
        public object Login(LoginVM model, UserManager<User> _userManager,  SignInManager<User> _signInManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            var ueerName = model.Email;
            var user = userManager.FindByNameAsync(ueerName).Result;
            if (user == null)
            {
                return new { IsSuccess = false };
            }

            var result = userManager.CheckPasswordAsync(user, model.Password).Result;
            var result1 = signInManager.PasswordSignInAsync(model.Email, model.Password, model.RemomberMe, false).Result;

            if (result)
            {

                var message = "Login done Successfuly";
                return new { IsSuccess = true, message };


            }
            return new { IsSuccess = false };


        }

        public List<User> GetAll()
        {
            return Fetch();
        }
    }
}