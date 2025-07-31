using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPA.BLL.Interfaces;
using UPA.BLL.Specifications;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.GovernorateVM;
using UPA.ViewModels.ViewModels.HospitalVM;
using UPA.Web.Helpers;

namespace UPA.Web.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HospitalController : ControllerBase
    {
        private IHospitalService _HospitalService;
        private IAssetDetailService _assetDetailService;
      

        

        public HospitalController(IHospitalService HospitalService,IAssetDetailService assetDetailService)
        {
            _HospitalService = HospitalService;
            _assetDetailService = assetDetailService;

        }
        [HttpPost]
        [Route("FilterHospitalsByBrandAndGovAndOrgAndSubOrg")]
        public IEnumerable<IndexHospitalVM.GetData> FilterHospitalsByBrandAndGovAndOrgAndSubOrg(CountOfAssetsSpecParams @params)
        {
            return _HospitalService.FilterHospitalsByBrandAndGovAndOrgAndSubOrg(@params);
        }

        [HttpGet]
        [Route("GetAllLstHospitals")]
        public List<IndexHospitalVM.GetData> GetAllLstHospitals()
        {
            return _HospitalService.GetAllLstHospitals();
        }

        [HttpGet("GetHospitalByGovId/{govId}")]
        public IEnumerable<Hospital> GetHospitalByGovId(int govId)
        {
            return _HospitalService.GetHospitalByGovId(govId);
        }
        [HttpPost]
        [Route("ListHospitals")]
        public IndexHospitalVM GetAll(CountOfAssetsSpecParams @params)
        {
            return _HospitalService.GetAll(@params);
        }

      
        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<EditHospitalVM> GetById(int id)
        
        {
            return _HospitalService.GetById(id);
        }


        [HttpPut]
        [Route("UpdateHospital")]
        public IActionResult Update(EditHospitalVM HospitalVM)
        {
            try
            {
                int id = HospitalVM.Id;
                if (HospitalVM.Code?.Length > 5)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "codelen", Message = "code must not exceed 5 characters", MessageAr = "الكود لا يتعدى 5 حروف وأرقام" });
                }
                var lstOrgCode = _HospitalService.GetAllHospitals().ToList().Where(a => a.Code == HospitalVM.Code && a.Id != id).ToList();
                if (lstOrgCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "code", Message = "Hospital code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                }
                var lstOrgNames = _HospitalService.GetAllHospitals().ToList().Where(a => a.Name == HospitalVM.Name && a.Id != id).ToList();
                if (lstOrgNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "name", Message = "Hospital name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                else
                {
                    int updatedRow = _HospitalService.Update(HospitalVM);
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
        [Route("AddHospital")]
        public ActionResult<Hospital> Add(CreateHospitalVM HospitalVM)
        {
            
            if (HospitalVM.Code?.Length > 5)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "codelen", Message = "code must not exceed 5 characters", MessageAr = "الكود لا يتعدى 5 حروف وأرقام" });
            }
            var lstOrgCode = _HospitalService.GetAllHospitals().ToList().Where(a => a.Code == HospitalVM.Code).ToList();
            if (lstOrgCode.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "code", Message = "Hospital code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstOrgNames = _HospitalService.GetAllHospitals().ToList().Where(a => a.Name == HospitalVM.Name).ToList();
            if (lstOrgNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "name", Message = "Hospital name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                var savedId = _HospitalService.Add(HospitalVM);


                return CreatedAtAction("GetById", new { id = savedId }, HospitalVM);
            }
        }

        [HttpDelete]
        [Route("DeleteHospital/{id}")]
        public ActionResult<Hospital> Delete(int id)
        {
            try
            {
                var hospitalObj = _HospitalService.GetById(id);

                //var lstBuildings = _buildingService.GetAllBuildingsByHospitalId(id).ToList();
                //if (lstBuildings.Count > 0)
                //{
                //    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "build", Message = "You cannot delete this hospital it has related buildings", MessageAr = "لا يمكنك مسح المستشفى وذلك لارتباط مباني بها" });
                //}
                //var lstAssets = _assetDetailService.GetAllAssetDetailsByHospitalId(id).ToList();
                //if (lstAssets.Count > 0)
                //{
                //    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "asset", Message = "You cannot delete this hospital it has related assets", MessageAr = "لا يمكنك مسح المستشفى وذلك لارتباط أصول بها" });
                //}

                //var lstEmployees = _employeeService.GetEmployeesByHospitalId(id).ToList();
                //if (lstEmployees.Count > 0)
                //{
                //    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "emp", Message = "You cannot delete this hospital it has related employees", MessageAr = "لا يمكنك مسح المستشفى وذلك لارتباط موظفين بها" });
                //}

                //else
                //{
                    int deletedRow = _HospitalService.Delete(id);
               // }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                string msg = ex.Message;
                return BadRequest("Error in delete");
            }

            return Ok();
        }



        [HttpGet("GenerateHospitalCode")]
        public GenerateHospitalCodeVM GenerateHospitalCode()
        {
            return _HospitalService.GenerateHospitalCode();
        }



        [HttpGet]
        [Route("GetHospitalByAssetId/{AssetId}")]
        public ActionResult<EditHospitalVM> GetHospitalByAssetId(int AssetId)

        {
            return _HospitalService.GetHospitalByAssetId(AssetId);
        }

        [HttpGet]
        [Route("AutoCompleteHospitalName/{name}")]

        public IEnumerable<IndexHospitalVM.GetData> AutoCompleteHospitalName(string name)
        {

            return _HospitalService.AutoCompleteHospitalName(name);
        }

    }
}
