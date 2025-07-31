using System.Collections.Generic;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.ECRIVM;

namespace UPA.BLL.Interfaces
{
  public  interface IECRIService
    {
        IEnumerable<IndexECRIVM.GetData> GetAll();
        EditECRIVM GetById(int id);
        int Add(CreateECRIVM ecriObj);
        int Update(EditECRIVM ecriObj);
        int Delete(int id);

        IEnumerable<IndexECRIVM.GetData> sortECRI(SortECRIVM searchObj);
    }
}
