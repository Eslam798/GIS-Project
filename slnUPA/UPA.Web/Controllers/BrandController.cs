using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPA.BLL.Interfaces;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.BrandVM;
using UPA.Web.Helpers;

namespace UPA.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }


        [HttpGet]
        [Route("ListBrands")]
        public IEnumerable<Brand> ListBrands()
        {
            return _brandService.ListBrands();
        }



        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditBrandVM> GetById(int id)
        {
            return _brandService.GetById(id);
        }



        [HttpPut]
        [Route("UpdateBrand")]
        public IActionResult Update(EditBrandVM BrandVM)
        {
            try
            {
                int id = BrandVM.Id;
                var lstbrandsCode = _brandService.ListBrands().ToList().Where(a => a.Code == BrandVM.Code && a.Id != id).ToList();
                if (lstbrandsCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "code", Message = "Brand code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                }
                var lstbrandsNames = _brandService.ListBrands().ToList().Where(a => a.Name == BrandVM.Name && a.Id != id).ToList();
                if (lstbrandsNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "name", Message = "Brand name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                var lstbrandsArNames = _brandService.ListBrands().ToList().Where(a => a.NameAr == BrandVM.NameAr && a.Id != id).ToList();
                if (lstbrandsArNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "name", Message = "Brand arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                else
                {
                    int updatedRow = _brandService.Update(BrandVM);
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
        [Route("AddBrand")]
        public ActionResult Add(CreateBrandVM brandVM)
        {
            if (brandVM.Code?.Length > 5)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "codelen", Message = "code must not be over 99999", MessageAr = "هذا الكود  لابد ألا يزيد عن خمس حروف أو أرقام" });

            }
            var lstbrandsCode = _brandService.ListBrands().ToList().Where(a => a.Code == brandVM.Code).ToList();
            if (lstbrandsCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "code", Message = "Brand code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstbrandsNames = _brandService.ListBrands().ToList().Where(a => a.Name == brandVM.Name).ToList();
            if (lstbrandsNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "name", Message = "Brand name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstbrandsArNames = _brandService.ListBrands().ToList().Where(a => a.NameAr == brandVM.NameAr).ToList();
            if (lstbrandsArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "name", Message = "Brand arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                var savedId = _brandService.Add(brandVM);
                var brandObj = _brandService.GetById(savedId);
                return Ok(savedId);
            }
        }

        [HttpDelete]
        [Route("DeleteBrand/{id}")]
        public ActionResult<Brand> Delete(int id)
        {
            try
            {

                int deletedRow = _brandService.Delete(id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }


        [HttpGet("GenerateBrandCode")]
        public GenerateBrandCodeVM GenerateBrandCode()
        {
            return _brandService.GenerateBrandCode();
        }


    }
}
