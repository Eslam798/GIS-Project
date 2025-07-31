using System.Collections.Generic;
using System.Threading.Tasks;
using UPA.BLL.Specifications;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.HospitalVM;

namespace UPA.BLL.Interfaces
{
    public interface IHospitalService
    {
        IEnumerable<Hospital> GetAllHospitals();
        IndexHospitalVM GetAll(CountOfAssetsSpecParams @params);
        EditHospitalVM GetById(int id);
        int Add(CreateHospitalVM Hospital);
        int Update(EditHospitalVM Hospital);
        int Delete(int id);
        public IEnumerable<IndexHospitalVM.GetData> FilterHospitalsByBrandAndGovAndOrgAndSubOrg(CountOfAssetsSpecParams @params);
        public IEnumerable<Hospital> GetHospitalByGovId(int govId);
        public List<IndexHospitalVM.GetData> GetAllLstHospitals();
        public IEnumerable<IndexHospitalVM.GetData> AutoCompleteHospitalName(string name);
        public EditHospitalVM GetHospitalByAssetId(int id);
        GenerateHospitalCodeVM GenerateHospitalCode();
    }
}
