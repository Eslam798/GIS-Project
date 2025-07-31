

using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.GovernorateVM;

namespace UPA.BLL.Interfaces
{
    public interface IGovernorateService
    {
        List<Governorate> ListGovernorates();

        List<IndexGovernorateVM.GetData> ListGovernoratesModel();

    }
}
