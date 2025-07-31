
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UPA.BLL.Interfaces;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.SupplierVM;
using UPA.Web.Helpers;

namespace UPA.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private IAssetDetailService _assetDetailService;
        private ISupplierService _SupplierService;
        IWebHostEnvironment _webHostingEnvironment;

        public SupplierController(ISupplierService SupplierService, IAssetDetailService assetDetailService,
           IWebHostEnvironment webHostingEnvironment)
        {
            _assetDetailService = assetDetailService;
            _SupplierService = SupplierService;
            _webHostingEnvironment = webHostingEnvironment;
        }


        [HttpGet]
        [Route("ListSuppliers")]
        public IEnumerable<IndexSupplierVM.GetData> GetAll()
        {
            return _SupplierService.ListSuppliers();
        }










        [HttpGet]
        [Route("CountSuppliers")]
        public int CountSuppliers()
        {
            return _SupplierService.CountSuppliers();
        }




        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditSupplierVM> GetById(int id)
        {
            return _SupplierService.GetById(id);
        }



        [HttpPut]
        [Route("UpdateSupplier")]
        public IActionResult Update(EditSupplierVM SupplierVM)
        {
            try
            {
                int id = SupplierVM.Id;
                var lstCitiesCode = _SupplierService.ListSuppliers().ToList().Where(a => a.Code == SupplierVM.Code && a.Id != id).ToList();
                if (lstCitiesCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "code", Message = "Supplier code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                }
                var lstCitiesNames = _SupplierService.ListSuppliers().ToList().Where(a => a.Name == SupplierVM.Name && a.Id != id).ToList();
                if (lstCitiesNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "name", Message = "Supplier name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                var lstCitiesArNames = _SupplierService.ListSuppliers().ToList().Where(a => a.NameAr == SupplierVM.NameAr && a.Id != id).ToList();
                if (lstCitiesArNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "name", Message = "Supplier arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }

                else
                {
                    int updatedRow = _SupplierService.Update(SupplierVM);
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
        [Route("AddSupplier")]
        public ActionResult Add(CreateSupplierVM SupplierVM)
        {
            if (SupplierVM.Code.Length > 5)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "codelen", Message = "Supplier code must not exceed 5 characters", MessageAr = "هذا الكود لا يتعدي 5 حروف وأرقام" });

            }
            var lstOrgCode = _SupplierService.ListSuppliers().ToList().Where(a => a.Code == SupplierVM.Code).ToList();
            if (lstOrgCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "code", Message = "Supplier code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstOrgNames = _SupplierService.ListSuppliers().ToList().Where(a => a.Name == SupplierVM.Name).ToList();
            if (lstOrgNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "name", Message = "Supplier name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstCitiesArNames = _SupplierService.ListSuppliers().ToList().Where(a => a.NameAr == SupplierVM.NameAr).ToList();
            if (lstCitiesArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "nameAr", Message = "Supplier arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                var savedId = _SupplierService.Add(SupplierVM);
                return Ok(savedId);// CreatedAtAction("GetById", new { id = savedId }, SupplierVM);
            }
        }


        [HttpPost]
        [Route("CreateSupplierAttachment")]
        public int CreateRequestAttachments(SupplierAttachment attachObj)
        {
            return _SupplierService.CreateSupplierAttachment(attachObj);
        }


        [HttpGet]
        [Route("GetSupplierAttachmentsBySupplierId/{supplierId}")]
        public List<SupplierAttachment> GetSupplierAttachmentsBySupplierId(int supplierId)
        {
            return _SupplierService.GetSupplierAttachmentsBySupplierId(supplierId);
        }



        [HttpPost]
        [Route("UploadSupplierFile")]
        public ActionResult UploadSupplierFile(IFormFile file)
        {
            var folderPath = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/SupplierAttachments";
            bool exists = System.IO.Directory.Exists(folderPath);
            if (!exists)
                System.IO.Directory.CreateDirectory(folderPath);
            string filePath = folderPath + "/" + file.FileName;
            if (System.IO.File.Exists(filePath))
            {
            }
            else
            {
                Stream stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);
                stream.Close();
            }
            return StatusCode(StatusCodes.Status201Created);
        }







        [HttpDelete]
        [Route("DeleteSupplier/{id}")]
        public ActionResult<Supplier> Delete(int id)
        {
            try
            {
                var supplierObj = _SupplierService.GetById(id);
                //var lstHospitalAssets = _assetDetailService.GetAll().Where(a => a.SupplierId == supplierObj.Id).ToList();
                //if (lstHospitalAssets.Count > 0)
                //{
                //    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "hostassets", Message = "Hospital Assets has this supplier", MessageAr = "أصول المستشفى بها منتجات من هذا المورد" });
                //}
                //var lstMasterContracts = _masterContractService.GetAll().Where(a => a.SupplierId == supplierObj.Id).ToList();
                //if (lstMasterContracts.Count > 0)
                //{
                //    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "contract", Message = "Contract has this supplier", MessageAr = "العقد به هذا المورد" });
                //}
                //else
                //{
                    int deletedRow = _SupplierService.Delete(id);
              //  }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }




        [HttpGet("GenerateSupplierCode")]
        public GenerateSupplierCodeVM GenerateSupplierCode()
        {
            return _SupplierService.GenerateSupplierCode();
        }


    }
}
