

using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.OrganizationVM;

namespace UPA.BLL.Interfaces
{
    public interface IOrganizationService 
    {
        List<Organization> ListOrganizations();

        EditOrganizationVM GetById(int id);


        int Add(CreateOrganizationVM organization);
        int Update(EditOrganizationVM organization);
        int Delete(int id);

    }
}
