using BlumindApp.Services;
using Commom.Models.Category;
using Entities.BlumindDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Validation.Creator;

namespace BlumindApp.Controllers {
    [Route("api/[controller]/[action]")]
    public class CategoryController : BaseController {
        private readonly CategoryService _service;
        public CategoryController(CategoryService categoryService)
        {
            _service = categoryService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllCategories()
        {
            using (var db = new BlumindbaseContext())
            {
                var model = await db.Categories.Select(p => new { p.Id, p.Name }).ToListAsync();

                return Ok(model);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCategory([FromQuery] int categoryId)
        {
            using (var db = new BlumindbaseContext())
            {
                var model = await db.Categories.FirstOrDefaultAsync(p => p.Id == categoryId);

                return Ok(model);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostCategory([FromBody] CategoryPostModel categoryModel)
        {
            var validator = ValidatorFactory.GetValidator<CategoryPostModel>();
            validator.Validate(categoryModel);

            await _service.SaveCategory(categoryModel, CurrentUserId);

            return Ok();
        }
    }
}
