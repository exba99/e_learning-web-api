using System;
using System.Linq;
using System.Threading.Tasks;
using e_learning_api.Authentication;
using e_learning_api.DataModel;
using e_learning_api.DbModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace e_learning_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {

        private ElearningDbContext _context;
        public LanguageController(ElearningDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> getAllLanguages()
        {
            if (HttpContext.User.Identity.IsAuthenticated && HttpContext.User.IsInRole(UserRole.Responsable_Admin))
            {
                Array languages;
                try
                {
                    languages = await _context.Languages.Select(c => new { c.LanguageId, c.Label }).ToArrayAsync();
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Erreur", Message = "Erreur lors de la recuperation des langues!" });
                }

                return Ok(languages);
            }
            return Unauthorized();
        }

        [HttpPut]
        [Route("update")]
        public IActionResult UpdateLanguage(LanguageModel model)
        {
            if (HttpContext.User.Identity.IsAuthenticated && HttpContext.User.IsInRole(UserRole.Responsable_Admin))
            {
                Language language = _context.Languages.FirstOrDefault(c => c.LanguageId == model.LanguageId);

                if (language == null)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseModel { Status = "Erreur", Message = "Cette langue n'existe pas !" });


                var languageExist = _context.Languages.FirstOrDefault(c => c.Label == model.Label);
                if (languageExist != null)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseModel { Status = "Erreur", Message = "Cette langue existe déjà !" });

                language.Label = model.Label;
                _context.Languages.Update(language);
                _context.SaveChanges();

                return Ok(new ResponseModel { Status = "Success", Message = "Langue modifiée avec succès!" });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddLanguage(LanguageModel model)
        {
            if (HttpContext.User.Identity.IsAuthenticated && HttpContext.User.IsInRole(UserRole.Responsable_Admin))
            {
                var languageExist = _context.Languages.FirstOrDefault(c => c.Label == model.Label);

                if (languageExist != null)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseModel { Status = "Erreur", Message = "Cette langue existe déjà !" });

                Language language = new Language();
                language.Label = model.Label;
                await _context.Languages.AddAsync(language);
                await _context.SaveChangesAsync();

                return Ok(new ResponseModel { Status = "Success", Message = "Langue créée avec succès!" });
            }
            return Unauthorized();
        }

        [HttpDelete]
        [Route("delete/{id_language}")]
        public IActionResult DeleteLanguage(int id_language)
        {
            if (HttpContext.User.Identity.IsAuthenticated && HttpContext.User.IsInRole(UserRole.Responsable_Admin))
            {
                var language = _context.Languages.FirstOrDefault(c => c.LanguageId == id_language);

                if (language == null)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseModel { Status = "Erreur", Message = "Cette langue n'existe pas !" });

                _context.Languages.Remove(language);
                _context.SaveChanges();

                return Ok(new ResponseModel { Status = "Success", Message = "Langue supprimée avec succès!" });
            }
            return Unauthorized();
        }
    }

}
