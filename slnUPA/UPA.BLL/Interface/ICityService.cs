using System.Collections.Generic;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.CityVM;

namespace UPA.BLL.Interfaces
{
  public  interface ICityService
    {

        IEnumerable<IndexCityVM.GetData> ListCities();

        IEnumerable<City> GetCities();

        City GetById(int id);
        int Add(CreateCityVM cityObj);
        int Update(EditCityVM cityObj);
        int Delete(int id);
        public IEnumerable<IndexCityVM.GetData> GetCitiesByGovernorateId(int govId);

    }
}
