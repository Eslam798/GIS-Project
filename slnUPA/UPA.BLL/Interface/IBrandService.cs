using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.BrandVM;

namespace UPA.BLL.Interfaces
{
    public interface IBrandService 
    {
        List<Brand> ListBrands();   
        EditBrandVM GetById(int id);
        int Add(CreateBrandVM brandObj);
        int Update(EditBrandVM brandObj);
        int Delete(int id);
        GenerateBrandCodeVM GenerateBrandCode();
    }
}
