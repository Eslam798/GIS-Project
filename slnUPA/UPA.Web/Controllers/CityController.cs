using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UPA.BLL.Interfaces;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.CityVM;
using UPA.Web.Helpers;

namespace UPA.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {

        private ICityService _cityService;
        private IHospitalService _hospitalService;


        public CityController(ICityService cityService, IHospitalService hospitalService)
        {
            _cityService = cityService;
            _hospitalService = hospitalService;
        }


        [HttpGet]
        [Route("ListCities")]
        public IEnumerable<IndexCityVM.GetData> GetAll()
        {
            return _cityService.ListCities();
        }


        [HttpGet]
        [Route("GetCitiesByGovernorateId/{govId}")]
        public IEnumerable<IndexCityVM.GetData> GetCitiesByGovernorateId(int govId)
        {
            return _cityService.GetCitiesByGovernorateId(govId);
        }






        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<City> GetById(int id)
        {
            return _cityService.GetById(id);
        }



        [HttpPut]
        [Route("UpdateCity")]
        public IActionResult Update(EditCityVM cityVM)
        {
            try
            {
                int id = cityVM.Id;
                var lstCitiesCode = _cityService.GetCities().ToList().Where(a => a.Code == cityVM.Code && a.Id != id).ToList();
                if (lstCitiesCode.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "code", Message = "City code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
                }
                var lstCitiesNames = _cityService.GetCities().ToList().Where(a => a.Name == cityVM.Name && a.Id != id).ToList();
                if (lstCitiesNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "name", Message = "City name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }
                var lstCitiesArNames = _cityService.GetCities().ToList().Where(a => a.NameAr == cityVM.NameAr && a.Id != id).ToList();
                if (lstCitiesArNames.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "name", Message = "City arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
                }

                else
                {
                    int updatedRow = _cityService.Update(cityVM);
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
        [Route("AddCity")]
        public ActionResult<City> Add(CreateCityVM cityVM)
        {
            var lstOrgCode = _cityService.GetCities().ToList().Where(a => a.Code == cityVM.Code && a.GovernorateId != cityVM.GovernorateId).ToList();
            if (lstOrgCode.Count() > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "code", Message = "City code already exist", MessageAr = "هذا الكود مسجل سابقاً" });
            }
            var lstOrgNames = _cityService.GetCities().ToList().Where(a => a.Name == cityVM.Name).ToList();
            if (lstOrgNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "name", Message = "City name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            var lstCitiesArNames = _cityService.GetCities().ToList().Where(a => a.NameAr == cityVM.NameAr).ToList();
            if (lstCitiesArNames.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "name", Message = "City arabic name already exist", MessageAr = "هذا الاسم مسجل سابقاً" });
            }
            else
            {
                var savedId = _cityService.Add(cityVM);
                return CreatedAtAction("GetById", new { id = savedId }, cityVM);
            }
        }

        [HttpDelete]
        [Route("DeleteCity/{id}")]
        public ActionResult<City> Delete(int id)
        {
            try
            {
                var cityObj = _cityService.GetById(id);
                var lstHospitals = _hospitalService.GetAllHospitals().Where(a=>a.CityId == id).ToList();
                if (lstHospitals.Count > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "hospital", Message = "You cannot delete this city it has related hospitals", MessageAr = "لا يمكنك مسح المدينة وذلك لارتباط المستشفيات بها" });
                }
                else
                {
                    int deletedRow = _cityService.Delete(id);
                }
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
