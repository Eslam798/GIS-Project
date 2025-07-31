using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPA.BLL.Specifications;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.OriginVM;

namespace UPA.BLL.Interfaces
{
   public interface IOriginService
    {
  
        IndexOriginVM ListOrigins(CountOfAssetsSpecParams @params);


        IEnumerable<Origin> GetOrigins();


        EditOriginVM GetById(int id);
        int Add(CreateOriginVM originObj);
        int Update(EditOriginVM originObj);
        int Delete(int id);
    }
}
