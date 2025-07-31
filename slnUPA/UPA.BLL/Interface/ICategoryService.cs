

using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.BrandVM;
using UPA.ViewModels.ViewModels.CategoryVM;

namespace UPA.BLL.Interfaces
{
    public interface ICategoryService 
    {
        List<Category> ListCategories();
        EditCategoryVM GetById(int id);
        int Add(CreateCategoryVM categoryVM);
        int Update(EditCategoryVM categoryVM);
        int Delete(int id);

        GenerateCategoryCodeVM GenerateCategoryCode();
    }
}
