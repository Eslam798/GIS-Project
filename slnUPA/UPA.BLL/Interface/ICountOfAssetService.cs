using UPA.BLL.Specifications;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.CountOfAssetsVM;

namespace UPA.BLL.Interfaces
{
    public interface ICountOfAssetService
    {
        public int CreateCountOfAsset(List<CreateCountOfAssetsVM> model);
        public int CreateRecordOfCountOfAsset(CreateCountOfAssetsVM model);
        public List<CountOfAsset> ListCountOfAsset();
        public EditCountOfAssetsVM GetCountOfAssetById(int id);
        public int UpdateCountOfAsset(EditCountOfAssetsVM model);

        public int DeleteCountOfAsset(int id);
        public IEnumerable<IndexCountOfAssetsVM.Data> GetCountOfAssetByCategoryGovernorate(CountOfAssetsSpecParams productParams);
        public IEnumerable<IndexCountOfAssetsVM.Data> GetCountOfAssetByOrganizationGovernorate();


        public IndexCountOfAssetsVM GetCountOfAssetWithfilter(CountOfAssetsSpecParams @params);
        public IEnumerable<IndexCountOfAssetsVM.Data> FilterCountOfAssetByOrganizationGovernorate(CountOfAssetsSpecParams @params);


    

    }
}
