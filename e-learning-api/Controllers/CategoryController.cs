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
    public class CategoryController : ControllerBase
    {

        private ElearningDbContext _context;
        public CategoryController(ElearningDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> getAllCategories()
        {
            if (HttpContext.User.Identity.IsAuthenticated && HttpContext.User.IsInRole(UserRole.Responsable_Admin))
            {
                Array categories;
                try
                {
                    categories = await _context.Categories.Select(c => new { c.CategoryId, c.Label }).ToArrayAsync();
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Erreur", Message = "Erreur lors de la recuperation des categories!" });
                }

                return Ok(categories);
            }
            return Unauthorized();
        }

        [HttpPut]
        [Route("update")]
        public IActionResult UpdateCategory(CategoryModel model)
        {
            if (HttpContext.User.Identity.IsAuthenticated && HttpContext.User.IsInRole(UserRole.Responsable_Admin))
            {
                Category category = _context.Categories.FirstOrDefault(c => c.CategoryId == model.CategoryId);

                if (category == null)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseModel { Status = "Erreur", Message = "Cette catégorie n'existe pas !" });


                var categoryExist = _context.Categories.FirstOrDefault(c => c.Label == model.Label);
                if (categoryExist != null)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseModel { Status = "Erreur", Message = "Cette categorie existe déjà !" });

                category.Label = model.Label;
                _context.Categories.Update(category);
                _context.SaveChanges();

                return Ok(new ResponseModel { Status = "Success", Message = "Catégorie modifiée avec succès!" });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddCategory(CategoryModel model)
        {
            if (HttpContext.User.Identity.IsAuthenticated && HttpContext.User.IsInRole(UserRole.Responsable_Admin))
            {
                var categoryExist = _context.Categories.FirstOrDefault(c => c.Label == model.Label);

                if (categoryExist != null)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseModel { Status = "Erreur", Message = "Cette categorie existe déjà !" });

                Category category = new Category();
                category.Label = model.Label;
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();

                return Ok(new ResponseModel { Status = "Success", Message = "Catégorie créée avec succès!" });
            }
            return Unauthorized();
        }

        [HttpDelete]
        [Route("delete/{id_category}")]
        public IActionResult DeleteCategory(int id_category)
        {
            if (HttpContext.User.Identity.IsAuthenticated && HttpContext.User.IsInRole(UserRole.Responsable_Admin))
            {
                var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id_category);

                if (category == null)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseModel { Status = "Erreur", Message = "Cett catégorie n'existe pas !" });

                _context.Categories.Remove(category);
                _context.SaveChanges();

                return Ok(new ResponseModel { Status = "Success", Message = "Categporie supprimée avec succès!" });
            }
            return Unauthorized();
        }
    }

}
