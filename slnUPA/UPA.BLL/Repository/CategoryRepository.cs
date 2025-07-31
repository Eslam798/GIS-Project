using Microsoft.EntityFrameworkCore;
using UPA.BLL.Interfaces;
using UPA.DAl;
using UPA.DAL.Models;

using UPA.ViewModels.ViewModels.CategoryVM;
using UPA.ViewModels.ViewModels.MasterAssetVM;

namespace UPA.BLL.Repository
{
   public class CategoryRepository :  ICategoryService
    {

        private readonly UPAModel _context;

        public CategoryRepository(UPAModel context)
        {
            _context = context;
        }

        public List<Category> ListCategories()
        {
          return _context.Categories.ToList();
        }


        public int Add(CreateCategoryVM model)
        {
            Category categoryObj = new Category();
            try
            {
                if (model != null)
                {
                    categoryObj.Code = model.Code;
                    categoryObj.Name = model.Name;
                    categoryObj.NameAr = model.NameAr;
                 //   categoryObj.CategoryTypeId = model.CategoryTypeId;
                    _context.Categories.Add(categoryObj);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return categoryObj.Id;
        }

        public int Delete(int id)
        {
            var categoryObj = _context.Categories.Find(id);
            try
            {
                if (categoryObj != null)
                {
                    _context.Categories.Remove(categoryObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public IEnumerable<IndexCategoryVM.GetData> GetAll()
        {
            return _context.Categories.ToList().Select(item => new IndexCategoryVM.GetData
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr,
               // CategoryTypeId = item.CategoryTypeId
            });
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public EditCategoryVM GetById(int id)
        {
            return _context.Categories.Where(a => a.Id == id).Select(item => new EditCategoryVM
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr,
              //  CategoryTypeId = item.CategoryTypeId
            }).FirstOrDefault();
        }

   

        public IEnumerable<IndexCategoryVM.GetData> GetCategoryByName(string categoryName)
        {
            return _context.Categories.Where(a => a.Name == categoryName || a.NameAr == categoryName).ToList().Select(item => new IndexCategoryVM.GetData
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr,
                //CategoryTypeId = item.CategoryTypeId
            });
        }

        public int Update(EditCategoryVM model)
        {
            try
            {
                var categoryObj = _context.Categories.Find(model.Id);
                if (categoryObj != null)
                {
                    categoryObj.Code = model.Code;
                    categoryObj.Name = model.Name;
                    categoryObj.NameAr = model.NameAr;
                 //   categoryObj.CategoryTypeId = model.CategoryTypeId;
                    _context.Entry(categoryObj).State = EntityState.Modified;
                    _context.SaveChanges();
                    return categoryObj.Id;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public GenerateCategoryCodeVM GenerateCategoryCode()
        {
            GenerateCategoryCodeVM numberObj = new GenerateCategoryCodeVM();
            int barCode = 0;

            var lastId = _context.Categories.ToList();
            if (lastId.Count() > 0)
            {
                var code = lastId.Max(a => a.Id);
                var barcode = (code + 1).ToString();
                var lastcode = barcode.ToString().PadLeft(5, '0');
                numberObj.Code = lastcode;
            }
            else
            {
                numberObj.Code = (barCode + 1).ToString();
            }

            return numberObj;
        }
    }
}
