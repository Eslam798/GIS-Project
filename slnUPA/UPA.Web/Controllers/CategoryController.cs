using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using UPA.BLL.Interfaces;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.CategoryVM;
using UPA.Web.Helpers;

namespace UPA.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private  ICategoryService _categoryService;
        private ISubCategoryService _subCategoryService;
        private IMasterAssetService _masterAssetService;
        public CategoryController(ICategoryService categoryService, ISubCategoryService subCategoryService, IMasterAssetService masterAssetService)
        {
            _categoryService = categoryService;
            _subCategoryService = subCategoryService;
            _masterAssetService = masterAssetService;
        }


        [HttpGet]
        [Route("ListCategories")]
        public IEnumerable<Category> ListCategories()
        {
            return _categoryService.ListCategories();
        }



        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditCategoryVM> GetById(int id)
        {
            return _categoryService.GetById(id);
        }



        [HttpPut]
        [Route("UpdateCategory")]
        public IActionResult Update(EditCategoryVM CategoryVM)
        {
            try
            {
                int id = CategoryVM.Id;

                if (CategoryVM.Code?.Length > 5)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "codelen", Message = "code must not be over 99999", MessageAr = "هذا الكود  لابد ألا يزيد عن خمس حروف أو أرقام" });
                }
                var lstCategoryCode = _categoryService.ListCategories().ToList().Where(a => a.Code == CategoryVM.Code && a.Id != id).ToList();
                if (lstCategoryCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "code", Message = "Category code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                }
                var lstCategoryNames = _categoryService.ListCategories().ToList().Where(a => a.Name == CategoryVM.Name && a.Id != id).ToList();
                if (lstCategoryNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "name", Message = "Category name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                var lstCategoryArNames = _categoryService.ListCategories().ToList().Where(a => a.NameAr == CategoryVM.NameAr && a.Id != id).ToList();
                if (lstCategoryArNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "nameAr", Message = "Category arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }

                else
                {
                    int updatedRow = _categoryService.Update(CategoryVM);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok();
        }


        [HttpPost]
        [Route("AddCategory")]
        public ActionResult<Category> Add(CreateCategoryVM CategoryVM)
        {

            if (CategoryVM.Code?.Length > 5)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "codelen", Message = "code must not be over 99999", MessageAr = "هذا الكود  لابد ألا يزيد عن خمس حروف أو أرقام" });
            }
            var lstCategoryCode = _categoryService.ListCategories().ToList().Where(a => a.Code == CategoryVM.Code).ToList();
            if (lstCategoryCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "code", Message = "Category code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstCategoryNames = _categoryService.ListCategories().ToList().Where(a => a.Name == CategoryVM.Name).ToList();
            if (lstCategoryNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "name", Message = "Category name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstCategoryArNames = _categoryService.ListCategories().ToList().Where(a => a.NameAr == CategoryVM.NameAr).ToList();
            if (lstCategoryArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "nameAr", Message = "Category arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                var savedId = _categoryService.Add(CategoryVM);
                return CreatedAtAction("GetById", new { id = savedId }, CategoryVM);
            }
        }

        [HttpDelete]
        [Route("DeleteCategory/{id}")]
        public ActionResult<Category> Delete(int id)
        {
            try
            {
                var lstSubCategories = _subCategoryService.GetAll().Where(a => a.CategoryId == id).ToList();
                if (lstSubCategories.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "categories", Message = "This Category has Sub Categories", MessageAr = "هذا التصنيف يحتوي على تصنيفات فرعية" });
                }
                var lstMasterCategories = _masterAssetService.ListMasterAsset().Results?.Where(a => a.CategoryId == id).ToList();
                if (lstMasterCategories.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "mastercategories", Message = "This Category has related master asset", MessageAr = "هذا التصنيف يحتوي على الأصول الأساسية" });
                }
                else
                {
                    int deletedRow = _categoryService.Delete(id);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }



        [HttpGet]
        [Route("GenerateCategoryCode")]
        public GenerateCategoryCodeVM GenerateCategoryCode()
        {
            return _categoryService.GenerateCategoryCode();
        }




    }
}
