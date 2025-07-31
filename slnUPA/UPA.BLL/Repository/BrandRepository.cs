using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using UPA.BLL.Interfaces;
using UPA.DAl;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.BrandVM;

namespace UPA.BLL.Repository
{
   public class BrandRepository:IBrandService
    {

        private readonly UPAModel _context;

        public BrandRepository(UPAModel context)
        {
            _context = context;
        }

        public List<Brand> ListBrands()
        {
          return  _context.Brands.ToList();
        }


        public int Add(CreateBrandVM model)
        {
            Brand brandObj = new Brand();
            try
            {
                if (model != null)
                {
                    brandObj.Code = model.Code;
                    brandObj.Name = model.Name;
                    brandObj.NameAr = model.NameAr;
                    _context.Brands.Add(brandObj);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return brandObj.Id;
        }

 
        public int Delete(int id)
        {
            var brandObj = _context.Brands.Find(id);
            try
            {
                if (brandObj != null)
                {
                    _context.Brands.Remove(brandObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;
        }

        public GenerateBrandCodeVM GenerateBrandCode()
        {
            GenerateBrandCodeVM numberObj = new GenerateBrandCodeVM();
            int code = 0;

            var lastId = _context.Brands.ToList();
            if (lastId.Count > 0)
            {

                var lastBrandCode = lastId.Max(a => a.Code);
                if (lastBrandCode == null)
                {
                    numberObj.Code = (code + 1).ToString();
                    var lastcode = numberObj.Code.PadLeft(3, '0');
                    numberObj.Code = lastcode;
                }
                else
                {
                    var hospitalCode = (int.Parse(lastBrandCode) + 1).ToString();
                    var lastcode = hospitalCode.ToString().PadLeft(3, '0');
                    numberObj.Code = lastcode;
                }
            }
            else
            {
                numberObj.Code = (code + 1).ToString();
                var lastcode = numberObj.Code.PadLeft(3, '0');
                numberObj.Code = lastcode;
            }

            return numberObj;
        }

   
  
        public EditBrandVM GetById(int id)
        {
            return _context.Brands.Where(a => a.Id == id).Select(item => new EditBrandVM
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr
            }).First();
        }

 
        public IEnumerable<IndexBrandVM.GetData> SortBrands(SortBrandVM sortObj)
        {
            var lstBrands = _context.Brands.ToList();
            if (sortObj.Code != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstBrands = lstBrands.OrderByDescending(d => d.Code).ToList();
                else
                    lstBrands = lstBrands.OrderBy(d => d.Code).ToList();
            }
            else if (sortObj.Name != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstBrands = lstBrands.OrderByDescending(d => d.Name).ToList();
                else
                    lstBrands = lstBrands.OrderBy(d => d.Name).ToList();
            }

            else if (sortObj.NameAr != "")
            {
                if (sortObj.SortStatus == "descending")
                    lstBrands = lstBrands.OrderByDescending(d => d.NameAr).ToList();
                else
                    lstBrands = lstBrands.OrderBy(d => d.NameAr).ToList();
            }

            return (IEnumerable<IndexBrandVM.GetData>)lstBrands;
        }

        public int Update(EditBrandVM model)
        {
            try
            {
                var brandObj = _context.Brands.Find(model.Id);
                if (brandObj != null)
                {
                    brandObj.Code = model.Code;
                    brandObj.Name = model.Name;
                    brandObj.NameAr = model.NameAr;
                    _context.Entry(brandObj).State = EntityState.Modified;
                    _context.SaveChanges();
                    return brandObj.Id;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }
        
    }
}
