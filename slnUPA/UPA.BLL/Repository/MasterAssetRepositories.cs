using Microsoft.EntityFrameworkCore;
using UPA.BLL.Interfaces;
using UPA.BLL.Specifications;
using UPA.DAl;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.HospitalVM;
using UPA.ViewModels.ViewModels.MasterAssetVM;

namespace UPA.BLL.Repository
{
    public class MasterAssetRepositories : IMasterAssetService
    {

        private UPAModel _context;


        public MasterAssetRepositories(UPAModel context)
        {
            _context = context;
        }

        public IEnumerable<IndexMasterAssetVM.GetData> GetAll()
        {
            List<IndexMasterAssetVM.GetData> list = new List<IndexMasterAssetVM.GetData>();
            var lstMasters = _context.MasterAssets.Include(a => a.Brand).Include(a => a.Category)

                .Include(a => a.SubCategory).Include(a => a.ECRIS).Include(a => a.Origin).OrderBy(a => a.Id).ToList();

            foreach (var item in lstMasters)
            {
                IndexMasterAssetVM.GetData getDataObj = new IndexMasterAssetVM.GetData();
                getDataObj.Id = item.Id;
                getDataObj.Code = item.Code;
                getDataObj.Model = item.ModelNumber;
                getDataObj.CategoryId = item.CategoryId;
                getDataObj.SubCategoryId = item.SubCategoryId;
                getDataObj.PMColor = item.PMColor;
                getDataObj.PMBGColor = item.PMBGColor;
                getDataObj.ECRIName = item.ECRIId != null ? item.ECRIS.Name : "";
                getDataObj.ECRINameAr = item.ECRIId != null ? item.ECRIS.NameAr : "";
                getDataObj.Name = item.Name;
                getDataObj.NameAr = item.NameAr;
                getDataObj.OriginName = item.OriginId != null ? item.Origin.Name : "";
                getDataObj.OriginNameAr = item.OriginId != null ? item.Origin.NameAr : "";
                getDataObj.BrandName = item.Brand != null ? item.Brand.Name : "";
                getDataObj.BrandNameAr = item.Brand != null ? item.Brand.NameAr : "";
                list.Add(getDataObj);
            }
            return list;
        }
        public int Add(CreateMasterAssetVM model)
        {
            MasterAsset masterAssetObj = new MasterAsset();
            try
            {
                if (model != null)
                {
                    masterAssetObj.Code = model.Code;
                    masterAssetObj.Name = model.Name;
                    masterAssetObj.NameAr = model.NameAr;
                    if (model.BrandId != null)
                        masterAssetObj.BrandId = model.BrandId;
                    if (model.CategoryId != null)
                        masterAssetObj.CategoryId = model.CategoryId;
                    if (model.SubCategoryId != null)
                        masterAssetObj.SubCategoryId = model.SubCategoryId;
                    masterAssetObj.Description = model.Description;
                    masterAssetObj.DescriptionAr = model.DescriptionAr;
                    masterAssetObj.ExpectedLifeTime = model.ExpectedLifeTime;
                    masterAssetObj.Height = model.Height;
                    masterAssetObj.Length = model.Length;
                    masterAssetObj.ModelNumber = model.ModelNumber;
                    masterAssetObj.VersionNumber = model.VersionNumber;
                    masterAssetObj.Weight = model.Weight;
                    masterAssetObj.Width = model.Width;
                    if (model.ECRIId != null)
                        masterAssetObj.ECRIId = model.ECRIId;

                    if (model.PeriorityId != null)
                        masterAssetObj.PeriorityId = model.PeriorityId;
                    if (model.OriginId != null)
                        masterAssetObj.OriginId = model.OriginId;
                    masterAssetObj.Power = model.Power;
                    masterAssetObj.Voltage = model.Voltage;
                    masterAssetObj.Ampair = model.Ampair;
                    masterAssetObj.Frequency = model.Frequency;
                    masterAssetObj.ElectricRequirement = model.ElectricRequirement;
                    if (model.PMTimeId != 0)
                        masterAssetObj.PMTimeId = model.PMTimeId;
                    masterAssetObj.AssetImg = model.AssetImg;

                    _context.MasterAssets.Add(masterAssetObj);
                    _context.SaveChanges();
                    return masterAssetObj.Id;
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return masterAssetObj.Id;
        }

        public int CountMasterAssets()
        {
            return _context.MasterAssets.Count();
        }



        public int CreateMasterAssetDocuments(MasterAssetAttachment attachObj)
        {
            MasterAssetAttachment MasterAssetAttachmentObj = new MasterAssetAttachment();
            MasterAssetAttachmentObj.MasterAssetId = attachObj.MasterAssetId;
            MasterAssetAttachmentObj.Title = attachObj.Title;
            MasterAssetAttachmentObj.FileName = attachObj.FileName;
            _context.MasterAssetAttachments.Add(MasterAssetAttachmentObj);
            _context.SaveChanges();
            int Id = MasterAssetAttachmentObj.Id;
            return Id;
        }

        public int Delete(int id)
        {
            var masterAssetObj = _context.MasterAssets.Find(id);
            try
            {
                if (masterAssetObj != null)
                {
                    _context.MasterAssets.Remove(masterAssetObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }

        public int DeleteMasterAssetAttachment(int id)
        {
            try
            {
                var attachObj = _context.MasterAssetAttachments.Find(id);
                _context.MasterAssetAttachments.Remove(attachObj);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }





        public IEnumerable<MasterAsset> GetAllMasterAssetsByHospitalId(int hospitalId)
        {
            List<MasterAsset> list = new List<MasterAsset>();
            var lstMasterAssets = _context.AssetDetails.Include(a => a.MasterAsset)
                                   .Where(a => a.HospitalId == hospitalId).ToList().GroupBy(a => a.MasterAsset.Id).ToList();


            foreach (var item in lstMasterAssets)
            {
                MasterAsset masterAssetObj = new MasterAsset();
                masterAssetObj.Id = item.FirstOrDefault().MasterAsset.Id;
                masterAssetObj.Name = item.FirstOrDefault().MasterAsset.Name;
                masterAssetObj.NameAr = item.FirstOrDefault().MasterAsset.NameAr;
                list.Add(masterAssetObj);
            }
            return list;
        }


        public IEnumerable<MasterAssetAttachment> GetAttachmentByMasterAssetId(int assetId)
        {
            return _context.MasterAssetAttachments.Where(a => a.MasterAssetId == assetId).ToList();
        }

        public EditMasterAssetVM GetById(int id)
        {
            var lstMasterAssets = _context.MasterAssets.Include(a => a.Brand).Where(a => a.Id == id).ToList();

            if (lstMasterAssets.Count() > 0)
            {
                MasterAsset item = lstMasterAssets[0];
                EditMasterAssetVM masterAssetObj = new EditMasterAssetVM();
                masterAssetObj.Id = item.Id;
                masterAssetObj.Name = item.Name;
                masterAssetObj.NameAr = item.NameAr;
                masterAssetObj.Code = item.Code;
                masterAssetObj.ECRIId = item.ECRIId != null ? (int)item.ECRIId : null;
                masterAssetObj.BrandId = item.Brand != null ? item.BrandId : null;
                masterAssetObj.CategoryId = item.CategoryId != null ? item.CategoryId : null;
                masterAssetObj.SubCategoryId = item.SubCategoryId != null ? item.SubCategoryId : null;
                masterAssetObj.Description = item.Description;
                masterAssetObj.DescriptionAr = item.DescriptionAr;
                masterAssetObj.ExpectedLifeTime = item.ExpectedLifeTime != null ? (int)item.ExpectedLifeTime : 0;
                masterAssetObj.Height = item.Height;
                masterAssetObj.Length = item.Length;
                masterAssetObj.ModelNumber = item.ModelNumber;
                masterAssetObj.Model = item.ModelNumber;
                masterAssetObj.VersionNumber = item.VersionNumber;
                masterAssetObj.Weight = item.Weight;
                masterAssetObj.Width = item.Width;
                masterAssetObj.PeriorityId = item.PeriorityId;
                masterAssetObj.OriginId = item.OriginId != null ? item.OriginId : null;
                masterAssetObj.Power = item.Power;
                masterAssetObj.Voltage = item.Voltage;
                masterAssetObj.Ampair = item.Ampair;
                masterAssetObj.Frequency = item.Frequency;
                masterAssetObj.ElectricRequirement = item.ElectricRequirement;
                masterAssetObj.PMTimeId = item.PMTimeId;
                masterAssetObj.AssetImg = item.AssetImg;


                if (item.BrandId != null)
                {
                    masterAssetObj.BrandName = item.Brand.Name;
                    masterAssetObj.BrandNameAr = item.Brand.NameAr;
                }

                if (masterAssetObj.PeriorityId != null)
                {
                    var lstPeriorities = _context.AssetPeriorities.Where(a => a.Id == masterAssetObj.PeriorityId).ToList();
                    if (lstPeriorities.Count() > 0)
                    {
                        masterAssetObj.PeriorityId = lstPeriorities[0].Id;

                    }
                }

                if (masterAssetObj.OriginId != null)
                {
                    var lstOrigins = _context.AssetPeriorities.Where(a => a.Id == masterAssetObj.OriginId).ToList();
                    if (lstOrigins.Count() > 0)
                    {
                        masterAssetObj.OriginId = lstOrigins[0].Id;
                    }
                }


                if (masterAssetObj.CategoryId != null)
                {
                    var lstCategories = _context.Categories.Where(a => a.Id == masterAssetObj.CategoryId).ToList();
                    if (lstCategories.Count() > 0)
                    {
                        masterAssetObj.CategoryId = lstCategories[0].Id;
                    }
                }

                if (masterAssetObj.SubCategoryId != null)
                {
                    var lstSubCategories = _context.SubCategories.Where(a => a.Id == masterAssetObj.SubCategoryId).ToList();
                    if (lstSubCategories.Count() > 0)
                    {
                        masterAssetObj.SubCategoryId = lstSubCategories[0].Id;
                    }
                }

                if (masterAssetObj.ECRIId != null)
                {
                    var lstECRIs = _context.ECRIS.Where(a => a.Id == masterAssetObj.ECRIId).ToList();
                    if (lstECRIs.Count() > 0)
                    {
                        masterAssetObj.ECRIId = lstECRIs[0].Id;
                    }
                }

                return masterAssetObj;
            }

            return null;
        }

        public int Update(EditMasterAssetVM model)
        {
            try
            {

                var masterAssetObj = _context.MasterAssets.Find(model.Id);
                masterAssetObj.Id = model.Id;
                masterAssetObj.Code = model.Code;
                masterAssetObj.Name = model.Name;
                masterAssetObj.NameAr = model.NameAr;
                masterAssetObj.BrandId = model.BrandId;
                masterAssetObj.CategoryId = model.CategoryId;
                masterAssetObj.SubCategoryId = model.SubCategoryId;
                masterAssetObj.Description = model.Description;
                masterAssetObj.DescriptionAr = model.DescriptionAr;
                masterAssetObj.ExpectedLifeTime = model.ExpectedLifeTime;
                masterAssetObj.Height = model.Height;
                masterAssetObj.Length = model.Length;
                masterAssetObj.ModelNumber = model.ModelNumber;
                masterAssetObj.VersionNumber = model.VersionNumber;
                masterAssetObj.Weight = model.Weight;
                masterAssetObj.Width = model.Width;
                masterAssetObj.ECRIId = model.ECRIId;
                masterAssetObj.PeriorityId = model.PeriorityId;
                masterAssetObj.OriginId = model.OriginId;
                masterAssetObj.Power = model.Power;
                masterAssetObj.Voltage = model.Voltage;
                masterAssetObj.Ampair = model.Ampair;
                masterAssetObj.Frequency = model.Frequency;
                masterAssetObj.ElectricRequirement = model.ElectricRequirement;
                masterAssetObj.PMTimeId = model.PMTimeId;
                masterAssetObj.AssetImg = model.AssetImg;

                //string[] splitFileName = model.AssetImg.Split('.');
                // var filenameOnly = splitFileName[0];
                // var ext = splitFileName[1];

                // var newFileName = filenameOnly + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + "." + ext;
                // masterAssetObj.AssetImg = newFileName;





                _context.Entry(masterAssetObj).State = EntityState.Modified;
                _context.SaveChanges();
                return masterAssetObj.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }

        public ViewMasterAssetVM ViewMasterAsset(int id)
        {
            ViewMasterAssetVM model = new ViewMasterAssetVM();
            var masterAssetObj = _context.MasterAssets.Find(id);
            model.Id = masterAssetObj.Id;
            model.Code = masterAssetObj.Code;
            model.Name = masterAssetObj.Name;
            model.NameAr = masterAssetObj.NameAr;
            model.Description = masterAssetObj.Description;
            model.DescriptionAr = masterAssetObj.DescriptionAr;
            model.ExpectedLifeTime = masterAssetObj.ExpectedLifeTime != null ? (int)masterAssetObj.ExpectedLifeTime : 0;
            model.Height = masterAssetObj.Height;
            model.Length = masterAssetObj.Length;
            model.ModelNumber = masterAssetObj.ModelNumber;
            model.VersionNumber = masterAssetObj.VersionNumber;
            model.Weight = masterAssetObj.Weight;
            model.Width = masterAssetObj.Width;

            model.Power = masterAssetObj.Power;
            model.Voltage = masterAssetObj.Voltage;
            model.Ampair = masterAssetObj.Ampair;
            model.Frequency = masterAssetObj.Frequency;
            model.ElectricRequirement = masterAssetObj.ElectricRequirement;
            model.AssetImg = masterAssetObj.AssetImg;

   

            var lstECRIs = _context.ECRIS.Where(a => a.Id == masterAssetObj.ECRIId).ToList();
            if (lstECRIs.Count() > 0)
            {
                model.ECRIId = lstECRIs[0].Id;
                model.ECRIName = lstECRIs[0].Name;
                model.ECRINameAr = lstECRIs[0].NameAr;
            }

            var lstBrands = _context.Brands.Where(a => a.Id == masterAssetObj.BrandId).ToList();
            if (lstBrands.Count() > 0)
            {
                model.BrandId = lstBrands[0].Id;
                model.BrandName = lstBrands[0].Name;
                model.BrandNameAr = lstBrands[0].NameAr;
            }

            var lstPeriorities = _context.AssetPeriorities.Where(a => a.Id == masterAssetObj.PeriorityId).ToList();
            if (lstPeriorities.Count() > 0)
            {
                model.PeriorityId = lstPeriorities[0].Id;
                model.PeriorityName = lstPeriorities[0].Name;
                model.PeriorityNameAr = lstPeriorities[0].NameAr;
            }

            var lstOrigins = _context.AssetPeriorities.Where(a => a.Id == masterAssetObj.OriginId).ToList();
            if (lstOrigins.Count() > 0)
            {
                model.OriginId = lstOrigins[0].Id;
                model.OriginName = lstOrigins[0].Name;
                model.OriginNameAr = lstOrigins[0].NameAr;
            }


            if (masterAssetObj.CategoryId != null)
            {
                var lstCategories = _context.Categories.Where(a => a.Id == masterAssetObj.CategoryId).ToList();
                if (lstCategories.Count() > 0)
                {
                    model.CategoryId = lstCategories[0].Id;
                    model.CategoryName = lstCategories[0].Name;
                    model.CategoryNameAr = lstCategories[0].NameAr;
                }
            }

            if (masterAssetObj.SubCategoryId != null)
            {
               var lstSubCategories = _context.SubCategories.Where(a => a.Id == masterAssetObj.SubCategoryId).ToList();
                if (lstSubCategories.Count() > 0)
                {
                    model.SubCategoryId = lstSubCategories[0].Id;
                    model.SubCategoryName = lstSubCategories[0].Name;
                    model.SubCategoryNameAr = lstSubCategories[0].NameAr;
                }
            }





            return model;
        }


        public IEnumerable<IndexMasterAssetVM.GetData> AutoCompleteMasterAssetName(string name)
        {
            var lst = _context.MasterAssets.Include(a=>a.Brand).Where(a => a.Name.Contains(name) || a.NameAr.Contains(name))
                .Select(item => new IndexMasterAssetVM.GetData
            {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr,
                Model = item.ModelNumber,
                ModelNumber = item.ModelNumber,
                BrandName = item.Brand != null? item.Brand.Name:"",
                BrandNameAr = item.Brand != null ? item.Brand.NameAr   :""

            }).ToList();
            return lst;
        }

     

        public int UpdateMasterAssetImageAfterInsert(CreateMasterAssetVM masterAssetObj)
        {
            var masterObj = _context.MasterAssets.Find(masterAssetObj.Id);
            masterObj.AssetImg = masterAssetObj.AssetImg;
            _context.Entry(masterObj).State = EntityState.Modified;
            _context.SaveChanges();
            return masterAssetObj.Id;
        }



        public IEnumerable<IndexMasterAssetVM.GetData> AutoCompleteMasterAssetName(string name, int hospitalId)
        {

            var lst = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.MasterAsset.Brand).Where(a => a.MasterAsset.Name.Contains(name) || a.MasterAsset.NameAr.Contains(name) && a.HospitalId == hospitalId).ToList().Select(item => new IndexMasterAssetVM.GetData
            {
                Id = item.Id,
                Name = item.MasterAsset != null ? item.MasterAsset.Name : "",
                NameAr = item.MasterAsset != null ? item.MasterAsset.NameAr : "",
                Model = item.MasterAsset != null ? item.MasterAsset.ModelNumber : "",
                ModelNumber = item.MasterAsset != null ? item.MasterAsset.ModelNumber : "",
                BrandName = item.MasterAsset?.Brand != null ? item.MasterAsset.Brand.Name : "",
                BrandNameAr = item.MasterAsset?.Brand != null ? item.MasterAsset.Brand.NameAr : "",
                SerialNumber = item.SerialNumber != null ? item.SerialNumber : "",
                BarCode = item.Barcode != null ? item.Barcode : "",
            }).ToList();
            return lst;
        }






        public MasterAssetAttachment GetLastDocumentForMsterAssetId(int masterId)
        {
            MasterAssetAttachment documentObj = new MasterAssetAttachment();
            var lstDocuments = _context.MasterAssetAttachments.Where(a => a.MasterAssetId == masterId).OrderBy(a => a.FileName).ToList();
            if (lstDocuments.Count() > 0)
            {
                documentObj = lstDocuments.Last();
            }
            return documentObj;
        }

        public IEnumerable<MasterAsset> DistinctAutoCompleteMasterAssetName(string name)
        {
            var lst = _context.MasterAssets.Where(a => a.Name.StartsWith(name) || a.NameAr.StartsWith(name)).ToList();
            lst = lst.GroupBy(a => a.Name).Select(x => x.FirstOrDefault()).ToList();
            return lst;
        }

        public GeneratedMasterAssetCodeVM GenerateMasterAssetCode()
        {
            GeneratedMasterAssetCodeVM numberObj = new GeneratedMasterAssetCodeVM();
            int barCode = 0;

            var lastId = _context.MasterAssets.ToList();
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

        public IndexMasterAssetVM ListMasterAsset(CountOfAssetsSpecParams @params)
        {
            IEnumerable<MasterAsset> result = _context.MasterAssets.Include(a => a.Brand).Include(a => a.ECRIS).Include(a => a.Origin).OrderBy(a=>a.Code).ToList();
            IndexMasterAssetVM mainClass = new IndexMasterAssetVM();
            List<IndexMasterAssetVM.GetData> indexCountOfAssetsVMs = new List<IndexMasterAssetVM.GetData>();
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
                IndexMasterAssetVM.GetData data = new IndexMasterAssetVM.GetData();
                data.Id = item.Id;
                data.Code = item.Code;
                data.Name = item.Name;
                data.NameAr = item.NameAr;


                data.ECRIId = item.ECRIId != null ? (int)item.ECRIId : null;
                data.BrandId = item.Brand != null ? item.BrandId : null;
                data.CategoryId = item.CategoryId != null ? item.CategoryId : null;
                data.SubCategoryId = item.SubCategoryId != null ? item.SubCategoryId : null;
                data.Description = item.Description;
                data.DescriptionAr = item.DescriptionAr;
                data.ExpectedLifeTime = item.ExpectedLifeTime != null ? (int)item.ExpectedLifeTime : 0;
                data.Height = item.Height;
                data.Length = item.Length;
                data.ModelNumber = item.ModelNumber;
                data.Model = item.ModelNumber;
                data.VersionNumber = item.VersionNumber;
                data.Weight = item.Weight;
                data.Width = item.Width;
                data.PeriorityId = item.PeriorityId;
                data.OriginId = item.OriginId != null ? item.OriginId : null;
                data.Power = item.Power;
                data.Voltage = item.Voltage;
                data.Ampair = item.Ampair;
                data.Frequency = item.Frequency;
                data.ElectricRequirement = item.ElectricRequirement;
                data.PMTimeId = item.PMTimeId;
                data.AssetImg = item.AssetImg;
                if (item.Brand != null)
                {
                    data.BrandName = item.Brand?.Name;
                    data.BrandNameAr = item.Brand?.NameAr;
                }


                if (item.Origin != null)
                {
                    data.OriginName = item.Origin?.Name;
                    data.OriginNameAr = item.Origin?.NameAr;
                }

                if (item.ECRIS != null)
                {
                    data.ECRIName = item.ECRIS?.Name;
                    data.ECRINameAr = item.ECRIS?.NameAr;
                }



                indexCountOfAssetsVMs.Add(data);


            }
            mainClass.Results = indexCountOfAssetsVMs;
            return mainClass;
        }



        public IndexMasterAssetVM ListMasterAsset()
        {
            IEnumerable<MasterAsset> result = _context.MasterAssets.Include(a => a.Brand).ToList();
            IndexMasterAssetVM mainClass = new IndexMasterAssetVM();
            List<IndexMasterAssetVM.GetData> indexCountOfAssetsVMs = new List<IndexMasterAssetVM.GetData>();

            foreach (var item in result)
            {
                IndexMasterAssetVM.GetData data = new IndexMasterAssetVM.GetData();
                data.Id = item.Id;
                data.Code = item.Code;
                data.Name = item.Name;
                data.NameAr = item.NameAr;
                data.ECRIId = item.ECRIId != null ? (int)item.ECRIId : null;
                data.BrandId = item.Brand != null ? item.BrandId : null;
                data.CategoryId = item.CategoryId != null ? item.CategoryId : null;
                data.SubCategoryId = item.SubCategoryId != null ? item.SubCategoryId : null;
                data.Description = item.Description;
                data.DescriptionAr = item.DescriptionAr;
                data.ExpectedLifeTime = item.ExpectedLifeTime != null ? (int)item.ExpectedLifeTime : 0;
                data.Height = item.Height;
                data.Length = item.Length;
                data.ModelNumber = item.ModelNumber;
                data.Model = item.ModelNumber;
                data.VersionNumber = item.VersionNumber;
                data.Weight = item.Weight;
                data.Width = item.Width;
                data.PeriorityId = item.PeriorityId;
                data.OriginId = item.OriginId != null ? item.OriginId : null;
                data.Power = item.Power;
                data.Voltage = item.Voltage;
                data.Ampair = item.Ampair;
                data.Frequency = item.Frequency;
                data.ElectricRequirement = item.ElectricRequirement;
                data.PMTimeId = item.PMTimeId;
                data.AssetImg = item.AssetImg;
                if (item.Brand != null)
                {
                    data.BrandName = item.Brand?.Name;
                    data.BrandNameAr = item.Brand?.NameAr;
                }
                indexCountOfAssetsVMs.Add(data);
            }
            mainClass.Results = indexCountOfAssetsVMs;
            return mainClass;
        }



    }
}
