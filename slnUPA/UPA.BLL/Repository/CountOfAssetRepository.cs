using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UPA.BLL.Interfaces;
using UPA.BLL.Specifications;
using UPA.DAl;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.CountOfAssetsVM;
using static UPA.ViewModels.ViewModels.CountOfAssetsVM.IndexCountOfAssetsVM;

namespace UPA.BLL.Repository
{
    public class CountOfAssetRepository : ICountOfAssetService
    {

        private readonly UPAModel _db;

        public CountOfAssetRepository(UPAModel db)
        {
            _db = db;
        }

        public int CreateCountOfAsset(List<CreateCountOfAssetsVM> model)
        {
            try
            {
                if (model.Count() > 0)
                {
                    foreach (var item in model)
                    {
                        CountOfAsset countOfAssetObj = new CountOfAsset();
                        countOfAssetObj.GovernorateId = item.GovernorateId;
                        countOfAssetObj.OrganizationId = item.OrganizationId;
                        countOfAssetObj.CategoryId = item.CategoryId;
                        countOfAssetObj.BrandId = item.BrandId;
                        countOfAssetObj.Count = item.Count;
                        _db.CountOfAssets.Add(countOfAssetObj);
                        _db.SaveChanges();
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return -1;
            }
        }

        public int CreateRecordOfCountOfAsset(CreateCountOfAssetsVM model)
        {
            try
            {
                CountOfAsset countOfAssetObj = new CountOfAsset();
                countOfAssetObj.GovernorateId = model.GovernorateId;
                countOfAssetObj.OrganizationId = model.OrganizationId;
                countOfAssetObj.CategoryId = model.CategoryId;
                countOfAssetObj.BrandId = model.BrandId;
                countOfAssetObj.Count = model.Count;
                _db.CountOfAssets.Add(countOfAssetObj);
                _db.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return -1;
            }
        }

        public int DeleteCountOfAsset(int id)
        {
            var countOfAssetObj = _db.CountOfAssets.Find(id);
            try
            {
                if (countOfAssetObj != null)
                {
                    _db.CountOfAssets.Remove(countOfAssetObj);
                    return _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return 0;

        }

        public IEnumerable<IndexCountOfAssetsVM.Data> GetCountOfAssetByCategoryGovernorate(CountOfAssetsSpecParams @params)
        {
     IQueryable<AssetDetail> results = _db.AssetDetails.Include(a => a.Hospital)
                                              .Include(a => a.Hospital.Organization)
                                              .Include(a => a.Hospital.Governorate)
                                              .Include(a => a.MasterAsset)
                                              .Include(a => a.MasterAsset.Category)
                                               .Include(a => a.MasterAsset.SubCategory)
                                              .Include(a => a.MasterAsset.Brand);

                          if (@params.BrandId?.Count > 0)
            {
                results = results.Where(item => @params.BrandId.Contains(item.MasterAsset.BrandId));
            }
            if (@params.OrgId?.Count > 0)
            {
                results = results.Where(item => @params.OrgId.Contains(item.Hospital.OrganizationId));
            }
            
        List< IndexCountOfAssetsVM.Data> data =   results.GroupBy(l => new { l.Hospital.GovernorateId, l.MasterAsset.CategoryId })
           .Select(cl => new IndexCountOfAssetsVM.Data
           {
               Id = cl.First().Id,
               GovernorateId = cl.First().Hospital.GovernorateId,
               CategoryId = cl.First().MasterAsset.CategoryId,
               BrandId = cl.First().MasterAsset.BrandId,
               OrganizationId = cl.First().Hospital.OrganizationId,
               Count = cl.Count()
           }).ToList();
            return data;

        }


        public IEnumerable<IndexCountOfAssetsVM.Data> GetCountOfAssetByOrganizationGovernorate()
        {
            var results = _db.AssetDetails.Include(a => a.Hospital)
                                     .Include(a => a.Hospital.Organization)
                                             .Include(a => a.Hospital.Governorate)
                                             .Include(a => a.MasterAsset)
                                             .Include(a => a.MasterAsset.Category)
                                              .Include(a => a.MasterAsset.SubCategory)
                                             .Include(a => a.MasterAsset.Brand)

                      .GroupBy(l => new { l.Hospital.GovernorateId, l.Hospital.OrganizationId })
                      .Select(cl => new IndexCountOfAssetsVM.Data
                      {
                          Id = cl.First().Id,
                          GovernorateId = cl.First().Hospital.GovernorateId,
                          CategoryId = cl.First().MasterAsset.CategoryId,
                          BrandId = cl.First().MasterAsset.BrandId,
                          OrganizationId = cl.First().Hospital.OrganizationId,
                          Count = cl.Count()
                      }).ToList();





            return _db.AssetDetails.Include(a => a.Hospital)
                                     .Include(a => a.Hospital.Organization)
                                             .Include(a => a.Hospital.Governorate)
                                             .Include(a => a.MasterAsset)
                                             .Include(a => a.MasterAsset.Category)
                                              .Include(a => a.MasterAsset.SubCategory)
                                             .Include(a => a.MasterAsset.Brand)

                      .GroupBy(l => new { l.Hospital.GovernorateId, l.Hospital.OrganizationId })
                      .Select(cl => new IndexCountOfAssetsVM.Data
                      {
                          Id = cl.First().Id,
                          GovernorateId = cl.First().Hospital.GovernorateId,
                          CategoryId = cl.First().MasterAsset.CategoryId,
                          BrandId = cl.First().MasterAsset.BrandId,
                          OrganizationId = cl.First().Hospital.OrganizationId,
                          Count = cl.Count()
                      }).ToList();

        }




        public IEnumerable<IndexCountOfAssetsVM.Data> FilterCountOfAssetByOrganizationGovernorate(CountOfAssetsSpecParams @params)
        {
            List<Data> indexCountOfAssetsVMs = new List<Data>();

            var results = _db.AssetDetails.Include(a => a.Hospital)
                                     .Include(a => a.Hospital.Organization)
                                             .Include(a => a.Hospital.Governorate)
                                             .Include(a => a.MasterAsset)
                                             .Include(a => a.MasterAsset.Category)
                                              .Include(a => a.MasterAsset.SubCategory)
                                             .Include(a => a.MasterAsset.Brand).AsQueryable();


                        if (@params.CategoryId?.Count > 0)
                        {
                            results = results.Where(item => @params.CategoryId.Contains(item.MasterAsset.CategoryId));
                        }
                        else
                        {
                            results = results;
                        }

                        if (@params.BrandId?.Count > 0)
                        {
                            results = results.Where(item => @params.BrandId.Contains(item.MasterAsset.BrandId));
                        }
                        else
                        {
                            results = results;
                        }


                      var results2 = results.GroupBy(l => new { l.Hospital.GovernorateId, l.Hospital.OrganizationId })
                         .Select(cl => new IndexCountOfAssetsVM.Data
                         {
                             Id = cl.FirstOrDefault().Id,
                             GovernorateId = cl.FirstOrDefault().Hospital.GovernorateId,
                             CategoryId = cl.FirstOrDefault().MasterAsset.CategoryId,
                             BrandId = cl.FirstOrDefault().MasterAsset.BrandId,
                             OrganizationId = cl.FirstOrDefault().Hospital.OrganizationId,
                             BrandNameAr = cl.FirstOrDefault().MasterAsset.Brand.NameAr,
                             CategoryName = cl.FirstOrDefault().MasterAsset.Category.Name,
                             CategoryNameAr = cl.FirstOrDefault().MasterAsset.Category.NameAr,
                             BrandName = cl.FirstOrDefault().MasterAsset.Brand.Name,
                             GovernorateName = cl.FirstOrDefault().Hospital.Governorate.Name,
                             GovernorateNameAr = cl.FirstOrDefault().Hospital.Governorate.NameAr,
                             OrganizationName = cl.FirstOrDefault().Hospital.Organization.Name,
                             OrganizationNameAr = cl.FirstOrDefault().Hospital.Organization.NameAr,
                             Count = cl.Count(),
                         });


            foreach (var item in results2)
            {
                Data data = new Data();
                data.BrandNameAr = item.BrandNameAr;
                data.BrandName = item.BrandName;
                data.GovernorateName = item.GovernorateName;
                data.GovernorateNameAr = item.GovernorateNameAr;
                data.CategoryName = item.CategoryName;
                data.CategoryNameAr = item.CategoryNameAr;
                data.OrganizationName = item.OrganizationName;
                data.OrganizationNameAr = item.OrganizationNameAr;
                data.CategoryId = item.CategoryId;
                data.OrganizationId = item.OrganizationId;
                data.GovernorateId = item.GovernorateId;
                data.Count = item.Count;
                indexCountOfAssetsVMs.Add(data);
            }

            return indexCountOfAssetsVMs;
        }









        public EditCountOfAssetsVM GetCountOfAssetById(int id)
        {
            EditCountOfAssetsVM editCountOfAssetObj = new EditCountOfAssetsVM();
            var lstCountOfAssets = _db.CountOfAssets.Include(a => a.Organization)
                                                    .Include(a => a.Governorate)
                                                    .Include(a => a.Category)
                                                    .Include(a => a.Brand)
                .Where(a => a.Id == id).ToList();
            if (lstCountOfAssets.Count > 0)
            {
                var countOfAssetObj = lstCountOfAssets[0];
                editCountOfAssetObj.Id = countOfAssetObj.Id;
                editCountOfAssetObj.Count = countOfAssetObj.Count;
                if (countOfAssetObj.Governorate != null)
                {
                    editCountOfAssetObj.GovernorateId = countOfAssetObj.GovernorateId;
                    editCountOfAssetObj.GovernorateName = countOfAssetObj.Governorate.Name;
                    editCountOfAssetObj.GovernorateNameAr = countOfAssetObj.Governorate.NameAr;
                }


                if (countOfAssetObj.Organization != null)
                {

                    editCountOfAssetObj.OrganizationId = countOfAssetObj.OrganizationId;
                    editCountOfAssetObj.OrganizationName = countOfAssetObj.Organization.Name;
                    editCountOfAssetObj.OrganizationNameAr = countOfAssetObj.Organization.NameAr;
                }

                if (countOfAssetObj.Brand != null)
                {
                    editCountOfAssetObj.BrandId = countOfAssetObj.BrandId;
                    editCountOfAssetObj.BrandName = countOfAssetObj.Brand.Name;
                    editCountOfAssetObj.BrandNameAr = countOfAssetObj.Brand.NameAr;
                }

                if (countOfAssetObj.Category != null)
                {
                    editCountOfAssetObj.CategoryId = countOfAssetObj.CategoryId;
                    editCountOfAssetObj.CategoryName = countOfAssetObj.Category.Name;
                    editCountOfAssetObj.CategoryNameAr = countOfAssetObj.Category.NameAr;
                }

            }
            return editCountOfAssetObj;
        }

        public List<CountOfAsset> ListCountOfAsset()
        {
            return _db.CountOfAssets.ToList();
        }

        public int UpdateCountOfAsset(EditCountOfAssetsVM model)
        {

            try
            {
                var countOfAssetObj = _db.CountOfAssets.Find(model.Id);
                if (countOfAssetObj != null)
                {
                    countOfAssetObj.GovernorateId = model.GovernorateId;
                    countOfAssetObj.OrganizationId = model.OrganizationId;
                    countOfAssetObj.CategoryId = model.CategoryId;
                    countOfAssetObj.BrandId = model.BrandId;
                    countOfAssetObj.Count = model.Count;
                    _db.Entry(countOfAssetObj).State = EntityState.Modified;
                    _db.SaveChanges();
                    return countOfAssetObj.Id;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;

        }

        public IndexCountOfAssetsVM GetCountOfAssetWithfilter(CountOfAssetsSpecParams @params)
        {
            IEnumerable<Data> result = _db.AssetDetails.Include(a => a.Hospital)
                      .Include(a => a.Hospital.Organization)
                              .Include(a => a.Hospital.Governorate)
                              .Include(a => a.MasterAsset)
                              .Include(a => a.MasterAsset.Category)
                               .Include(a => a.MasterAsset.SubCategory)
                              .Include(a => a.MasterAsset.Brand)

              .GroupBy(l => new { l.Hospital.GovernorateId, l.Hospital.OrganizationId, l.MasterAsset.BrandId, l.MasterAsset.CategoryId })

       .Select(cl => new IndexCountOfAssetsVM.Data
       {
           Id = cl.FirstOrDefault().Id,
           GovernorateId = cl.FirstOrDefault().Hospital.GovernorateId,
           CategoryId = cl.FirstOrDefault().MasterAsset.CategoryId,
           BrandId = cl.FirstOrDefault().MasterAsset.BrandId,
           OrganizationId = cl.FirstOrDefault().Hospital.OrganizationId,
           BrandNameAr = cl.FirstOrDefault().MasterAsset.Brand.NameAr,
           CategoryName = cl.FirstOrDefault().MasterAsset.Category.Name,
           CategoryNameAr = cl.FirstOrDefault().MasterAsset.Category.NameAr,
           BrandName = cl.FirstOrDefault().MasterAsset.Brand.Name,
           GovernorateName = cl.FirstOrDefault().Hospital.Governorate.Name,
           GovernorateNameAr = cl.FirstOrDefault().Hospital.Governorate.NameAr,
           OrganizationName = cl.FirstOrDefault().Hospital.Organization.Name,
           OrganizationNameAr = cl.FirstOrDefault().Hospital.Organization.NameAr,
           Count = cl.Count()
       }).ToList();


            IndexCountOfAssetsVM mainClass = new IndexCountOfAssetsVM();
            List<Data> indexCountOfAssetsVMs = new List<Data>();


            if (@params.GovId?.Count > 0)
            {
                result = result.Where(item => @params.GovId.Contains(item.GovernorateId));

            }
            if (@params.BrandId?.Count > 0)
            {

                result = result.Where(item => @params.BrandId.Contains(item.BrandId));
            }
            if (@params.OrgId?.Count > 0)
            {

                result = result.Where(item => @params.OrgId.Contains(item.OrganizationId));
            }
            if (@params.CategoryId?.Count > 0)
            {
                result = result.Where(item => @params.CategoryId.Contains(item.CategoryId));
            }
            if (@params.Count?.Count > 0)
            {
                result = result.Where(item => @params.Count.Contains(item.Count));

            }

            if (!string.IsNullOrEmpty(@params.Sort) && !string.IsNullOrEmpty(@params.SortStatus))
            {
                if (@params.SortStatus == "ascending" && @params.Sort == "Governorate")
                {

                    result = result.OrderBy(P => P.GovernorateName);

                }
                else if (@params.SortStatus == "descending" && @params.Sort == "Governorate")
                {

                    result = result.OrderByDescending(P => P.GovernorateName);

                }
                else if (@params.SortStatus == "descending" && @params.Sort == "المحافظة")
                {

                    result = result.OrderByDescending(P => P.GovernorateNameAr);

                }
                else if (@params.SortStatus == "ascending" && @params.Sort == "المحافظة")
                {

                    result = result.OrderBy(P => P.GovernorateNameAr);

                }
                else if (@params.SortStatus == "descending" && @params.Sort == "Organization")
                {
                    result = result.OrderByDescending(P => P.OrganizationName);

                }
                else if (@params.SortStatus == "ascending" && @params.Sort == "Organization")
                {

                    result = result.OrderBy(P => P.OrganizationName);

                }
                else if (@params.SortStatus == "ascending" && @params.Sort == "الهيئة")
                {

                    result = result.OrderBy(P => P.OrganizationNameAr);

                }
                else if (@params.SortStatus == "descending" && @params.Sort == "الهيئة")
                {
                    result = result.OrderByDescending(P => P.OrganizationNameAr);

                }
                else if (@params.SortStatus == "descending" && @params.Sort == "Brand")
                {
                    result = result.OrderByDescending(P => P.BrandName);

                }
                else if (@params.SortStatus == "descending" && @params.Sort == "الماركة")
                {

                    result = result.OrderByDescending(P => P.BrandNameAr);

                }
                else if (@params.SortStatus == "ascending" && @params.Sort == "الماركة")
                {

                    result = result.OrderBy(P => P.BrandNameAr);

                }
                else if (@params.SortStatus == "ascending" && @params.Sort == "Brand")
                {
                    result = result.OrderBy(P => P.BrandName);

                }
                else if (@params.SortStatus == "descending" && @params.Sort == "Category")
                {

                    result = result.OrderByDescending(P => P.CategoryName);

                }
                else if (@params.SortStatus == "descending" && @params.Sort == "التصنيف")
                {

                    result = result.OrderByDescending(P => P.CategoryNameAr);

                }
                else if (@params.SortStatus == "ascending" && @params.Sort == "التصنيف")
                {
                    result = result.OrderBy(P => P.CategoryNameAr);

                }
                else if (@params.SortStatus == "ascending" && @params.Sort == "Category")
                {

                    result = result.OrderBy(P => P.CategoryName);

                }
                else if (@params.SortStatus == "ascending" && @params.Sort == "Count")
                {
                    result = result.OrderBy(P => P.Count);


                }
                else if (@params.SortStatus == "descending" && @params.Sort == "Count")
                {
                    result = result.OrderByDescending(P => P.Count);
                }

                else if (@params.SortStatus == "descending" && @params.Sort == "العدد")
                {

                    result = result.OrderByDescending(P => P.Count);

                }
                else if (@params.SortStatus == "ascending" && @params.Sort == "العدد")
                {
                    result = result.OrderBy(P => P.Count);

                }
            }

            mainClass.Count = result.Count();

            if (@params.PageSize > 0)
            {
                result = result.Skip(@params.PageSize * (@params.PageIndex - 1)).Take(@params.PageSize).ToList();
            }
            else
            {
                result = result.ToList();
            }
            foreach (var item in result)
            {
                Data data = new Data();
                data.BrandNameAr = item.BrandNameAr;
                data.BrandName = item.BrandName;
                data.GovernorateName = item.GovernorateName;
                data.GovernorateNameAr = item.GovernorateNameAr;
                data.CategoryName = item.CategoryName;
                data.CategoryNameAr = item.CategoryNameAr;
                data.OrganizationName = item.OrganizationName;
                data.OrganizationNameAr = item.OrganizationNameAr;
                data.Count = item.Count;
                indexCountOfAssetsVMs.Add(data);
            }
            mainClass.Results = indexCountOfAssetsVMs;
            return mainClass;
        }



    }
}
