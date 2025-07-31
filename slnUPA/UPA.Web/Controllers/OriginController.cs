using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPA.BLL.Interfaces;
using UPA.BLL.Specifications;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.OriginVM;
using UPA.Web.Helpers;

namespace UPA.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OriginController : ControllerBase
    {

        private IOriginService _originService;

        public OriginController(IOriginService OriginService)
        {
            _originService = OriginService;
        }


        [HttpPost]
        [Route("ListOrigins")]
        public IndexOriginVM ListOrigins(CountOfAssetsSpecParams @params)
        {
            return _originService.ListOrigins(@params);
        }




        [HttpGet]
        [Route("GetOrigins")]
        public IEnumerable<Origin> GetOrigins()
        {
            return _originService.GetOrigins();
        }



        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditOriginVM> GetById(int id)
        {
            return _originService.GetById(id);
        }






        [HttpPut]
        [Route("UpdateOrigin")]
        public IActionResult Update(EditOriginVM OriginVM)
        {
            try
            {
                int id = OriginVM.Id;
                var lstoriginCode = _originService.GetOrigins().ToList().Where(a => a.Code == OriginVM.Code && a.Id != id).ToList();
                if (lstoriginCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "code", Message = "Origin code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                }
                var lstoriginNames = _originService.GetOrigins().ToList().Where(a => a.Name == OriginVM.Name && a.Id != id).ToList();
                if (lstoriginNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "name", Message = "Origin name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                var lstoriginArNames = _originService.GetOrigins().ToList().Where(a => a.NameAr == OriginVM.NameAr && a.Id != id).ToList();
                if (lstoriginArNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "nameAr", Message = "Origin arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }

                else
                {
                    int updatedRow = _originService.Update(OriginVM);
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
        [Route("AddOrigin")]
        public ActionResult<Origin> Add(CreateOriginVM OriginVM)
        {
            var lstoriginCode = _originService.GetOrigins().ToList().Where(a => a.Code == OriginVM.Code).ToList();
            if (lstoriginCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "code", Message = "Origin code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstoriginNames = _originService.GetOrigins().ToList().Where(a => a.Name == OriginVM.Name).ToList();
            if (lstoriginNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "name", Message = "Origin name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstoriginArNames = _originService.GetOrigins().ToList().Where(a => a.NameAr == OriginVM.NameAr).ToList();
            if (lstoriginArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "name", Message = "Origin arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                var savedId = _originService.Add(OriginVM);
                return CreatedAtAction("GetById", new { id = savedId }, OriginVM);
            }
        }

        [HttpDelete]
        [Route("DeleteOrigin/{id}")]
        public ActionResult<Origin> Delete(int id)
        {
            try
            {

                int deletedRow = _originService.Delete(id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }
    }
}
