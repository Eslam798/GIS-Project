

using AutoMapper;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.AssetDetailVM;
using UPA.ViewModels.ViewModels.CityVM;
using UPA.ViewModels.ViewModels.CountOfAssetsVM;
using UPA.ViewModels.ViewModels.GovernorateVM;

namespace UPA.Web.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile() {
            CreateMap<CountOfAsset, IndexCountOfAssetsVM>().ReverseMap();


            CreateMap<AssetDetail, IndexAssetDetailVM>().ReverseMap();
            CreateMap<AssetDetail, CreateAssetDetailVM>().ReverseMap();
            CreateMap<AssetDetail, EditAssetDetailVM>().ReverseMap();



            CreateMap<Governorate, IndexGovernorateVM>().ReverseMap();
            CreateMap<Governorate, CreateGovernorateVM>().ReverseMap();
            CreateMap<Governorate, EditGovernorateVM>().ReverseMap();



           // CreateMap<City, >().ReverseMap();
            CreateMap<City, CreateCityVM>().ReverseMap();
            CreateMap<City, EditCityVM>().ReverseMap();

        }
    }
}
