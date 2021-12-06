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
    [Route("api/level-course")]
    [ApiController]
    public class LevelCourseController : ControllerBase
    {

        private ElearningDbContext _context;
        public LevelCourseController(ElearningDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> getAllLevelCourses()
        {
            if (HttpContext.User.Identity.IsAuthenticated && HttpContext.User.IsInRole(UserRole.Responsable_Admin))
            {
                Array levelCourses;
                try
                {
                    levelCourses = await _context.LevelCourses.Select(c => new { c.LevelCourseId, c.Label }).ToArrayAsync();
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Erreur", Message = "Erreur lors de la recuperation des niveaux de cours!" });
                }

                return Ok(levelCourses);
            }
            return Unauthorized();
        }

        [HttpPut]
        [Route("update")]
        public IActionResult UpdateLevelCourse(LevelCourseModel model)
        {
            if (HttpContext.User.Identity.IsAuthenticated && HttpContext.User.IsInRole(UserRole.Responsable_Admin))
            {
                LevelCourse levelCourse = _context.LevelCourses.FirstOrDefault(c => c.LevelCourseId == model.LevelCourseId);

                if (levelCourse == null)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseModel { Status = "Erreur", Message = "Ce niveau de cours n'existe pas !" });


                var levelCourseExist = _context.LevelCourses.FirstOrDefault(c => c.Label == model.Label);
                if (levelCourseExist != null)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseModel { Status = "Erreur", Message = "Ce niveau de cours existe déjà !" });

                levelCourse.Label = model.Label;
                _context.LevelCourses.Update(levelCourse);
                _context.SaveChanges();

                return Ok(new ResponseModel { Status = "Success", Message = "Niveau modifiée avec succès!" });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddLevelCourse(LevelCourseModel model)
        {
            if (HttpContext.User.Identity.IsAuthenticated && HttpContext.User.IsInRole(UserRole.Responsable_Admin))
            {
                var levelCourseExist = _context.LevelCourses.FirstOrDefault(c => c.Label == model.Label);

                if (levelCourseExist != null)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseModel { Status = "Erreur", Message = "Ce niveau de cours existe déjà !" });

                LevelCourse levelCourse = new LevelCourse();
                levelCourse.Label = model.Label;
                await _context.LevelCourses.AddAsync(levelCourse);
                await _context.SaveChangesAsync();

                return Ok(new ResponseModel { Status = "Success", Message = "Niveau créée avec succès!" });
            }
            return Unauthorized();
        }

        [HttpDelete]
        [Route("delete/{id_level}")]
        public IActionResult DeleteLevellCourse(int id_level)
        {
            if (HttpContext.User.Identity.IsAuthenticated && HttpContext.User.IsInRole(UserRole.Responsable_Admin))
            {
                var levelCourse = _context.LevelCourses.FirstOrDefault(c => c.LevelCourseId == id_level);

                if (levelCourse == null)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseModel { Status = "Erreur", Message = "Ce niveau de cours n'existe pas !" });

                _context.LevelCourses.Remove(levelCourse);
                _context.SaveChanges();

                return Ok(new ResponseModel { Status = "Success", Message = "Niveau supprimée avec succès!" });
            }
            return Unauthorized();
        }
    }

}
