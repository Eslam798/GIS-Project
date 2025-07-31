using Azure;
using Microsoft.AspNetCore.Mvc;
using System;
using UPA.BLL.Interfaces;
using UPA.Web.Helpers;
using UPA.ViewModels.ViewModels.CountOfAssetsVM;
using AutoMapper;
using UPA.DAL.Models;
using UPA.BLL.Repository;
using Talabat.API.Helpers;
using UPA.BLL.Specifications;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace UPA.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountOfAssetController : ControllerBase
    {
        private readonly ICountOfAssetService _countOfAssetService;
        private readonly IGenericRepo<CountOfAsset> _genericRepo;
        private readonly IMapper _mapper;
        public CountOfAssetController(ICountOfAssetService countOfAssetService, IGenericRepo<CountOfAsset> genericRepo, IMapper mapper)
        {
            _countOfAssetService = countOfAssetService;
            _genericRepo = genericRepo;
            _mapper = mapper;
        }


        [HttpPost]
        [Route("CreateCountOfAssets")]
        public int CreateCountOfAsset(List<CreateCountOfAssetsVM> model)
        {
            return _countOfAssetService.CreateCountOfAsset(model);
        }

        [HttpPost]
        [Route("SaveRecordCountOfAssets")]
        public IActionResult SaveRecordCountOfAssets(CreateCountOfAssetsVM model)
        {
            var lstCountOfAssets = _countOfAssetService.ListCountOfAsset().ToList()
                .Where(a => a.GovernorateId == model.GovernorateId && a.BrandId == model.BrandId && a.OrganizationId == model.OrganizationId && a.CategoryId == model.CategoryId).ToList();
            if (lstCountOfAssets.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "asset", Message = "This item already exist", MessageAr = "هذاالعنصر مسجل سابقاً" });
            }
            else
            {
                var saved = _countOfAssetService.CreateRecordOfCountOfAsset(model);
                return Ok(saved);
            }
        }


        [HttpPost("FilterCountOfAssets")]
        public ActionResult<IndexCountOfAssetsVM> FilterCountOfAssets(CountOfAssetsSpecParams productParams)
        {
            var data = _countOfAssetService.GetCountOfAssetWithfilter(productParams);
            return Ok(data);
        }



        [HttpPost("FilterCountOfAssetByOrganizationGovernorate")]
        public ActionResult<IndexCountOfAssetsVM.Data> FilterCountOfAssetByOrganizationGovernorate(CountOfAssetsSpecParams productParams)
        {
            var data = _countOfAssetService.FilterCountOfAssetByOrganizationGovernorate(productParams);
            return Ok(data);
        }




        [HttpGet]
        [Route("GetCountOfAssetById/{id}")]
        public EditCountOfAssetsVM GetCountOfAssetById(int id)
        {
            return _countOfAssetService.GetCountOfAssetById(id);
        }


        [HttpPost]
        [Route("GetCountOfAssetByCategoryGovernorate")]
        public IEnumerable<IndexCountOfAssetsVM.Data> GetCountOfAssetByCategoryGovernorate(CountOfAssetsSpecParams productParams)
        {
            return _countOfAssetService.GetCountOfAssetByCategoryGovernorate( productParams);
        }


        [HttpGet]
        [Route("GetCountOfAssetByOrganizationGovernorate")]
        public IEnumerable<IndexCountOfAssetsVM.Data> GetCountOfAssetByOrganizationGovernorate()
        {
            return _countOfAssetService.GetCountOfAssetByOrganizationGovernorate();
        }


        [HttpGet]
        [Route("CategoryPivotTable")]
        public async Task<ActionResult<Pagination<IndexCountOfAssetsVM>>> CategoryPivotTable([FromQuery] CountOfAssetsSpecParams productParams)
        {
            var spec = new CountOfAssetsWithGovernatesAndCategoriesAndBrandsAndOrganizationSpecification(productParams);
            var countCategoryPivotTable = new ProductWithFiltersForCountSpecification(productParams);
            var totalItems = await _genericRepo.GetCountAsync(countCategoryPivotTable);
            var lstCategoryPivotTable = await _genericRepo.GetAllWithSpecAsync(spec);
            var data = _mapper.Map<IReadOnlyList<CountOfAsset>, IReadOnlyList<IndexCountOfAssetsVM>>(lstCategoryPivotTable);
            return Ok(new Pagination<IndexCountOfAssetsVM>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }





        [HttpPut]
        [Route("UpdateCountOfAsset")]
        public IActionResult UpdateCountOfAsset(EditCountOfAssetsVM model)
        {
            var lstCountOfAssets = _countOfAssetService.ListCountOfAsset().ToList()
                .Where(a => a.Id != model.Id && a.GovernorateId == model.GovernorateId && a.BrandId == model.BrandId && a.OrganizationId == model.OrganizationId && a.CategoryId == model.CategoryId).ToList();
            if (lstCountOfAssets.Count > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse { Status = "asset", Message = "This item already exist", MessageAr = "هذاالعنصر مسجل سابقاً" });
            }
            else
            {
                var saved = _countOfAssetService.UpdateCountOfAsset(model);
                return Ok(saved);
            }
        }


        [HttpDelete]
        [Route("DeleteCountOfAsset/{id}")]
        public int DeleteCountOfAsset(int id)
        {
            return _countOfAssetService.DeleteCountOfAsset(id);
        }


    }
}
