using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using e_learning_api.Authentication;
using e_learning_api.DbModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace e_learning_api.Controllers
{
    [Route("api/[controller]")]
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


        [HttpPost]
        [Route("admin/register")]
        public async Task<IActionResult> RegisterMemberAdmin([FromBody] User user)
        {
            var userExist = await userManager.FindByEmailAsync(user.Email);
            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Erreur", Message = "Cette utilisateur existe deja !" });
            }

            ApplicationUser myUser = new ApplicationUser()
            {
                Email = user.Email,
                UserName = user.Email,
                PhoneNumber = user.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            // creation user security
            var result = await userManager.CreateAsync(myUser, user.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Erreur", Message = "L'ajout de l'utilisateur a echoué" });
            }

            //Atribution du role et creation user
            var role = await roleManager.FindByNameAsync(UserRole.Responsable_Admin);

            user.IdUser = myUser.Id;
            user.IdRole = role.Id;
            _context.users.Add(user);
            _context.SaveChanges();

            await userManager.AddToRoleAsync(myUser, role.Name);

            return Ok(new ResponseModel { Status = "Success", Message = "L'ajout de l'utilisateur a reussi" });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterMember([FromBody] User user)
        {
            var userExist = await userManager.FindByEmailAsync(user.Email);
            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Erreur", Message = "Cette utilisateur existe deja !" });
            }

            ApplicationUser myUser = new ApplicationUser()
            {
                Email = user.Email,
                UserName = user.Email,
                PhoneNumber = user.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            // creation user security
            var result = await userManager.CreateAsync(myUser, user.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Erreur", Message = "L'ajout de l'utilisateur a echoué" });
            }

            //Atribution du role et creation user
            var role = await roleManager.FindByIdAsync(user.IdRole);

            user.IdUser = myUser.Id;
            user.IdRole = role.Id;
            _context.users.Add(user);
            _context.SaveChanges();

            await userManager.AddToRoleAsync(myUser, role.Name);

            return Ok(new ResponseModel { Status = "Success", Message = "L'ajout de l'utilisateur a reussi" });
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, roles[0])
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidIssuer"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    user = user,
                });

            }

            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Erreur", Message = "User inexistant" });
        }
    }
}
