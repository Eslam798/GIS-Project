using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UPA.BLL.Interfaces;
using UPA.BLL.Specifications;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.MasterAssetVM;
using UPA.Web.Helpers;

namespace UPA.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterAssetController : ControllerBase
    {

        private IMasterAssetService _masterAssetService;
        private IAssetDetailService _assetDetailService;
        IWebHostEnvironment _webHostEnvironment;


        public MasterAssetController(IMasterAssetService masterAssetService, IAssetDetailService assetDetailService, IWebHostEnvironment webHostEnvironment)
        {
            _masterAssetService = masterAssetService;
            _assetDetailService = assetDetailService;
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpPost]
        [Route("ListMasterAssets")]
        public IndexMasterAssetVM GetAll(CountOfAssetsSpecParams @params)
        {
            return _masterAssetService.ListMasterAsset(@params);
        }



        [HttpGet]
        [Route("ListMasterAssetsWithNofilter")]
        public IEnumerable<IndexMasterAssetVM.GetData> GetAll()
        {
            return _masterAssetService.GetAll();


        }


            [HttpGet]
        [Route("GetMasterAssetById/{id}")]
        public ActionResult<EditMasterAssetVM> GetById(int id)
        {
            return _masterAssetService.GetById(id);
        }


        [HttpGet]
        [Route("ViewMasterAsset/{id}")]
        public ActionResult<ViewMasterAssetVM> ViewMasterAsset(int id)
        {
            return _masterAssetService.ViewMasterAsset(id);
        }


        [HttpGet]
        [Route("DistinctAutoCompleteMasterAssetName/{name}")]
        public IEnumerable<MasterAsset> DistinctAutoCompleteMasterAssetName(string name)
        {
            return _masterAssetService.DistinctAutoCompleteMasterAssetName(name);
        }


        [Route("GetLastDocumentForMsterAssetId/{masterId}")]
        public MasterAssetAttachment GetLastDocumentForMsterAssetId(int masterId)
        {
            return _masterAssetService.GetLastDocumentForMsterAssetId(masterId);
        }



        [HttpPut]
        [Route("UpdateMasterAsset")]
        public IActionResult Update(EditMasterAssetVM MasterAssetVM)
        {
            try
            {
                int id = MasterAssetVM.Id;
                if (MasterAssetVM.Code != null)
                {
                    var lstCode = _masterAssetService.ListMasterAsset().Results?.Where(a => (a.Code == MasterAssetVM.Code) && a.Id != id).ToList();
                    if (lstCode?.Count() > 0)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "code", Message = "MasterAsset code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                    }
                }
                var lstNames = _masterAssetService.ListMasterAsset().Results?.ToList().Where(a => a.Name == MasterAssetVM.Name && a.ModelNumber == MasterAssetVM.ModelNumber && (a.VersionNumber == MasterAssetVM.VersionNumber && a.VersionNumber != null) && a.Id != id).ToList();
                if (lstNames?.Count() > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "name", Message = "MasterAsset name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                var lstArNames = _masterAssetService.ListMasterAsset().Results?.ToList().Where(a => a.NameAr == MasterAssetVM.NameAr && a.ModelNumber == MasterAssetVM.ModelNumber && (a.VersionNumber == MasterAssetVM.VersionNumber && a.VersionNumber != null) && a.Id != id).ToList();
                if (lstArNames?.Count() > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "nameAr", Message = "MasterAsset arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                else
                {
                    int updatedRow = _masterAssetService.Update(MasterAssetVM);

                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok(MasterAssetVM.Id);
        }


        [HttpPut]
        [Route("UpdateMasterAssetImageAfterInsert")]
        public IActionResult Update(CreateMasterAssetVM masterAssetObj)
        {
            try
            {
                int updatedRow = _masterAssetService.UpdateMasterAssetImageAfterInsert(masterAssetObj);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }

            return Ok(masterAssetObj.Id);
        }


        [HttpPost]
        [Route("AddMasterAsset")]
        public ActionResult Add(CreateMasterAssetVM MasterAssetVM)
        {
            if (MasterAssetVM.Code.Length > 5)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "codelen", Message = "code must be maximum 5  charchters", MessageAr = "هذا الكود اقصى حد  له 5 حروف وأرقام " });
            }
            if (MasterAssetVM.BrandId == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "brnd", Message = "You shold select Brand", MessageAr = "لابد من اختيار الماركة" });
            }
            var lstCode = _masterAssetService.ListMasterAsset().Results?.ToList().Where(a => a.Code == MasterAssetVM.Code).ToList();
            if (lstCode?.Count > 0)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "code", Message = "MasterAsset code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstNames = _masterAssetService.ListMasterAsset().Results?.ToList().Where(a => a.Name == MasterAssetVM.Name && a.ModelNumber == MasterAssetVM.ModelNumber && a.VersionNumber == MasterAssetVM.VersionNumber).ToList();
            if (lstNames?.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "name", Message = "MasterAsset already exist with this data", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstArNames = _masterAssetService.ListMasterAsset().Results?.ToList().Where(a => a.NameAr == MasterAssetVM.NameAr && a.ModelNumber == MasterAssetVM.ModelNumber && a.VersionNumber == MasterAssetVM.VersionNumber).ToList();
            if (lstArNames?.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "nameAr", Message = "MasterAsset arabic already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                var savedId = _masterAssetService.Add(MasterAssetVM);
                return Ok(savedId);
            }
        }

        [HttpDelete]
        [Route("DeleteMasterAsset/{id}")]
        public ActionResult<MasterAsset> Delete(int id)
        {
            try
            {
                var assetObj = _masterAssetService.GetById(id);
                //var lstHospitalAssets = _assetDetailService.ViewAllAssetDetailByMasterId(id).ToList();
                //if (lstHospitalAssets.Count > 0)
                //{
                //    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "code", Message = "MasterAsset cannot be deleted", MessageAr = "لا يمكن مسح الأصل الرئيسي" });
                //}
                //else
                //{
                int deletedRow = _masterAssetService.Delete(id);
                //  }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }


        [HttpPost]
        [Route("CreateMasterAssetAttachments")]
        public int CreateMasterAssetAttachments(MasterAssetAttachment attachObj)
        {
            return _masterAssetService.CreateMasterAssetDocuments(attachObj);
        }
        [HttpPost]
        [Route("UploadMasterAssetFiles")]
        public ActionResult UploadInFiles(IFormFile file)
        {
            var folderPath = _webHostEnvironment.ContentRootPath + "/UploadedAttachments/MasterAssets";
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


        [HttpPost]
        [Route("UploadMasterAssetImage")]
        public ActionResult UploadMasterAssetImage(IFormFile file)
        {

            var folderPath = _webHostEnvironment.ContentRootPath + "/UploadedAttachments/MasterAssets/UploadMasterAssetImage";
            bool exists = System.IO.Directory.Exists(folderPath);
            if (!exists)
                System.IO.Directory.CreateDirectory(folderPath);

            string filePath = folderPath + "/" + file.FileName;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                Stream stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);
                stream.Close();
            }
            else
            {
                Stream stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);
                stream.Close();
            }

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        [Route("GetAttachmentByMasterAssetId/{assetId}")]
        public IEnumerable<MasterAssetAttachment> GetAttachmentByMasterAssetId(int assetId)
        {
            return _masterAssetService.GetAttachmentByMasterAssetId(assetId);
        }



        [HttpDelete]
        [Route("DeleteMasterAssetAttachment/{id}")]
        public int DeleteMasterAssetAttachment(int id)
        {
            return _masterAssetService.DeleteMasterAssetAttachment(id);
        }



        [HttpDelete]
        [Route("DeleteMasterAssetImage/{id}")]
        public ActionResult DeleteMasterAssetImage(int id)
        {
            var masterAssetObj = _masterAssetService.GetById(id);
            var folderPath = _webHostEnvironment.ContentRootPath + "/UploadedAttachments/MasterAssets/UploadMasterAssetImage/" + masterAssetObj.AssetImg;
            bool exists = System.IO.File.Exists(folderPath);
            if (exists)
            {
                System.IO.File.Delete(folderPath);
                masterAssetObj.AssetImg = "";
                _masterAssetService.Update(masterAssetObj);
            }
            return Ok();
        }






        [HttpGet]
        [Route("GenerateMasterAssetCode")]
        public GeneratedMasterAssetCodeVM GenerateMasterAssetCode()
        {
            return _masterAssetService.GenerateMasterAssetCode();
        }

        [HttpGet]
        [Route("AutoCompleteMasterAssetName/{name}")]
        public IEnumerable<IndexMasterAssetVM.GetData> AutoCompleteMasterAssetName(string name)
        {

            return _masterAssetService.AutoCompleteMasterAssetName(name);
        }

    }
}
