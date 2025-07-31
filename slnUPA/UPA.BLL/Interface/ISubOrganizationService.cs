using System.Collections.Generic;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.SubOrganizationVM;

namespace UPA.BLL.Interfaces
{
  public  interface ISubOrganizationService
    {

        IEnumerable<IndexSubOrganizationVM.GetData> ListSubOrganizations();
               IEnumerable<IndexSubOrganizationVM.GetData> GetSubOrganizationByOrgId(int orgId);

          SubOrganization GetById(int id);
        Organization GetOrganizationBySubId(int subId);
        int Add(CreateSubOrganizationVM subOrganization);
        int Update(EditSubOrganizationVM subOrganization);
        int Delete(int id);
    }
}
