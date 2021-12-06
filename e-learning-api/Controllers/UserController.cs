using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using e_learning_api.Authentication;
using e_learning_api.DataModel;
using e_learning_api.DbModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace e_learning_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private ElearningDbContext _context;

        public UserController(ElearningDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpPut]
        [Route("update-basic-info")]
        public async Task<IActionResult> UpdateBasicInfo(UpdateBasicInfoUserModel model)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string userId = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                User loggedUser = await _context.users.FirstOrDefaultAsync(x => x.Email == userId);
                var userOwin = await userManager.FindByIdAsync(loggedUser.IdUser);
                if (userOwin != null && loggedUser != null)
                {
                    //Update Owin User
                    userOwin.Email = model.Email;
                    userOwin.UserName = model.Email;
                    await userManager.UpdateAsync(userOwin);

                    // Update My User
                    loggedUser.Email = model.Email;
                    loggedUser.EmailContact = model.EmailContact;
                    loggedUser.Avatar = model.Avatar;
                    loggedUser.Biography = model.Biography;
                    loggedUser.Speciality = model.Speciality;
                    loggedUser.FirstName = model.FirstName;
                    loggedUser.LastName = model.LastName;
                    loggedUser.PhoneNumber = model.PhoneNumber;

                    _context.users.Update(loggedUser);
                    await _context.SaveChangesAsync();



                    var role = await roleManager.FindByIdAsync(loggedUser.IdRole);

                    return Ok(new
                    {
                        email = loggedUser.Email,
                        firstName = loggedUser.FirstName,
                        lastName = loggedUser.LastName,
                        avatar = loggedUser.Avatar,
                        phoneNumber = loggedUser.PhoneNumber,
                        idUser = loggedUser.IdUser,
                        emailContact = loggedUser.EmailContact,
                        biography = loggedUser.Biography,
                        speciality = loggedUser.Speciality,
                        roleName = role.Name,
                        idRole = role.Id,
                    });
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Error", Message = "Error for updating baisc info user!" });
            }
            return Unauthorized();

        }

        [HttpPut]
        [Route("update-password")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordModel model)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string userId = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                User loggedUser = await _context.users.FirstOrDefaultAsync(x => x.Email == userId);
                var userOwin = await userManager.FindByIdAsync(loggedUser.IdUser);
                bool result = await userManager.CheckPasswordAsync(userOwin, model.CurrentPassword);
                if (result)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(userOwin);

                    var user = await userManager.ResetPasswordAsync(userOwin, token, model.NewPassword);

                    return Ok(new ResponseModel { Status = "Success", Message = "Reset password success!" });

                }

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Error", Message = "Votre mot de passe actuel est incorrect !" });
            }
            return Unauthorized();

        }
    }
}
