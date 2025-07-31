using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.CategoryVM;
using UPA.ViewModels.ViewModels.SubCategoryVM;

namespace UPA.BLL.Interfaces
{
    public interface ISubCategoryService
    {
        IEnumerable<IndexSubCategoryVM.GetData> GetAll();
        EditSubCategoryVM GetById(int id);
        IEnumerable<SubCategory> GetSubCategoryByCategoryId(int categoryId);
        int Add(CreateSubCategoryVM subCategoryObj);
        int Update(EditSubCategoryVM subCategoryObj);
        int Delete(int id);

        GenerateSubCategoryCodeVM GenerateSubCategoryCode();

    }
}
