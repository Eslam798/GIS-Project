using System.Collections.Generic;
using System.Linq;
using UPA.BLL.Interfaces;
using UPA.DAl;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.GovernorateVM;

namespace UPA.BLL.Repository
{
    public class GovernorateRepository : IGovernorateService
    {

        private readonly UPAModel _db;

        public GovernorateRepository(UPAModel db)
        {
            // Log.Debug("[GovernorateService]");
            _db = db;
        }

        public List<Governorate> ListGovernorates()
        {
            var result= _db.Governorates.OrderBy(a => a.Code).ToList();
            return result;
        }

        public List<IndexGovernorateVM.GetData> ListGovernoratesModel()
        {
            return _db.Governorates.ToList().Select(item => new IndexGovernorateVM.GetData
            {
                Id = item.Id,
                GovernorateName = item.Name,
                GovernorateNameAr = item.NameAr,
                Logo= item.Logo   
            }).ToList();
        }
    }
}
