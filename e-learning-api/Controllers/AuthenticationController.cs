﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using e_learning_api.Authentication;
using e_learning_api.Configuration;
using e_learning_api.DataModel;
using e_learning_api.DbModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace e_learning_api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private ElearningDbContext _context;

        public AuthenticationController(ElearningDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        [Route("admin/create-roles")]
        public async Task<IActionResult> CreateRoles()
        {
            if (!await roleManager.RoleExistsAsync(UserRole.Responsable_Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRole.Responsable_Admin));
            }
            if (!await roleManager.RoleExistsAsync(UserRole.Teacher))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRole.Teacher));
            }
            if (!await roleManager.RoleExistsAsync(UserRole.Student))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRole.Student));
            }

            return Ok(new ResponseModel { Status = "Success", Message = "Ajout des roles reussi" });
        }

        [HttpGet]
        [Route("admin/register")]
        public async Task<IActionResult> RegisterAdmin()
        {

            string Email = "admin@admin.com";
            string EmailContact = "abdoulbachir98@@admin.com";
            string Password = "Passer@1234";
            string PhoneNumber = "781025261";
            string Avatar = "data:@file/jpeg;base64,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";
            string FirstName = "Exba";
            string LastName = "Juniors";

            var userExist = await userManager.FindByEmailAsync(Email);
            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Erreur", Message = "Cette utilisateur existe deja !" });
            }

            ApplicationUser userSecurity = new ApplicationUser()
            {
                Email = Email,
                UserName = Email,
                PhoneNumber = PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            // creation user security
            var result = await userManager.CreateAsync(userSecurity, Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Erreur", Message = "L'ajout de l'utilisateur a echoué" });
            }

            //Atribution du role et creation user
            var role = await roleManager.FindByNameAsync(UserRole.Responsable_Admin);

            User newUser = new User();

            newUser.Email = Email;
            newUser.EmailContact = EmailContact;
            newUser.Avatar = Avatar;
            newUser.FirstName = FirstName;
            newUser.PhoneNumber = PhoneNumber;
            newUser.LastName = LastName;
            newUser.Password = userSecurity.PasswordHash;
            newUser.IdUser = userSecurity.Id;
            newUser.IdRole = role.Id;


            _context.users.Add(newUser);
            _context.SaveChanges();

            await userManager.AddToRoleAsync(userSecurity, role.Name);

            return Ok(new ResponseModel { Status = "Success", Message = "L'ajout de l'utilisateur a reussi" });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterMember([FromBody] RegisterModel modelUser)
        {
            var userExist = await userManager.FindByEmailAsync(modelUser.Email);
            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Erreur", Message = "Cette utilisateur existe deja !" });
            }

            ApplicationUser userSecurity = new ApplicationUser()
            {
                Email = modelUser.Email,
                UserName = modelUser.Email,
                PhoneNumber = modelUser.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            // creation user security
            var result = await userManager.CreateAsync(userSecurity, modelUser.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Erreur", Message = "L'ajout de l'utilisateur a echoué" });
            }

            //Atribution du role et creation user
            var role = await roleManager.FindByIdAsync(modelUser.RoleId);

            User newUser = new User();

            newUser.Email = modelUser.Email;
            newUser.EmailContact = modelUser.EmailContact;
            newUser.Avatar = modelUser.Avatar;
            newUser.Biography = modelUser.Biography;
            newUser.PhoneNumber = modelUser.PhoneNumber;
            newUser.FirstName = modelUser.FirstName;
            newUser.LastName = modelUser.LastName;
            newUser.Speciality = modelUser.Speciality;
            newUser.Password = userSecurity.PasswordHash;
            newUser.IdUser = userSecurity.Id;
            newUser.IdRole = role.Id;

            _context.users.Add(newUser);
            _context.SaveChanges();

            await userManager.AddToRoleAsync(userSecurity, role.Name);

            return Ok(new ResponseModel { Status = "Success", Message = "L'ajout de l'utilisateur a reussi" });
        }


        [HttpGet]
        [Route("user-info")]
        public async Task<IActionResult> GetInfoUser()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string userId = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                User loggedUser = await _context.users.FirstOrDefaultAsync(x => x.Email == userId);
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
            return Unauthorized();
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> logOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
                return NoContent();
            }
            return Unauthorized();
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {

                var token = GenerateJwtToken(user);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                });

            }

            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "ErrorLogin", Message = "Bad Credentials!" });
        }

        private JwtSecurityToken GenerateJwtToken(IdentityUser user)
        {
            var roles = userManager.GetRolesAsync((ApplicationUser)user);

            var key = Encoding.UTF8.GetBytes(_configuration["JwtConfig:Secret"]);



            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, roles.Result[0]),
                },

                expires: DateTime.Now.AddHours(6),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return tokenDescriptor;
        }
    }
}
