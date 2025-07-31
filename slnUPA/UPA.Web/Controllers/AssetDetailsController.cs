using Asset.ViewModels.AssetDetailAttachmentVM;
using Asset.ViewModels.AssetDetailVM;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPA.BLL.Interfaces;
using UPA.BLL.Repository;
using UPA.BLL.Specifications;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.AssetDetailVM;
using UPA.ViewModels.ViewModels.BrandVM;
using UPA.ViewModels.ViewModels.CountOfAssetsVM;
using UPA.Web.Helpers;

namespace UPA.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetDetailsController : ControllerBase
    {
        private readonly IAssetDetailService _assetDetailService;
        IWebHostEnvironment _webHostingEnvironment;
        public AssetDetailsController(IAssetDetailService assetDetailService, IWebHostEnvironment webHostingEnvironment)
        {
            _assetDetailService = assetDetailService;
            _webHostingEnvironment = webHostingEnvironment;
        }

        [HttpPost("GetAllAssets")]
        public IndexAssetDetailVM GetAll(CountOfAssetsSpecParams countOfAssetsSpecParams)
        {
            return _assetDetailService.GetAll(countOfAssetsSpecParams);
        }

        [HttpPost]
        [Route("FilterDataByDepartmentBrandSupplierIdAndPaging/{pageNumber}/{pageSize}")]
        public IndexAssetDetailVM FilterDataByDepartmentBrandSupplierIdAndPaging(FilterHospitalAsset data, int pageNumber, int pageSize)
         {
         return    _assetDetailService.FilterDataByDepartmentBrandSupplierIdAndPaging(data, pageNumber, pageSize);
         
        }


        [HttpGet]
        [Route("GetAssetsByBrandId/{brandId}")]
        public IndexAssetDetailVM GetAssetsByBrandId(int brandId)
        {
            IndexAssetDetailVM result = new IndexAssetDetailVM();
            result = _assetDetailService.GetAssetsByBrandId(brandId);
            return result;
        }
        [HttpGet]
        [Route("GetAssetsByGovId/{brandId}")]
        public IndexAssetDetailVM GetAssetsByGovId(int brandId)
        {
            IndexAssetDetailVM result = new IndexAssetDetailVM();
            result = _assetDetailService.GetAssetsByGovId(brandId);
            return result;
        }
        [HttpGet]
        [Route("GetAssetsByHosId/{brandId}")]
        public IndexAssetDetailVM GetAssetsByHosId(int brandId)
        {
            IndexAssetDetailVM result = new IndexAssetDetailVM();
            result = _assetDetailService.GetAssetsByHosId(brandId);
            return result;
        }
        [HttpGet]
        [Route("GetAssetsBySubOrgId/{brandId}")]
        public IndexAssetDetailVM GetAssetsBySubOrgId(int brandId)
        {
            IndexAssetDetailVM result = new IndexAssetDetailVM();
            result = _assetDetailService.GetAssetsBySubOrgId(brandId);
            return result;
        }
        [HttpGet]
        [Route("GetAssetsByOrgId/{brandId}")]
        public IndexAssetDetailVM GetAssetsByOrgId(int brandId)
        {
            IndexAssetDetailVM result = new IndexAssetDetailVM();
            result = _assetDetailService.GetAssetsByOrgId(brandId);
            return result;
        }
        [HttpPost]
        [Route("SortAssetDetailAfterSearch/{pageNumber}/{pageSize}")]
        public IndexAssetDetailVM SortAssetDetailAfterSearch(SortAndFilterDataModel data, int pageNumber, int pageSize)
        {
            return _assetDetailService.SortAssetDetailAfterSearch(data, pageNumber, pageSize);
        }
        [HttpPost]
        [Route("SortAssetDetail/{pageNumber}/{pageSize}")]
        public IndexAssetDetailVM SortAssetDetail(SortAssetDetail sortObject, int pageNumber, int pageSize)
        {
            var result = new IndexAssetDetailVM();
            result = _assetDetailService.SortAssetDetail(sortObject, pageNumber, pageSize);
            return result;
        }

        [HttpPost]
        [Route("GetAssetsByUserIdAndPaging/{pageNumber}/{pageSize}")]
        public IndexAssetDetailVM GetAssetsByUserIdAndPaging( int pageNumber, int pageSize)
        {
            // return _AssetDetailService.GetAssetsByUserId(userId, pageNumber, pageSize);
            return _assetDetailService.GetAssetsByUserIdAndPaging( pageNumber, pageSize);
        }

        [HttpPut]
        [Route("UpdateAssetDetail")]
        public IActionResult Update(EditAssetDetailVM AssetDetailVM)
        {
            try
            {
                //a.BarCode == AssetDetailVM.Barcode && a.SerialNumber == AssetDetailVM.SerialNumber
                int id = AssetDetailVM.Id;
                if (!string.IsNullOrEmpty(AssetDetailVM.Code))
                {
                    var lstCode = _assetDetailService.GetAll().Where(a => a.Code == AssetDetailVM.Code && a.Id != id).ToList();
                    if (lstCode.Count > 0)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "code", Message = "Asset code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                    }
                }
                var lstNames = _assetDetailService.GetAll().ToList().Where(a => a.BarCode == AssetDetailVM.BarCode && a.Serial == AssetDetailVM.SerialNumber && a.Id != id).ToList();
                if (lstNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "serial", Message = "Asset serial already exist", MessageAr = "هذا السيريال مسجل سابقاً" });
                }

                else
                {

                    int updatedRow = _assetDetailService.Update(AssetDetailVM);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in update");
            }
            return Ok();
        }




        [HttpDelete]
        [Route("DeleteAssetDetail/{id}")]
        public void Delete(int id)
        {

            _assetDetailService.Delete(id);
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public EditAssetDetailVM GetById(int id)
        {
            return _assetDetailService.GetById(id);

        }
        [HttpDelete]
        [Route("DeleteAssetDetailAttachment/{id}")]
        public int DeleteAssetDetailAttachment(int id)
        {
            return _assetDetailService.DeleteAssetDetailAttachment(id);
        }


        [HttpPost]
        [Route("UploadAssetDetailFiles")]
        public ActionResult UploadInFiles(IFormFile file)
        {
            string path = _webHostingEnvironment.ContentRootPath + "/UploadedAttachments/AssetDetails/" + file.FileName;
            if (!System.IO.File.Exists(path))
            {
                Stream stream = new FileStream(path, FileMode.Create);
                file.CopyTo(stream);
                stream.Close();
            }
            return StatusCode(StatusCodes.Status201Created);
        }


        [HttpGet]
        [Route("GetHospitalByAssetId/{id}")]
        public EditAssetDetailVM GetHospitalByAssetId(int id)
        {
            return _assetDetailService.GetById(id);

        }
        [HttpGet]
        [Route("GetAttachmentByAssetDetailId/{assetId}")]
        public IEnumerable<AssetDetailAttachment> GetAttachmentByAssetDetailId(int assetId)
        {
            return _assetDetailService.GetAttachmentByAssetDetailId(assetId);
        }
        [HttpPost]
        [Route("CreateAssetDetailAttachments")]
        public int CreateAssetDetailAttachments(CreateAssetDetailAttachmentVM attachObj)
        {
            return _assetDetailService.CreateAssetDetailDocuments(attachObj);
        }

        [Route("GetLastDocumentForAssetDetailId/{assetDetailId}")]
        public AssetDetailAttachment GetLastDocumentForWorkOrderTrackingId(int assetDetailId)
        {
            return _assetDetailService.GetLastDocumentForAssetDetailId(assetDetailId);
        }


        [HttpPost]
        [Route("AddAssetDetail")]
        public ActionResult Add(CreateAssetDetailVM AssetDetailVM)
        {
            var lstCode = _assetDetailService.GetAll().ToList().Where(a => a.Code == AssetDetailVM.Code).ToList();
            if (lstCode.Count > 0)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "code", Message = "Asset code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstNames = _assetDetailService.GetAll().ToList().Where(a => a.BarCode == AssetDetailVM.Barcode && a.Serial == AssetDetailVM.SerialNumber).ToList();
            if (lstNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "name", Message = "Asset already exist with this data", MessageAr = "هذا الجهاز مسجل سابقاً" });
            }
            else
            {
                var savedId = _assetDetailService.Add(AssetDetailVM);

                CreateAssetDetailAttachmentVM qrAttach = new CreateAssetDetailAttachmentVM();
                qrAttach.AssetDetailId = savedId;
                qrAttach.FileName = "asset-" + AssetDetailVM.Id + ".png";
                CreateAssetDetailAttachments(qrAttach);
                return Ok(savedId);
            }
        }


        [HttpGet]
        [Route("GenerateAssetDetailBarcode")]
        public GeneratedAssetDetailBCVM GenerateAssetDetailBarcode()
        {
            return _assetDetailService.GenerateAssetDetailBarcode();
        }

        [HttpGet]
        [Route("ViewAssetDetailById/{id}")]
        public ActionResult<ViewAssetDetailVM> ViewAssetDetailById(int id)
        {
            return _assetDetailService.ViewAssetDetailById(id);
        }


        [HttpPost]
        [Route("GroupAssetDetailsByBrand")]
        public List<BrandGroupVM> GroupAssetDetailsByBrand(FilterHospitalAsset data)
        {
            return _assetDetailService.GroupAssetDetailsByBrand(data);
        }

        [HttpPost]
        [Route("GroupAssetDetailsByGovernorate")]
        public List<BrandGroupVM> GroupAssetDetailsByGovernorate(FilterHospitalAsset data)
        {
            return _assetDetailService.GroupAssetDetailsByGovernorate(data);
        }
         [HttpPost]
        [Route("GroupAssetDetailsByOrganization")]
        public List<BrandGroupVM> GroupAssetDetailsByOrganization(FilterHospitalAsset data)
        {
            return _assetDetailService.GroupAssetDetailsByOrganization(data);
        }
        [HttpPost]
        [Route("GroupAssetDetailsBySubOrganization")]
        public List<BrandGroupVM> GroupAssetDetailsBySubOrganization(FilterHospitalAsset data)
        {
            return _assetDetailService.GroupAssetDetailsBySubOrganization(data);
        }
        [HttpPost]
        [Route("GroupAssetDetailsByHospital")]
        public List<BrandGroupVM> GroupAssetDetailsByHospital(FilterHospitalAsset data)
        {
            return _assetDetailService.GroupAssetDetailsByHospital(data);
        }
        [Route("PyramidGovernorateChart")]
        public IEnumerable<IndexCountOfAssetsVM.Data> PyramidGovernorateChart()
        {
            return _assetDetailService.PyramidGovernorateChart();
        }


        [HttpPost]
        [Route("GetHospitalAssetsByGovIdAndDeptIdAndHospitalId/{departmentId}/{govId}/{hospitalId}/{pageNumber}/{pageSize}")]
        public ActionResult<IndexAssetDetailVM> GetHospitalAssetsByGovIdAndDeptIdAndHospitalId2(int departmentId, int govId, int hospitalId, int pageNumber, int pageSize)
        {
            return _assetDetailService.GetHospitalAssetsByGovIdAndDeptIdAndHospitalId(departmentId, govId, hospitalId, pageNumber, pageSize);
        }
        [HttpPost]
        [Route("SortAssetsWithoutSearch/{pageNumber}/{pageSize}")]
        public IndexAssetDetailVM SortAssetsWithoutSearch(Sort sortObj, int pageNumber, int pageSize)
        {
            return _assetDetailService.SortAssetsWithoutSearch(sortObj, pageNumber, pageSize);
        }
        [HttpPost]
        [Route("PyramidGovernorateChartByParams")]
        public IEnumerable<IndexCountOfAssetsVM.Data> PyramidGovernorateChartByParams(CountOfAssetsSpecParams @params)
        {
            return _assetDetailService.PyramidGovernorateChartByParams(@params);
        }



        [HttpPost]
        [Route("PyramidGovernoratePopulationChartByParams")]
        public IEnumerable<IndexCountOfAssetsVM.Data> PyramidGovernoratePopulationChartByParams(CountOfAssetsSpecParams @params)
        {
            return _assetDetailService.PyramidGovernoratePopulationChartByParams(@params);
        }

        [HttpPost]
        [Route("GetAssetsCountByOrganizationsAndCategories")]
        public IEnumerable<ChartCountOfAssetDetailVM> GetAssetsCountByOrganizationsAndCategories(CountOfAssetsSpecParams @params)
        {
            return _assetDetailService.GetAssetsCountByOrganizationsAndCategories(@params);
        }




    }
}
