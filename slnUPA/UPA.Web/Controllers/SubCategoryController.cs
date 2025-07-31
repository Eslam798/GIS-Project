using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPA.BLL.Interfaces;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.CategoryVM;
using UPA.ViewModels.ViewModels.SubCategoryVM;
using UPA.Web.Helpers;

namespace UPA.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {

        private ISubCategoryService _subCategoryService;
        private IMasterAssetService _masterAssetService;

        public SubCategoryController(ISubCategoryService SubCategoryService, IMasterAssetService masterAssetService)
        {
            _subCategoryService = SubCategoryService;
            _masterAssetService = masterAssetService;
        }


        [HttpGet]
        [Route("ListSubCategories")]
        public IEnumerable<IndexSubCategoryVM.GetData> GetAll()
        {
            return _subCategoryService.GetAll();
        }






        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditSubCategoryVM> GetById(int id)
        {
            return _subCategoryService.GetById(id);
        }


        [HttpGet]
        [Route("GetSubCategoryByCategoryId/{categoryId}")]
        public ActionResult<IEnumerable<SubCategory>> GetSubCategoryByCategoryId(int categoryId)
        {
            return _subCategoryService.GetSubCategoryByCategoryId(categoryId).ToList();
        }




        [HttpPut]
        [Route("UpdateSubCategory")]
        public IActionResult Update(EditSubCategoryVM SubCategoryVM)
        {
            try
            {
                int id = SubCategoryVM.Id;
                var lstsubCategoriesCode = _subCategoryService.GetAll().ToList().Where(a => a.Code == SubCategoryVM.Code && a.Id != id).ToList();
                if (lstsubCategoriesCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "code", Message = "SubCategory code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                }
                var lstsubCategoriesNames = _subCategoryService.GetAll().ToList().Where(a => a.Name == SubCategoryVM.Name && a.Id != id).ToList();
                if (lstsubCategoriesNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "name", Message = "SubCategory name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                var lstsubCategoriesArNames = _subCategoryService.GetAll().ToList().Where(a => a.NameAr == SubCategoryVM.NameAr && a.Id != id).ToList();
                if (lstsubCategoriesArNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "name", Message = "SubCategory arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }

                else
                {
                    int updatedRow = _subCategoryService.Update(SubCategoryVM);
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
        [Route("AddSubCategory")]
        public ActionResult<SubCategory> Add(CreateSubCategoryVM SubCategoryVM)
        {
            var lstOrgCode = _subCategoryService.GetAll().ToList().Where(a => a.Code == SubCategoryVM.Code).ToList();
            if (lstOrgCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "code", Message = "SubCategory code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstOrgNames = _subCategoryService.GetAll().ToList().Where(a => a.Name == SubCategoryVM.Name).ToList();
            if (lstOrgNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "name", Message = "SubCategory name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstsubCategoriesArNames = _subCategoryService.GetAll().ToList().Where(a => a.NameAr == SubCategoryVM.NameAr).ToList();
            if (lstsubCategoriesArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "name", Message = "SubCategory arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
                {
                var savedId = _subCategoryService.Add(SubCategoryVM);
                return CreatedAtAction("GetById", new { id = savedId }, SubCategoryVM);
            }
        }

        [HttpDelete]
        [Route("DeleteSubCategory/{id}")]
        public ActionResult<SubCategory> Delete(int id)
        {
            try
            {
                var lstMasterCategories = _masterAssetService.ListMasterAsset().Results?.Where(a => a.SubCategoryId == id).ToList();
                if (lstMasterCategories.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "mastersubcategories", Message = "This Sub Category has related master asset", MessageAr = "هذا التصنيف الفرعي يحتوي على الأصول الأساسية" });
                }
                else
                {
                    int deletedRow = _subCategoryService.Delete(id);
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
        [Route("GenerateSubCategoryCode")]
        public GenerateSubCategoryCodeVM GenerateSubCategoryCode()
        {
            return _subCategoryService.GenerateSubCategoryCode();
        }
    }
}
