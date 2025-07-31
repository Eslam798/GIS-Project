using Asset.ViewModels.AssetDetailAttachmentVM;
using Asset.ViewModels.AssetDetailVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPA.BLL.Specifications;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.AssetDetailVM;
using UPA.ViewModels.ViewModels.BrandVM;
using UPA.ViewModels.ViewModels.CountOfAssetsVM;
using UPA.ViewModels.ViewModels.MasterAssetVM;

namespace UPA.BLL.Interfaces
{
    public interface IAssetDetailService
    {
        public IndexAssetDetailVM SortAssetsWithoutSearch(Sort sortObj, int pageNumber, int pageSize);

        IndexAssetDetailVM GetHospitalAssetsByGovIdAndDeptIdAndHospitalId(int departmentId, int govId, int hospitalId, int pageNumber, int pageSize);
        public IndexAssetDetailVM GetAssetsByUserIdAndPaging(int pageNumber, int pageSize);
        public IndexAssetDetailVM SortAssetDetailAfterSearch(SortAndFilterDataModel data, int pageNumber, int pageSize);
        public IndexAssetDetailVM GetAssetsByBrandId(int brandId);
        public IndexAssetDetailVM FilterDataByDepartmentBrandSupplierIdAndPaging(FilterHospitalAsset data, int pageNumber, int pageSize);
        public IndexAssetDetailVM SortAssetDetail(SortAssetDetail sortObject, int pageNumber, int pageSize);
        public IndexAssetDetailVM GetAll(CountOfAssetsSpecParams @params);
        IEnumerable<IndexAssetDetailVM.GetData> GetAll();
        EditAssetDetailVM GetById(int id);
        int Add(CreateAssetDetailVM assetDetailObj);
        int Update(EditAssetDetailVM assetDetailObj);
        int Delete(int id);
        ViewAssetDetailVM ViewAssetDetailById(int id);
        int CreateAssetDetailDocuments(CreateAssetDetailAttachmentVM attachObj);
        IEnumerable<AssetDetailAttachment> GetAttachmentByAssetDetailId(int assetId);
        int DeleteAssetDetailAttachment(int id);
        AssetDetailAttachment GetLastDocumentForAssetDetailId(int assetDetailId);

        public List<BrandGroupVM> GroupAssetDetailsByBrand(FilterHospitalAsset data);


        public GeneratedAssetDetailBCVM GenerateAssetDetailBarcode();


          public IEnumerable<IndexCountOfAssetsVM.Data> PyramidGovernorateChart();

        public IEnumerable<IndexCountOfAssetsVM.Data> PyramidGovernorateChartByParams(CountOfAssetsSpecParams @params);

        public IEnumerable<IndexCountOfAssetsVM.Data> PyramidGovernoratePopulationChartByParams(CountOfAssetsSpecParams @params);


        public List<BrandGroupVM> GroupAssetDetailsByGovernorate(FilterHospitalAsset data);
        public IEnumerable<ChartCountOfAssetDetailVM> GetAssetsCountByOrganizationsAndCategories(CountOfAssetsSpecParams @params);

        public IndexAssetDetailVM GetAssetsByGovId(int govId);
        public IndexAssetDetailVM GetAssetsByHosId(int govId);
        public IndexAssetDetailVM GetAssetsByOrgId(int govId);
        public IndexAssetDetailVM GetAssetsBySubOrgId(int govId);
        public List<BrandGroupVM> GroupAssetDetailsByOrganization(FilterHospitalAsset data);
        public List<BrandGroupVM> GroupAssetDetailsBySubOrganization(FilterHospitalAsset data);
        public List<BrandGroupVM> GroupAssetDetailsByHospital(FilterHospitalAsset data);
       
    }
}
