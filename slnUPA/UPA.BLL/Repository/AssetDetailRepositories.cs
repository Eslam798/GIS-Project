using System;
using System.Collections.Generic;
using System.Linq;

using System.Drawing;


using System.Threading.Tasks;
using System.IO;
using Microsoft.EntityFrameworkCore;

using UPA.BLL.Interfaces;
using UPA.DAl;
using UPA.ViewModels.ViewModels.AssetDetailVM;
using UPA.DAL.Models;
using UPA.BLL.Specifications;
using Asset.ViewModels.AssetDetailAttachmentVM;
using UPA.ViewModels.ViewModels.CountOfAssetsVM;
using System.Reflection.Metadata;
using System.Data;
using static UPA.ViewModels.ViewModels.CountOfAssetsVM.IndexCountOfAssetsVM;
using Asset.ViewModels.AssetDetailVM;
using UPA.ViewModels.ViewModels.BrandVM;

namespace UPA.BLL.Repository
{
    public class AssetDetailRepositories : IAssetDetailService
    {
        private UPAModel _context;
        public AssetDetailRepositories(UPAModel context)
        {
            _context = context;
        }

        private IQueryable<AssetDetail> GetLstAssetDetails()
        {
            return _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.MasterAsset.Brand)

                         .Include(a => a.Hospital).ThenInclude(h => h.Organization)
                         .Include(a => a.Hospital).ThenInclude(h => h.Governorate)
                         .Include(a => a.Hospital).ThenInclude(h => h.City)
                         .Include(a => a.Hospital).ThenInclude(h => h.SubOrganization).OrderBy(q => q.Barcode);
        }


        public IndexAssetDetailVM GetAssetsByUserIdAndPaging(int pageNumber, int pageSize)
        {
            IndexAssetDetailVM mainClass = new IndexAssetDetailVM();
            List<IndexAssetDetailVM.GetData> list = new List<IndexAssetDetailVM.GetData>();




            mainClass.Count = _context.AssetDetails.Count();

            var lstAllAssets = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Hospital)

                                .Include(a => a.Hospital.Governorate).Include(a => a.Hospital.City)
                                .Include(a => a.Hospital.Organization).Include(a => a.Hospital.SubOrganization)
                                .OrderBy(a => a.Barcode).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            if (lstAllAssets.Count > 0)
            {
                foreach (var asset in lstAllAssets)
                {
                    IndexAssetDetailVM.GetData detail = new IndexAssetDetailVM.GetData();


                    detail.Id = asset.Id;

                    detail.Code = asset.Code;

                    detail.Price = asset.Price;
                    detail.BarCode = asset.Barcode;
                    detail.MasterImg = asset.MasterAsset.AssetImg;
                    detail.Serial = asset.SerialNumber;
                    detail.BrandName = asset.MasterAsset.Brand != null ? asset.MasterAsset.Brand.Name : "";
                    detail.BrandNameAr = asset.MasterAsset.Brand != null ? asset.MasterAsset.Brand.NameAr : "";
                    detail.Model = asset.MasterAsset.ModelNumber;
                    detail.SerialNumber = asset.SerialNumber;
                    detail.MasterAssetId = asset.MasterAssetId;
                    detail.PurchaseDate = asset.PurchaseDate;
                    detail.HospitalId = asset.Hospital.Id;
                    detail.HospitalName = asset.Hospital.Name;
                    detail.HospitalNameAr = asset.Hospital.NameAr;
                    detail.AssetName = asset.MasterAsset.Name;
                    detail.AssetNameAr = asset.MasterAsset.NameAr;

                    detail.GovernorateId = asset.Hospital.GovernorateId;
                    detail.GovernorateName = asset.Hospital.Governorate.Name;
                    detail.GovernorateNameAr = asset.Hospital.Governorate.NameAr;
                    detail.CityId = asset.Hospital.CityId;
                    detail.CityName = asset.Hospital.City != null ? asset.Hospital.City.Name:"";
                    detail.CityNameAr = asset.Hospital.City != null ? asset.Hospital.City.NameAr : "";
                    detail.OrganizationId = asset.Hospital.OrganizationId;
                    detail.OrgName = asset.Hospital.Organization.Name;
                    detail.OrgNameAr = asset.Hospital.Organization.NameAr;
                    detail.SubOrganizationId = asset.Hospital.SubOrganizationId;
                    detail.SubOrgName = asset.Hospital.SubOrganization.Name;
                    detail.SubOrgNameAr = asset.Hospital.SubOrganization.NameAr;
                    detail.SupplierName = asset.Supplier != null ? asset.Supplier.Name : "";
                    detail.SupplierNameAr = asset.Supplier != null ? asset.Supplier.NameAr : "";
                    detail.QrFilePath = asset.QrFilePath;
                    list.Add(detail);
                }
            }

            mainClass.Results = list;

            return mainClass;
        
        
        
        }
        public IndexAssetDetailVM GetAssetsByBrandId(int brandId)
        {


            IndexAssetDetailVM mainClass = new IndexAssetDetailVM();

            #region New Code 
            var AssetDetails = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.MasterAsset.Brand).Include(a => a.Hospital)
               .Include(a => a.Supplier).Include(a => a.Hospital.Governorate).Include(a => a.Hospital.City)
               .Include(a => a.Hospital.Organization).Include(a => a.Hospital.SubOrganization).Where(a => a.MasterAsset.BrandId == brandId)
               .Select(Ass => new IndexAssetDetailVM.GetData
               {
                   Id = Ass.Id,
                   AssetName = Ass.MasterAsset.Name,
                   BarCode = Ass.Barcode,
                   SerialNumber = Ass.SerialNumber,
                   Model = Ass.MasterAsset.ModelNumber,
               
                   AssetNameAr = Ass.MasterAsset.NameAr,
                   BrandName = Ass.MasterAsset.Brand.Name,
                   BrandNameAr = Ass.MasterAsset.Brand.NameAr,
                   GovernorateName = Ass.Hospital.Governorate.Name,
                   GovernorateNameAr = Ass.Hospital.Governorate.NameAr,
                   CityName = Ass.Hospital.City.Name,
                   CityNameAr = Ass.Hospital.City.NameAr,
                   HospitalId = Ass.HospitalId,
                   HospitalName = Ass.Hospital.Name,
                   HospitalNameAr = Ass.Hospital.NameAr,
                   SupplierName = Ass.Supplier.Name,
                   SupplierNameAr = Ass.Supplier.NameAr,
                   OrgName = Ass.Hospital.Organization.Name,
                   OrgNameAr = Ass.Hospital.Organization.NameAr,
                   PurchaseDate = Ass.PurchaseDate,
                   BrandId = Ass.MasterAsset.BrandId

               }).ToList();

            mainClass.Results = AssetDetails;
            mainClass.Count = AssetDetails.Count;
            return mainClass;
            #endregion



        }


        public IndexAssetDetailVM GetAssetsByGovId(int govId)
        {


            IndexAssetDetailVM mainClass = new IndexAssetDetailVM();

            #region New Code 
            var AssetDetails = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.MasterAsset.Brand).Include(a => a.Hospital)
               .Include(a => a.Supplier).Include(a => a.Hospital.Governorate).Include(a => a.Hospital.City)
               .Include(a => a.Hospital.Organization).Include(a => a.Hospital.SubOrganization).Where(a => a.Hospital.GovernorateId == govId)
               .Select(Ass => new IndexAssetDetailVM.GetData
               {
                   Id = Ass.Id,
                   AssetName = Ass.MasterAsset.Name,
                   BarCode = Ass.Barcode,
                   SerialNumber = Ass.SerialNumber,
                   Model = Ass.MasterAsset.ModelNumber,

                   AssetNameAr = Ass.MasterAsset.NameAr,
                   BrandName = Ass.MasterAsset.Brand.Name,
                   BrandNameAr = Ass.MasterAsset.Brand.NameAr,
                   GovernorateName = Ass.Hospital.Governorate.Name,
                   GovernorateNameAr = Ass.Hospital.Governorate.NameAr,
                   CityName = Ass.Hospital.City.Name,
                   CityNameAr = Ass.Hospital.City.NameAr,
                   HospitalId = Ass.HospitalId,
                   HospitalName = Ass.Hospital.Name,
                   HospitalNameAr = Ass.Hospital.NameAr,
                   SupplierName = Ass.Supplier.Name,
                   SupplierNameAr = Ass.Supplier.NameAr,
                   OrgName = Ass.Hospital.Organization.Name,
                   OrgNameAr = Ass.Hospital.Organization.NameAr,
                   PurchaseDate = Ass.PurchaseDate,
                   BrandId = Ass.MasterAsset.BrandId

               }).ToList();

            mainClass.Results = AssetDetails;
            mainClass.Count = AssetDetails.Count;
            return mainClass;
            #endregion



        }
        public IndexAssetDetailVM GetAssetsByHosId(int hosId)
        {


            IndexAssetDetailVM mainClass = new IndexAssetDetailVM();

            #region New Code 
            var AssetDetails = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.MasterAsset.Brand).Include(a => a.Hospital)
               .Include(a => a.Supplier).Include(a => a.Hospital.Governorate).Include(a => a.Hospital.City)
               .Include(a => a.Hospital.Organization).Include(a => a.Hospital.SubOrganization).Where(a => a.HospitalId == hosId)
               .Select(Ass => new IndexAssetDetailVM.GetData
               {
                   Id = Ass.Id,
                   AssetName = Ass.MasterAsset.Name,
                   BarCode = Ass.Barcode,
                   SerialNumber = Ass.SerialNumber,
                   Model = Ass.MasterAsset.ModelNumber,

                   AssetNameAr = Ass.MasterAsset.NameAr,
                   BrandName = Ass.MasterAsset.Brand.Name,
                   BrandNameAr = Ass.MasterAsset.Brand.NameAr,
                   GovernorateName = Ass.Hospital.Governorate.Name,
                   GovernorateNameAr = Ass.Hospital.Governorate.NameAr,
                   CityName = Ass.Hospital.City.Name,
                   CityNameAr = Ass.Hospital.City.NameAr,
                   HospitalId = Ass.HospitalId,
                   HospitalName = Ass.Hospital.Name,
                   HospitalNameAr = Ass.Hospital.NameAr,
                   SupplierName = Ass.Supplier.Name,
                   SupplierNameAr = Ass.Supplier.NameAr,
                   OrgName = Ass.Hospital.Organization.Name,
                   OrgNameAr = Ass.Hospital.Organization.NameAr,
                   PurchaseDate = Ass.PurchaseDate,
                   BrandId = Ass.MasterAsset.BrandId

               }).ToList();

            mainClass.Results = AssetDetails;
            mainClass.Count = AssetDetails.Count;
            return mainClass;
            #endregion



        }







        public IndexAssetDetailVM GetAssetsByOrgId(int orgId)
        {


            IndexAssetDetailVM mainClass = new IndexAssetDetailVM();

            #region New Code 
            var AssetDetails = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.MasterAsset.Brand).Include(a => a.Hospital)
               .Include(a => a.Supplier).Include(a => a.Hospital.Governorate).Include(a => a.Hospital.City)
               .Include(a => a.Hospital.Organization).Include(a => a.Hospital.SubOrganization).Where(a => a.Hospital.OrganizationId == orgId)
               .Select(Ass => new IndexAssetDetailVM.GetData
               {
                   Id = Ass.Id,
                   AssetName = Ass.MasterAsset.Name,
                   BarCode = Ass.Barcode,
                   SerialNumber = Ass.SerialNumber,
                   Model = Ass.MasterAsset.ModelNumber,

                   AssetNameAr = Ass.MasterAsset.NameAr,
                   BrandName = Ass.MasterAsset.Brand.Name,
                   BrandNameAr = Ass.MasterAsset.Brand.NameAr,
                   GovernorateName = Ass.Hospital.Governorate.Name,
                   GovernorateNameAr = Ass.Hospital.Governorate.NameAr,
                   CityName = Ass.Hospital.City.Name,
                   CityNameAr = Ass.Hospital.City.NameAr,
                   HospitalId = Ass.HospitalId,
                   HospitalName = Ass.Hospital.Name,
                   HospitalNameAr = Ass.Hospital.NameAr,
                   SupplierName = Ass.Supplier.Name,
                   SupplierNameAr = Ass.Supplier.NameAr,
                   OrgName = Ass.Hospital.Organization.Name,
                   OrgNameAr = Ass.Hospital.Organization.NameAr,
                   PurchaseDate = Ass.PurchaseDate,
                   BrandId = Ass.MasterAsset.BrandId

               }).ToList();

            mainClass.Results = AssetDetails;
            mainClass.Count = AssetDetails.Count;
            return mainClass;
            #endregion



        }





        public IndexAssetDetailVM GetAssetsBySubOrgId(int subOrgId)
        {


            IndexAssetDetailVM mainClass = new IndexAssetDetailVM();

            #region New Code 
            var AssetDetails = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.MasterAsset.Brand).Include(a => a.Hospital)
               .Include(a => a.Supplier).Include(a => a.Hospital.Governorate).Include(a => a.Hospital.City)
               .Include(a => a.Hospital.Organization).Include(a => a.Hospital.SubOrganization).Where(a => a.Hospital.SubOrganizationId == subOrgId)
               .Select(Ass => new IndexAssetDetailVM.GetData
               {
                   Id = Ass.Id,
                   AssetName = Ass.MasterAsset.Name,
                   BarCode = Ass.Barcode,
                   SerialNumber = Ass.SerialNumber,
                   Model = Ass.MasterAsset.ModelNumber,

                   AssetNameAr = Ass.MasterAsset.NameAr,
                   BrandName = Ass.MasterAsset.Brand.Name,
                   BrandNameAr = Ass.MasterAsset.Brand.NameAr,
                   GovernorateName = Ass.Hospital.Governorate.Name,
                   GovernorateNameAr = Ass.Hospital.Governorate.NameAr,
                   CityName = Ass.Hospital.City.Name,
                   CityNameAr = Ass.Hospital.City.NameAr,
                   HospitalId = Ass.HospitalId,
                   HospitalName = Ass.Hospital.Name,
                   HospitalNameAr = Ass.Hospital.NameAr,
                   SupplierName = Ass.Supplier.Name,
                   SupplierNameAr = Ass.Supplier.NameAr,
                   OrgName = Ass.Hospital.Organization.Name,
                   OrgNameAr = Ass.Hospital.Organization.NameAr,
                   PurchaseDate = Ass.PurchaseDate,
                   BrandId = Ass.MasterAsset.BrandId

               }).ToList();

            mainClass.Results = AssetDetails;
            mainClass.Count = AssetDetails.Count;
            return mainClass;
            #endregion



        }

        public IndexAssetDetailVM SortAssetDetailAfterSearch(SortAndFilterDataModel data, int pageNumber, int pageSize)
        {


            IQueryable<AssetDetail> query = null;
            IndexAssetDetailVM mainClass = new IndexAssetDetailVM();
            List<IndexAssetDetailVM.GetData> list = new List<IndexAssetDetailVM.GetData>();
            List<AssetDetail> searchResult = new List<AssetDetail>();


            if (data.FilteredObj.AssetNameAr != "")
            {
                if (query is not null)
                {


                    if (data.SortObject.SortBy != "")
                    {
                        switch (data.SortObject.SortBy)
                        {
                            case "الاسم":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    // filteration and sort then pagination 
                                    // Where() , OrderBy() then Skip() Take()
                                    query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                      OrderBy(ww => ww.MasterAsset.NameAr);
                                }
                                else
                                {
                                    query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                  OrderByDescending(ww => ww.MasterAsset.NameAr);
                                }
                                break;

                            case "Name":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                    OrderBy(ww => ww.MasterAsset.Name);

                                }
                                else
                                {

                                    query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                        OrderByDescending(ww => ww.MasterAsset.Name);

                                }

                                break;

                            case "الباركود":
                            case "Barcode":

                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                    OrderBy(ww => ww.Barcode);


                                }
                                else
                                {
                                    query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                      OrderByDescending(ww => ww.Barcode);

                                }

                                break;

                            case "السيريال":
                            case "Serial":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                    OrderBy(ww => ww.SerialNumber);

                                }
                                else
                                {
                                    query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                   OrderByDescending(ww => ww.SerialNumber);
                                }

                                break;

                            case "رقم الموديل":
                            case "Model Number":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                         OrderBy(ww => ww.MasterAsset.ModelNumber);
                                }
                                else
                                {
                                    query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                         OrderByDescending(ww => ww.MasterAsset.ModelNumber);
                                }
                                break;
                            case "القسم":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                      OrderBy(ww => ww.DepartmentId);
                                }
                                else
                                {
                                    query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                                    OrderByDescending(ww => ww.DepartmentId);

                                }
                                break;


                            case "Department":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                         OrderBy(ww => ww.DepartmentId);

                                }
                                else
                                {
                                    query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                                   OrderByDescending(ww => ww.DepartmentId);
                                }
                                break;

                            case "الماركة":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                        OrderBy(ww => ww.MasterAsset.Brand.NameAr);


                                }
                                else
                                {
                                    query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                       OrderByDescending(ww => ww.MasterAsset.Brand.NameAr);
                                }
                                break;
                            case "Manufacture":
                                if (data.SortObject.SortStatus == "ascending")

                                {


                                    query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                      OrderBy(ww => ww.MasterAsset.Brand.Name);


                                }
                                else
                                {
                                    query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                OrderByDescending(ww => ww.MasterAsset.Brand.Name);
                                }
                                break;
                            case "المورد":
                                if (data.SortObject.SortStatus == "ascending")
                                {

                                    query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                      OrderBy(ww => ww.SupplierId);
                                }
                                else
                                {
                                    query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                    OrderByDescending(ww => ww.SupplierId);
                                }
                                break;
                            case "Supplier":
                                if (data.SortObject.SortStatus == "ascending")
                                {

                                    query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                      OrderBy(ww => ww.SupplierId);
                                }
                                else
                                {
                                    query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                              OrderByDescending(ww => ww.SupplierId);

                                }
                                break;
                            case "تاريخ الشراء":
                            case "Purchased Date":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                   OrderBy(ww => ww.PurchaseDate);
                                }
                                else
                                {
                                    query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                   OrderByDescending(ww => ww.PurchaseDate);
                                }
                                break;



                        }
                    }


                }

                else
                {

                    if (data.SortObject.SortBy != "")
                    {
                        switch (data.SortObject.SortBy)
                        {
                            case "الاسم":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    // filteration and sort then pagination 
                                    // Where() , OrderBy() then Skip() Take()
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                      OrderBy(ww => ww.MasterAsset.NameAr);
                                }
                                else
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                  OrderByDescending(ww => ww.MasterAsset.NameAr);
                                }
                                break;

                            case "Name":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                    OrderBy(ww => ww.MasterAsset.Name);

                                }
                                else
                                {

                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                        OrderByDescending(ww => ww.MasterAsset.Name);

                                }

                                break;

                            case "الباركود":
                            case "Barcode":

                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                    OrderBy(ww => ww.Barcode);


                                }
                                else
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                      OrderByDescending(ww => ww.Barcode);

                                }

                                break;

                            case "السيريال":
                            case "Serial":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                    OrderBy(ww => ww.SerialNumber);

                                }
                                else
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                   OrderByDescending(ww => ww.SerialNumber);
                                }

                                break;

                            case "رقم الموديل":
                            case "Model Number":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                         OrderBy(ww => ww.MasterAsset.ModelNumber);
                                }
                                else
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                         OrderByDescending(ww => ww.MasterAsset.ModelNumber);
                                }
                                break;
                            case "القسم":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                      OrderBy(ww => ww.DepartmentId);
                                }
                                else
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                                    OrderByDescending(ww => ww.DepartmentId);

                                }
                                break;


                            case "Department":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                         OrderBy(ww => ww.DepartmentId);

                                }
                                else
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                                   OrderByDescending(ww => ww.DepartmentId);
                                }
                                break;

                            case "الماركة":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                        OrderBy(ww => ww.MasterAsset.Brand.NameAr);


                                }
                                else
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                       OrderByDescending(ww => ww.MasterAsset.Brand.NameAr);
                                }
                                break;
                            case "Manufacture":
                                if (data.SortObject.SortStatus == "ascending")

                                {


                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                      OrderBy(ww => ww.MasterAsset.Brand.Name);


                                }
                                else
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                OrderByDescending(ww => ww.MasterAsset.Brand.Name);
                                }
                                break;
                            case "المورد":
                                if (data.SortObject.SortStatus == "ascending")
                                {

                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                      OrderBy(ww => ww.SupplierId);
                                }
                                else
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                    OrderByDescending(ww => ww.SupplierId);
                                }
                                break;
                            case "Supplier":
                                if (data.SortObject.SortStatus == "ascending")
                                {

                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                      OrderBy(ww => ww.SupplierId);
                                }
                                else
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                              OrderByDescending(ww => ww.SupplierId);

                                }
                                break;
                            case "تاريخ الشراء":
                            case "Purchased Date":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                   OrderBy(ww => ww.PurchaseDate);
                                }
                                else
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                   OrderByDescending(ww => ww.PurchaseDate);
                                }
                                break;



                        }
                    }






                }

                mainClass.Count = query.Count();
                searchResult = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            }

            if (data.FilteredObj.AssetName != "")
            {
                if (query is not null)
                {
                    if (data.SortObject.SortBy != "")
                    {
                        switch (data.SortObject.SortBy)
                        {
                            case "الاسم":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    // filteration and sort then pagination 
                                    // Where() , OrderBy() then Skip() Take()
                                    query = query.Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                      OrderBy(ww => ww.MasterAsset.NameAr);
                                }
                                else
                                {
                                    query = query.Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                  OrderByDescending(ww => ww.MasterAsset.NameAr);
                                }
                                break;

                            case "Name":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = query.Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                    OrderBy(ww => ww.MasterAsset.Name);

                                }
                                else
                                {

                                    query = query.Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                        OrderByDescending(ww => ww.MasterAsset.Name);

                                }

                                break;

                            case "الباركود":
                            case "Barcode":

                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = query.Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                    OrderBy(ww => ww.Barcode);


                                }
                                else
                                {
                                    query = query.Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                      OrderByDescending(ww => ww.Barcode);

                                }

                                break;

                            case "السيريال":
                            case "Serial":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = query.Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                    OrderBy(ww => ww.SerialNumber);

                                }
                                else
                                {
                                    query = query.Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                   OrderByDescending(ww => ww.SerialNumber);
                                }

                                break;

                            case "رقم الموديل":
                            case "Model Number":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = query.Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                         OrderBy(ww => ww.MasterAsset.ModelNumber);
                                }
                                else
                                {
                                    query = query.Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                         OrderByDescending(ww => ww.MasterAsset.ModelNumber);
                                }
                                break;
                            case "القسم":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = query.Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                      OrderBy(ww => ww.DepartmentId);
                                }
                                else
                                {
                                    query = query.Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                                    OrderByDescending(ww => ww.DepartmentId);

                                }
                                break;


                            case "Department":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = query.Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                         OrderBy(ww => ww.DepartmentId);

                                }
                                else
                                {
                                    query = query.Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                                   OrderByDescending(ww => ww.DepartmentId);
                                }
                                break;

                            case "الماركة":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = query.Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                        OrderBy(ww => ww.MasterAsset.Brand.NameAr);


                                }
                                else
                                {
                                    query = query.Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                       OrderByDescending(ww => ww.MasterAsset.Brand.NameAr);
                                }
                                break;
                            case "Manufacture":
                                if (data.SortObject.SortStatus == "ascending")

                                {


                                    query = query.Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                      OrderBy(ww => ww.MasterAsset.Brand.Name);


                                }
                                else
                                {
                                    query = query.Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                OrderByDescending(ww => ww.MasterAsset.Brand.Name);
                                }
                                break;
                            case "المورد":
                                if (data.SortObject.SortStatus == "ascending")
                                {

                                    query = query.Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                      OrderBy(ww => ww.SupplierId);
                                }
                                else
                                {
                                    query = query.Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                    OrderByDescending(ww => ww.SupplierId);
                                }
                                break;
                            case "Supplier":
                                if (data.SortObject.SortStatus == "ascending")
                                {

                                    query = query.Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                      OrderBy(ww => ww.SupplierId);
                                }
                                else
                                {
                                    query = query.Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                              OrderByDescending(ww => ww.SupplierId);

                                }
                                break;
                            case "تاريخ الشراء":
                            case "Purchased Date":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = query.Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                   OrderBy(ww => ww.PurchaseDate);
                                }
                                else
                                {
                                    query = query.Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                   OrderByDescending(ww => ww.PurchaseDate);
                                }
                                break;



                        }
                    }

                }
                else
                {

                    if (data.SortObject.SortBy != "")
                    {
                        switch (data.SortObject.SortBy)
                        {
                            case "الاسم":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    // filteration and sort then pagination 
                                    // Where() , OrderBy() then Skip() Take()
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                      OrderBy(ww => ww.MasterAsset.NameAr);
                                }
                                else
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                  OrderByDescending(ww => ww.MasterAsset.NameAr);
                                }
                                break;

                            case "Name":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                    OrderBy(ww => ww.MasterAsset.Name);

                                }
                                else
                                {

                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                        OrderByDescending(ww => ww.MasterAsset.Name);

                                }

                                break;

                            case "الباركود":
                            case "Barcode":

                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                    OrderBy(ww => ww.Barcode);


                                }
                                else
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                      OrderByDescending(ww => ww.Barcode);

                                }

                                break;

                            case "السيريال":
                            case "Serial":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                    OrderBy(ww => ww.SerialNumber);

                                }
                                else
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                   OrderByDescending(ww => ww.SerialNumber);
                                }

                                break;

                            case "رقم الموديل":
                            case "Model Number":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                         OrderBy(ww => ww.MasterAsset.ModelNumber);
                                }
                                else
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                         OrderByDescending(ww => ww.MasterAsset.ModelNumber);
                                }
                                break;
                            case "القسم":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                      OrderBy(ww => ww.DepartmentId);
                                }
                                else
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                                    OrderByDescending(ww => ww.DepartmentId);

                                }
                                break;


                            case "Department":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                         OrderBy(ww => ww.DepartmentId);

                                }
                                else
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                                   OrderByDescending(ww => ww.DepartmentId);
                                }
                                break;

                            case "الماركة":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                        OrderBy(ww => ww.MasterAsset.Brand.NameAr);


                                }
                                else
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                       OrderByDescending(ww => ww.MasterAsset.Brand.NameAr);
                                }
                                break;
                            case "Manufacture":
                                if (data.SortObject.SortStatus == "ascending")

                                {


                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                      OrderBy(ww => ww.MasterAsset.Brand.Name);


                                }
                                else
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                OrderByDescending(ww => ww.MasterAsset.Brand.Name);
                                }
                                break;
                            case "المورد":
                                if (data.SortObject.SortStatus == "ascending")
                                {

                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                      OrderBy(ww => ww.SupplierId);
                                }
                                else
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                    OrderByDescending(ww => ww.SupplierId);
                                }
                                break;
                            case "Supplier":
                                if (data.SortObject.SortStatus == "ascending")
                                {

                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                      OrderBy(ww => ww.SupplierId);
                                }
                                else
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                              OrderByDescending(ww => ww.SupplierId);

                                }
                                break;
                            case "تاريخ الشراء":
                            case "Purchased Date":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                   OrderBy(ww => ww.PurchaseDate);
                                }
                                else
                                {
                                    query = GetLstAssetDetails().Where(e => e.MasterAsset.Name == data.FilteredObj.AssetName).
                                   OrderByDescending(ww => ww.PurchaseDate);
                                }
                                break;



                        }
                    }



                }

                mainClass.Count = query.Count();
                searchResult = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            }



            if (data.FilteredObj.DepartmentId != 0)
            {
                if (query is not null)
                {
                    if (data.SortObject.SortBy != "")
                    {
                        switch (data.SortObject.SortBy)
                        {
                            case "الاسم":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    // filteration and sort then pagination 
                                    // Where() , OrderBy() then Skip() Take()
                                    query = query.Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                      OrderBy(ww => ww.MasterAsset.NameAr);
                                }
                                else
                                {
                                    query = query.Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                  OrderByDescending(ww => ww.MasterAsset.NameAr);
                                }
                                break;

                            case "Name":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = query.Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                    OrderBy(ww => ww.MasterAsset.Name);

                                }
                                else
                                {

                                    query = query.Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                        OrderByDescending(ww => ww.MasterAsset.Name);

                                }

                                break;

                            case "الباركود":
                            case "Barcode":

                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = query.Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                    OrderBy(ww => ww.Barcode);


                                }
                                else
                                {
                                    query = query.Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                      OrderByDescending(ww => ww.Barcode);

                                }

                                break;

                            case "السيريال":
                            case "Serial":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = query.Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                    OrderBy(ww => ww.SerialNumber);

                                }
                                else
                                {
                                    query = query.Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                   OrderByDescending(ww => ww.SerialNumber);
                                }

                                break;

                            case "رقم الموديل":
                            case "Model Number":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = query.Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                         OrderBy(ww => ww.MasterAsset.ModelNumber);
                                }
                                else
                                {
                                    query = query.Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                         OrderByDescending(ww => ww.MasterAsset.ModelNumber);
                                }
                                break;
                            case "القسم":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = query.Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                      OrderBy(ww => ww.DepartmentId);
                                }
                                else
                                {
                                    query = query.Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                                    OrderByDescending(ww => ww.DepartmentId);

                                }
                                break;


                            case "Department":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = query.Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                         OrderBy(ww => ww.DepartmentId);

                                }
                                else
                                {
                                    query = query.Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                                   OrderByDescending(ww => ww.DepartmentId);
                                }
                                break;

                            case "الماركة":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = query.Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                        OrderBy(ww => ww.MasterAsset.Brand.NameAr);


                                }
                                else
                                {
                                    query = query.Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                       OrderByDescending(ww => ww.MasterAsset.Brand.NameAr);
                                }
                                break;
                            case "Manufacture":
                                if (data.SortObject.SortStatus == "ascending")

                                {


                                    query = query.Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                      OrderBy(ww => ww.MasterAsset.Brand.Name);


                                }
                                else
                                {
                                    query = query.Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                OrderByDescending(ww => ww.MasterAsset.Brand.Name);
                                }
                                break;
                            case "المورد":
                                if (data.SortObject.SortStatus == "ascending")
                                {

                                    query = query.Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                      OrderBy(ww => ww.SupplierId);
                                }
                                else
                                {
                                    query = query.Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                    OrderByDescending(ww => ww.SupplierId);
                                }
                                break;
                            case "Supplier":
                                if (data.SortObject.SortStatus == "ascending")
                                {

                                    query = query.Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                      OrderBy(ww => ww.SupplierId);
                                }
                                else
                                {
                                    query = query.Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                              OrderByDescending(ww => ww.SupplierId);

                                }
                                break;
                            case "تاريخ الشراء":
                            case "Purchased Date":
                                if (data.SortObject.SortStatus == "ascending")
                                {
                                    query = query.Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                   OrderBy(ww => ww.PurchaseDate);
                                }
                                else
                                {
                                    query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                                   OrderByDescending(ww => ww.PurchaseDate);
                                }
                                break;



                        }
                    }

                }
                else

                {
                    switch (data.SortObject.SortBy)
                    {
                        case "الاسم":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                // filteration and sort then pagination 
                                // Where() , OrderBy() then Skip() Take()
                                query = GetLstAssetDetails().Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                  OrderBy(ww => ww.MasterAsset.NameAr);
                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                              OrderByDescending(ww => ww.MasterAsset.NameAr);
                            }
                            break;

                        case "Name":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                OrderBy(ww => ww.MasterAsset.Name);

                            }
                            else
                            {

                                query = GetLstAssetDetails().Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                    OrderByDescending(ww => ww.MasterAsset.Name);

                            }

                            break;

                        case "الباركود":
                        case "Barcode":

                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                OrderBy(ww => ww.Barcode);


                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                  OrderByDescending(ww => ww.Barcode);

                            }

                            break;

                        case "السيريال":
                        case "Serial":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                OrderBy(ww => ww.SerialNumber);

                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                               OrderByDescending(ww => ww.SerialNumber);
                            }

                            break;

                        case "رقم الموديل":
                        case "Model Number":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                     OrderBy(ww => ww.MasterAsset.ModelNumber);
                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                     OrderByDescending(ww => ww.MasterAsset.ModelNumber);
                            }
                            break;
                        case "القسم":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                  OrderBy(ww => ww.DepartmentId);
                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                                OrderByDescending(ww => ww.DepartmentId);

                            }
                            break;


                        case "Department":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                     OrderBy(ww => ww.DepartmentId);

                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                               OrderByDescending(ww => ww.DepartmentId);
                            }
                            break;

                        case "الماركة":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                    OrderBy(ww => ww.MasterAsset.Brand.NameAr);


                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                   OrderByDescending(ww => ww.MasterAsset.Brand.NameAr);
                            }
                            break;
                        case "Manufacture":
                            if (data.SortObject.SortStatus == "ascending")

                            {


                                query = GetLstAssetDetails().Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                  OrderBy(ww => ww.MasterAsset.Brand.Name);


                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                            OrderByDescending(ww => ww.MasterAsset.Brand.Name);
                            }
                            break;
                        case "المورد":
                            if (data.SortObject.SortStatus == "ascending")
                            {

                                query = GetLstAssetDetails().Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                  OrderBy(ww => ww.SupplierId);
                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                OrderByDescending(ww => ww.SupplierId);
                            }
                            break;
                        case "Supplier":
                            if (data.SortObject.SortStatus == "ascending")
                            {

                                query = GetLstAssetDetails().Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                                  OrderBy(ww => ww.SupplierId);
                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                          OrderByDescending(ww => ww.SupplierId);

                            }
                            break;
                        case "تاريخ الشراء":
                        case "Purchased Date":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                               OrderBy(ww => ww.PurchaseDate);
                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.DepartmentId == data.FilteredObj.DepartmentId).
                               OrderByDescending(ww => ww.PurchaseDate);
                            }
                            break;



                    }

                }

                mainClass.Count = query.Count();
                searchResult = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


            }


            if (data.FilteredObj.BrandId != 0)
            {
                if (query is not null)
                {
                    switch (data.SortObject.SortBy)
                    {
                        case "الاسم":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                // filteration and sort then pagination 
                                // Where() , OrderBy() then Skip() Take()
                                query = query.Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                  OrderBy(ww => ww.MasterAsset.NameAr);
                            }
                            else
                            {
                                query = query.Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                              OrderByDescending(ww => ww.MasterAsset.NameAr);
                            }
                            break;

                        case "Name":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = query.Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                OrderBy(ww => ww.MasterAsset.Name);

                            }
                            else
                            {

                                query = query.Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                    OrderByDescending(ww => ww.MasterAsset.Name);

                            }

                            break;

                        case "الباركود":
                        case "Barcode":

                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = query.Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                OrderBy(ww => ww.Barcode);


                            }
                            else
                            {
                                query = query.Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                  OrderByDescending(ww => ww.Barcode);

                            }

                            break;

                        case "السيريال":
                        case "Serial":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = query.Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                OrderBy(ww => ww.SerialNumber);

                            }
                            else
                            {
                                query = query.Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                               OrderByDescending(ww => ww.SerialNumber);
                            }

                            break;

                        case "رقم الموديل":
                        case "Model Number":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = query.Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                     OrderBy(ww => ww.MasterAsset.ModelNumber);
                            }
                            else
                            {
                                query = query.Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                     OrderByDescending(ww => ww.MasterAsset.ModelNumber);
                            }
                            break;
                        case "القسم":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = query.Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                  OrderBy(ww => ww.DepartmentId);
                            }
                            else
                            {
                                query = query.Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                                OrderByDescending(ww => ww.DepartmentId);

                            }
                            break;


                        case "Department":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = query.Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                     OrderBy(ww => ww.DepartmentId);

                            }
                            else
                            {
                                query = query.Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                               OrderByDescending(ww => ww.DepartmentId);
                            }
                            break;

                        case "الماركة":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = query.Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                    OrderBy(ww => ww.MasterAsset.Brand.NameAr);


                            }
                            else
                            {
                                query = query.Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                   OrderByDescending(ww => ww.MasterAsset.Brand.NameAr);
                            }
                            break;
                        case "Manufacture":
                            if (data.SortObject.SortStatus == "ascending")

                            {


                                query = query.Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                  OrderBy(ww => ww.MasterAsset.Brand.Name);


                            }
                            else
                            {
                                query = query.Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                            OrderByDescending(ww => ww.MasterAsset.Brand.Name);
                            }
                            break;
                        case "المورد":
                            if (data.SortObject.SortStatus == "ascending")
                            {

                                query = query.Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                  OrderBy(ww => ww.SupplierId);
                            }
                            else
                            {
                                query = query.Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                OrderByDescending(ww => ww.SupplierId);
                            }
                            break;
                        case "Supplier":
                            if (data.SortObject.SortStatus == "ascending")
                            {

                                query = query.Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                  OrderBy(ww => ww.SupplierId);
                            }
                            else
                            {
                                query = query.Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                          OrderByDescending(ww => ww.SupplierId);

                            }
                            break;
                        case "تاريخ الشراء":
                        case "Purchased Date":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = query.Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                               OrderBy(ww => ww.PurchaseDate);
                            }
                            else
                            {
                                query = query.Where(e => e.MasterAsset.NameAr == data.FilteredObj.AssetNameAr).
                               OrderByDescending(ww => ww.PurchaseDate);
                            }
                            break;



                    }

                }

                else
                {
                    switch (data.SortObject.SortBy)
                    {
                        case "الاسم":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                // filteration and sort then pagination 
                                // Where() , OrderBy() then Skip() Take()
                                query = GetLstAssetDetails().Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                  OrderBy(ww => ww.MasterAsset.NameAr);
                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                              OrderByDescending(ww => ww.MasterAsset.NameAr);
                            }
                            break;

                        case "Name":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                OrderBy(ww => ww.MasterAsset.Name);

                            }
                            else
                            {

                                query = GetLstAssetDetails().Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                    OrderByDescending(ww => ww.MasterAsset.Name);

                            }

                            break;

                        case "الباركود":
                        case "Barcode":

                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                OrderBy(ww => ww.Barcode);


                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                  OrderByDescending(ww => ww.Barcode);

                            }

                            break;

                        case "السيريال":
                        case "Serial":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                OrderBy(ww => ww.SerialNumber);

                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                               OrderByDescending(ww => ww.SerialNumber);
                            }

                            break;

                        case "رقم الموديل":
                        case "Model Number":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                     OrderBy(ww => ww.MasterAsset.ModelNumber);
                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                     OrderByDescending(ww => ww.MasterAsset.ModelNumber);
                            }
                            break;
                        case "القسم":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                  OrderBy(ww => ww.DepartmentId);
                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                                OrderByDescending(ww => ww.DepartmentId);

                            }
                            break;


                        case "Department":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                     OrderBy(ww => ww.DepartmentId);

                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                               OrderByDescending(ww => ww.DepartmentId);
                            }
                            break;

                        case "الماركة":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                    OrderBy(ww => ww.MasterAsset.Brand.NameAr);


                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                   OrderByDescending(ww => ww.MasterAsset.Brand.NameAr);
                            }
                            break;
                        case "Manufacture":
                            if (data.SortObject.SortStatus == "ascending")

                            {


                                query = GetLstAssetDetails().Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                  OrderBy(ww => ww.MasterAsset.Brand.Name);


                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                            OrderByDescending(ww => ww.MasterAsset.Brand.Name);
                            }
                            break;
                        case "المورد":
                            if (data.SortObject.SortStatus == "ascending")
                            {

                                query = GetLstAssetDetails().Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                  OrderBy(ww => ww.SupplierId);
                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                OrderByDescending(ww => ww.SupplierId);
                            }
                            break;
                        case "Supplier":
                            if (data.SortObject.SortStatus == "ascending")
                            {

                                query = GetLstAssetDetails().Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                                  OrderBy(ww => ww.SupplierId);
                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                          OrderByDescending(ww => ww.SupplierId);

                            }
                            break;
                        case "تاريخ الشراء":
                        case "Purchased Date":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                               OrderBy(ww => ww.PurchaseDate);
                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAsset.BrandId == data.FilteredObj.BrandId).
                               OrderByDescending(ww => ww.PurchaseDate);
                            }
                            break;



                    }
                }

                mainClass.Count = query.Count();
                searchResult = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


            }


            if (data.FilteredObj.SupplierId != 0)
            {
                if (query is not null)
                {
                    switch (data.SortObject.SortBy)
                    {
                        case "الاسم":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                // filteration and sort then pagination 
                                // Where() , OrderBy() then Skip() Take()
                                query = query.Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                  OrderBy(ww => ww.MasterAsset.NameAr);
                            }
                            else
                            {
                                query = query.Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                              OrderByDescending(ww => ww.MasterAsset.NameAr);
                            }
                            break;

                        case "Name":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = query.Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                OrderBy(ww => ww.MasterAsset.Name);

                            }
                            else
                            {

                                query = query.Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                    OrderByDescending(ww => ww.MasterAsset.Name);

                            }

                            break;

                        case "الباركود":
                        case "Barcode":

                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = query.Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                OrderBy(ww => ww.Barcode);


                            }
                            else
                            {
                                query = query.Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                  OrderByDescending(ww => ww.Barcode);

                            }

                            break;

                        case "السيريال":
                        case "Serial":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = query.Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                OrderBy(ww => ww.SerialNumber);

                            }
                            else
                            {
                                query = query.Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                               OrderByDescending(ww => ww.SerialNumber);
                            }

                            break;

                        case "رقم الموديل":
                        case "Model Number":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = query.Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                     OrderBy(ww => ww.MasterAsset.ModelNumber);
                            }
                            else
                            {
                                query = query.Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                     OrderByDescending(ww => ww.MasterAsset.ModelNumber);
                            }
                            break;
                        case "القسم":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = query.Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                  OrderBy(ww => ww.DepartmentId);
                            }
                            else
                            {
                                query = query.Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                                OrderByDescending(ww => ww.DepartmentId);

                            }
                            break;


                        case "Department":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = query.Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                     OrderBy(ww => ww.DepartmentId);

                            }
                            else
                            {
                                query = query.Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                               OrderByDescending(ww => ww.DepartmentId);
                            }
                            break;

                        case "الماركة":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = query.Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                    OrderBy(ww => ww.MasterAsset.Brand.NameAr);


                            }
                            else
                            {
                                query = query.Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                   OrderByDescending(ww => ww.MasterAsset.Brand.NameAr);
                            }
                            break;
                        case "Manufacture":
                            if (data.SortObject.SortStatus == "ascending")

                            {


                                query = query.Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                  OrderBy(ww => ww.MasterAsset.Brand.Name);


                            }
                            else
                            {
                                query = query.Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                            OrderByDescending(ww => ww.MasterAsset.Brand.Name);
                            }
                            break;
                        case "المورد":
                            if (data.SortObject.SortStatus == "ascending")
                            {

                                query = query.Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                  OrderBy(ww => ww.SupplierId);
                            }
                            else
                            {
                                query = query.Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                OrderByDescending(ww => ww.SupplierId);
                            }
                            break;
                        case "Supplier":
                            if (data.SortObject.SortStatus == "ascending")
                            {

                                query = query.Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                  OrderBy(ww => ww.SupplierId);
                            }
                            else
                            {
                                query = query.Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                          OrderByDescending(ww => ww.SupplierId);

                            }
                            break;
                        case "تاريخ الشراء":
                        case "Purchased Date":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = query.Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                               OrderBy(ww => ww.PurchaseDate);
                            }
                            else
                            {
                                query = query.Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                               OrderByDescending(ww => ww.PurchaseDate);
                            }
                            break;



                    }

                }
                else
                {
                    switch (data.SortObject.SortBy)
                    {
                        case "الاسم":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                // filteration and sort then pagination 
                                // Where() , OrderBy() then Skip() Take()
                                query = GetLstAssetDetails().Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                  OrderBy(ww => ww.MasterAsset.NameAr);
                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                              OrderByDescending(ww => ww.MasterAsset.NameAr);
                            }
                            break;

                        case "Name":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                OrderBy(ww => ww.MasterAsset.Name);

                            }
                            else
                            {

                                query = GetLstAssetDetails().Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                    OrderByDescending(ww => ww.MasterAsset.Name);

                            }

                            break;

                        case "الباركود":
                        case "Barcode":

                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                OrderBy(ww => ww.Barcode);


                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                  OrderByDescending(ww => ww.Barcode);

                            }

                            break;

                        case "السيريال":
                        case "Serial":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                OrderBy(ww => ww.SerialNumber);

                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                               OrderByDescending(ww => ww.SerialNumber);
                            }

                            break;

                        case "رقم الموديل":
                        case "Model Number":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                     OrderBy(ww => ww.MasterAsset.ModelNumber);
                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                     OrderByDescending(ww => ww.MasterAsset.ModelNumber);
                            }
                            break;
                        case "القسم":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                  OrderBy(ww => ww.DepartmentId);
                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                                OrderByDescending(ww => ww.DepartmentId);

                            }
                            break;


                        case "Department":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                     OrderBy(ww => ww.DepartmentId);

                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                               OrderByDescending(ww => ww.DepartmentId);
                            }
                            break;

                        case "الماركة":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                    OrderBy(ww => ww.MasterAsset.Brand.NameAr);


                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                   OrderByDescending(ww => ww.MasterAsset.Brand.NameAr);
                            }
                            break;
                        case "Manufacture":
                            if (data.SortObject.SortStatus == "ascending")

                            {


                                query = GetLstAssetDetails().Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                  OrderBy(ww => ww.MasterAsset.Brand.Name);


                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                            OrderByDescending(ww => ww.MasterAsset.Brand.Name);
                            }
                            break;
                        case "المورد":
                            if (data.SortObject.SortStatus == "ascending")
                            {

                                query = GetLstAssetDetails().Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                  OrderBy(ww => ww.SupplierId);
                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                OrderByDescending(ww => ww.SupplierId);
                            }
                            break;
                        case "Supplier":
                            if (data.SortObject.SortStatus == "ascending")
                            {

                                query = GetLstAssetDetails().Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                                  OrderBy(ww => ww.SupplierId);
                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                          OrderByDescending(ww => ww.SupplierId);

                            }
                            break;
                        case "تاريخ الشراء":
                        case "Purchased Date":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                               OrderBy(ww => ww.PurchaseDate);
                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.SupplierId == data.FilteredObj.SupplierId).
                               OrderByDescending(ww => ww.PurchaseDate);
                            }
                            break;



                    }

                }
                mainClass.Count = query.Count();
                searchResult = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            }

            if (data.FilteredObj.MasterAssetId != 0)
            {
                if (query is not null)
                {
                    switch (data.SortObject.SortBy)
                    {
                        case "الاسم":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                // filteration and sort then pagination 
                                // Where() , OrderBy() then Skip() Take()
                                query = query.Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                  OrderBy(ww => ww.MasterAsset.NameAr);
                            }
                            else
                            {
                                query = query.Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                              OrderByDescending(ww => ww.MasterAsset.NameAr);
                            }
                            break;

                        case "Name":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = query.Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                OrderBy(ww => ww.MasterAsset.Name);

                            }
                            else
                            {

                                query = query.Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                    OrderByDescending(ww => ww.MasterAsset.Name);

                            }

                            break;

                        case "الباركود":
                        case "Barcode":

                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = query.Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                OrderBy(ww => ww.Barcode);


                            }
                            else
                            {
                                query = query.Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                  OrderByDescending(ww => ww.Barcode);

                            }

                            break;

                        case "السيريال":
                        case "Serial":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = query.Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                OrderBy(ww => ww.SerialNumber);

                            }
                            else
                            {
                                query = query.Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                               OrderByDescending(ww => ww.SerialNumber);
                            }

                            break;

                        case "رقم الموديل":
                        case "Model Number":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = query.Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                     OrderBy(ww => ww.MasterAsset.ModelNumber);
                            }
                            else
                            {
                                query = query.Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                     OrderByDescending(ww => ww.MasterAsset.ModelNumber);
                            }
                            break;
                        case "القسم":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = query.Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                  OrderBy(ww => ww.DepartmentId);
                            }
                            else
                            {
                                query = query.Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                                OrderByDescending(ww => ww.DepartmentId);

                            }
                            break;


                        case "Department":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = query.Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                     OrderBy(ww => ww.DepartmentId);

                            }
                            else
                            {
                                query = query.Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                               OrderByDescending(ww => ww.DepartmentId);
                            }
                            break;

                        case "الماركة":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = query.Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                    OrderBy(ww => ww.MasterAsset.Brand.NameAr);


                            }
                            else
                            {
                                query = query.Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                   OrderByDescending(ww => ww.MasterAsset.Brand.NameAr);
                            }
                            break;
                        case "Manufacture":
                            if (data.SortObject.SortStatus == "ascending")

                            {


                                query = query.Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                  OrderBy(ww => ww.MasterAsset.Brand.Name);


                            }
                            else
                            {
                                query = query.Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                            OrderByDescending(ww => ww.MasterAsset.Brand.Name);
                            }
                            break;
                        case "المورد":
                            if (data.SortObject.SortStatus == "ascending")
                            {

                                query = query.Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                  OrderBy(ww => ww.SupplierId);
                            }
                            else
                            {
                                query = query.Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                OrderByDescending(ww => ww.SupplierId);
                            }
                            break;
                        case "Supplier":
                            if (data.SortObject.SortStatus == "ascending")
                            {

                                query = query.Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                  OrderBy(ww => ww.SupplierId);
                            }
                            else
                            {
                                query = query.Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                          OrderByDescending(ww => ww.SupplierId);

                            }
                            break;
                        case "تاريخ الشراء":
                        case "Purchased Date":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = query.Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                               OrderBy(ww => ww.PurchaseDate);
                            }
                            else
                            {
                                query = query.Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                               OrderByDescending(ww => ww.PurchaseDate);
                            }
                            break;



                    }
                }
                else
                {
                    switch (data.SortObject.SortBy)
                    {
                        case "الاسم":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                // filteration and sort then pagination 
                                // Where() , OrderBy() then Skip() Take()
                                query = GetLstAssetDetails().Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                  OrderBy(ww => ww.MasterAsset.NameAr);
                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                              OrderByDescending(ww => ww.MasterAsset.NameAr);
                            }
                            break;

                        case "Name":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                OrderBy(ww => ww.MasterAsset.Name);

                            }
                            else
                            {

                                query = GetLstAssetDetails().Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                    OrderByDescending(ww => ww.MasterAsset.Name);

                            }

                            break;

                        case "الباركود":
                        case "Barcode":

                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                OrderBy(ww => ww.Barcode);


                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                  OrderByDescending(ww => ww.Barcode);

                            }

                            break;

                        case "السيريال":
                        case "Serial":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                OrderBy(ww => ww.SerialNumber);

                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                               OrderByDescending(ww => ww.SerialNumber);
                            }

                            break;

                        case "رقم الموديل":
                        case "Model Number":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                     OrderBy(ww => ww.MasterAsset.ModelNumber);
                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                     OrderByDescending(ww => ww.MasterAsset.ModelNumber);
                            }
                            break;
                        case "القسم":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                  OrderBy(ww => ww.DepartmentId);
                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                                OrderByDescending(ww => ww.DepartmentId);

                            }
                            break;


                        case "Department":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                     OrderBy(ww => ww.DepartmentId);

                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                               OrderByDescending(ww => ww.DepartmentId);
                            }
                            break;

                        case "الماركة":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                    OrderBy(ww => ww.MasterAsset.Brand.NameAr);


                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                   OrderByDescending(ww => ww.MasterAsset.Brand.NameAr);
                            }
                            break;
                        case "Manufacture":
                            if (data.SortObject.SortStatus == "ascending")

                            {


                                query = GetLstAssetDetails().Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                  OrderBy(ww => ww.MasterAsset.Brand.Name);


                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                            OrderByDescending(ww => ww.MasterAsset.Brand.Name);
                            }
                            break;
                        case "المورد":
                            if (data.SortObject.SortStatus == "ascending")
                            {

                                query = GetLstAssetDetails().Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                  OrderBy(ww => ww.SupplierId);
                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                OrderByDescending(ww => ww.SupplierId);
                            }
                            break;
                        case "Supplier":
                            if (data.SortObject.SortStatus == "ascending")
                            {

                                query = GetLstAssetDetails().Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                                  OrderBy(ww => ww.SupplierId);
                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                          OrderByDescending(ww => ww.SupplierId);

                            }
                            break;
                        case "تاريخ الشراء":
                        case "Purchased Date":
                            if (data.SortObject.SortStatus == "ascending")
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                               OrderBy(ww => ww.PurchaseDate);
                            }
                            else
                            {
                                query = GetLstAssetDetails().Where(e => e.MasterAssetId == data.FilteredObj.MasterAssetId).
                               OrderByDescending(ww => ww.PurchaseDate);
                            }
                            break;



                    }
                }
                mainClass.Count = query.Count();

                searchResult = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            }




            else
            {
                searchResult = searchResult.ToList();
            }
            string setstartday, setstartmonth, setendday, setendmonth = "";
            DateTime? startingFrom = new DateTime();
            DateTime? endingTo = new DateTime();
            if (data.FilteredObj.Start == "")
            {
                data.FilteredObj.purchaseDateFrom = DateTime.Parse("01/01/1900");
                startingFrom = DateTime.Parse("01/01/1900");
            }
            else
            {
                data.FilteredObj.purchaseDateFrom = DateTime.Parse(data.FilteredObj.Start.ToString());
                var startyear = data.FilteredObj.purchaseDateFrom.Value.Year;
                var startmonth = data.FilteredObj.purchaseDateFrom.Value.Month;
                var startday = data.FilteredObj.purchaseDateFrom.Value.Day;
                if (startday < 10)
                    setstartday = data.FilteredObj.purchaseDateFrom.Value.Day.ToString().PadLeft(2, '0');
                else
                    setstartday = data.FilteredObj.purchaseDateFrom.Value.Day.ToString();

                if (startmonth < 10)
                    setstartmonth = data.FilteredObj.purchaseDateFrom.Value.Month.ToString().PadLeft(2, '0');
                else
                    setstartmonth = data.FilteredObj.purchaseDateFrom.Value.Month.ToString();

                var sDate = startyear + "-" + setstartmonth + "-" + setstartday;
                startingFrom = DateTime.Parse(sDate);//.AddDays(1);
            }

            if (data.FilteredObj.End == "")
            {
                data.FilteredObj.purchaseDateTo = DateTime.Today.Date;
                endingTo = DateTime.Today.Date;
            }
            else
            {
                data.FilteredObj.purchaseDateTo = DateTime.Parse(data.FilteredObj.End.ToString());
                var endyear = data.FilteredObj.purchaseDateTo.Value.Year;
                var endmonth = data.FilteredObj.purchaseDateTo.Value.Month;
                var endday = data.FilteredObj.purchaseDateTo.Value.Day;
                if (endday < 10)
                    setendday = data.FilteredObj.purchaseDateTo.Value.Day.ToString().PadLeft(2, '0');
                else
                    setendday = data.FilteredObj.purchaseDateTo.Value.Day.ToString();
                if (endmonth < 10)
                    setendmonth = data.FilteredObj.purchaseDateTo.Value.Month.ToString().PadLeft(2, '0');
                else
                    setendmonth = data.FilteredObj.purchaseDateTo.Value.Month.ToString();
                var eDate = endyear + "-" + setendmonth + "-" + setendday;
                endingTo = DateTime.Parse(eDate);
            }
            if (startingFrom != null || endingTo != null)
            {
                foreach (var item in list)
                {
                    if (item.PurchaseDate != null)
                    {
                        list = list.Where(a => a.PurchaseDate.Value.Date >= startingFrom.Value.Date && a.PurchaseDate.Value.Date <= endingTo.Value.Date).ToList();

                    }

                }

            }
            else
            {
                list = list.ToList();
            }




            if (list.Count == 0 || list is null)
            {
                if (searchResult.Count > 0)
                {
                    foreach (var item in searchResult)
                    {
                        IndexAssetDetailVM.GetData getDataobj = new IndexAssetDetailVM.GetData();
                        getDataobj.Id = item.Id;
                        getDataobj.Code = item.Code;
                        getDataobj.Price = item.Price;
                        getDataobj.BarCode = item.Barcode;
                        getDataobj.Serial = item.SerialNumber;
                        getDataobj.SerialNumber = item.SerialNumber;
                        getDataobj.BarCode = item.Barcode;
                        getDataobj.Model = item.MasterAsset.ModelNumber;
                        getDataobj.MasterAssetId = item.MasterAssetId;
                        getDataobj.PurchaseDate = item.PurchaseDate;
                        getDataobj.HospitalId = item.HospitalId;
                        getDataobj.DepartmentId = item.DepartmentId;
                        getDataobj.HospitalName = item.Hospital.Name;
                        getDataobj.HospitalNameAr = item.Hospital.NameAr;
                        getDataobj.AssetName = item.MasterAsset.Name;
                        getDataobj.AssetNameAr = item.MasterAsset.NameAr;
                        getDataobj.GovernorateId = item.Hospital.Governorate.Id;
                        getDataobj.GovernorateName = item.Hospital.Governorate.Name;
                        getDataobj.GovernorateNameAr = item.Hospital.Governorate.NameAr;
                        getDataobj.CityId = item.Hospital.City.Id;
                        getDataobj.CityName = item.Hospital.City.Name;
                        getDataobj.CityNameAr = item.Hospital.City.NameAr;
                        getDataobj.OrganizationId = item.Hospital.Organization.Id;
                        getDataobj.OrgName = item.Hospital.Organization.Name;
                        getDataobj.OrgNameAr = item.Hospital.Organization.NameAr;
                        getDataobj.SubOrgName = item.Hospital.SubOrganization.Name;
                        getDataobj.SubOrgNameAr = item.Hospital.SubOrganization.NameAr;
                        getDataobj.SubOrganizationId = item.Hospital.SubOrganization.Id;
                        getDataobj.BrandId = item.MasterAsset.Brand != null ? item.MasterAsset.Brand.Id : 0;
                        getDataobj.BrandName = item.MasterAsset.Brand != null ? item.MasterAsset.Brand.Name : "";
                        getDataobj.BrandNameAr = item.MasterAsset.Brand != null ? item.MasterAsset.Brand.NameAr : "";
                        getDataobj.SupplierId = item.Supplier != null ? item.Supplier.Id : 0;
                        getDataobj.SupplierName = item.Supplier != null ? item.Supplier.Name : "";
                        getDataobj.SupplierNameAr = item.Supplier != null ? item.Supplier.NameAr : "";
                    

                        list.Add(getDataobj);
                    }

                }
            }


            mainClass.Results = list;

            return mainClass;




      

        }
        public IndexAssetDetailVM FilterDataByDepartmentBrandSupplierIdAndPaging(FilterHospitalAsset data,  int pageNumber, int pageSize)


        { 
            IQueryable<AssetDetail> query = null;
            IndexAssetDetailVM mainClass = new IndexAssetDetailVM();
            List<IndexAssetDetailVM.GetData> list = new List<IndexAssetDetailVM.GetData>();
            List<AssetDetail> searchResult = new List<AssetDetail>();

            query = GetLstAssetDetails();
            if (data.AssetNameAr != "")
            {


                query = query.Where(e => e.MasterAsset.NameAr == data.AssetNameAr);
                mainClass.Count = query.Count();
                if (query != null)
                    searchResult = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


            }

            if (data.AssetName != "")
            {
                if (query is not null)
                {
                    query = query.Where(e => e.MasterAsset.Name == data.AssetName);
                }
                else if (query is null)
                {
                    query = GetLstAssetDetails().Where(e => e.MasterAsset.Name == data.AssetName);
                }

                mainClass.Count = query.Count();
                searchResult = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            }



            //if (data.DepartmentId != 0)
            //{
            //    if (query is not null)
            //    {
            //        query = query.Where(e => e.DepartmentId == data.DepartmentId);

            //    }
            //    else if (query is null)

            //    {
            //        query = GetLstAssetDetails().Where(e => e.DepartmentId == data.DepartmentId);
            //    }

            //    mainClass.Count = query.Count();
            //    searchResult = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


            //}


            if (data.BrandId != 0)
            {
              
                    query = query.Where(e => e.MasterAsset.BrandId == data.BrandId);

                
                
              
                

                mainClass.Count = query.Count();
                searchResult = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


            }



            if (data.GovernorateId != 0)
            {

                query = query.Where(e => e.Hospital.GovernorateId == data.GovernorateId);






                mainClass.Count = query.Count();
                searchResult = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


            }
            if (data.hospitalId != 0)
            {

                query = query.Where(e => e.HospitalId == data.hospitalId);






                mainClass.Count = query.Count();
                searchResult = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


            }
            if (data.OrganizationId != 0)
            {

                query = query.Where(e => e.Hospital.OrganizationId == data.OrganizationId);






                mainClass.Count = query.Count();
                searchResult = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


            }
            if (data.SubOrganizationId != 0)
            {

                query = query.Where(e => e.Hospital.SubOrganizationId == data.SubOrganizationId);






                mainClass.Count = query.Count();
                searchResult = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


            }

            //if (data.SupplierId != 0)
            //{
            //    if (query is not null)
            //    {
            //        query = query.Where(ww => ww.SupplierId == data.SupplierId);
            //    }
            //    else if (query is null)
            //    {
            //        query = GetLstAssetDetails().Where(ww => ww.SupplierId == data.SupplierId);
            //    }
            //    mainClass.Count = query.Count();
            //    searchResult = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            //}

            if (data.MasterAssetId != 0)
            {
                if (query is not null)
                {
                    query = query.Where(ww => ww.MasterAssetId == data.MasterAssetId);
                }
                else if (query is null)
                {
                    query = GetLstAssetDetails().Where(ww => ww.MasterAssetId == data.MasterAssetId);
                }
                mainClass.Count = query.Count();

                searchResult = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            }


       

          
            string setstartday, setstartmonth, setendday, setendmonth = "";
            DateTime? startingFrom = new DateTime();
            DateTime? endingTo = new DateTime();
            if (data.Start == "" && data.End != "")
            {
                data.purchaseDateFrom = DateTime.Parse("01/01/1900");
                startingFrom = DateTime.Parse("01/01/1900");
            }
             if (    data.Start != "" )
            {
                data.purchaseDateFrom = DateTime.Parse(data.Start.ToString());
                var startyear = data.purchaseDateFrom.Value.Year;
                var startmonth = data.purchaseDateFrom.Value.Month;
                var startday = data.purchaseDateFrom.Value.Day;
                if (startday < 10)
                    setstartday = data.purchaseDateFrom.Value.Day.ToString().PadLeft(2, '0');
                else
                    setstartday = data.purchaseDateFrom.Value.Day.ToString();

                if (startmonth < 10)
                    setstartmonth = data.purchaseDateFrom.Value.Month.ToString().PadLeft(2, '0');
                else
                    setstartmonth = data.purchaseDateFrom.Value.Month.ToString();

                var sDate = startyear + "-" + setstartmonth + "-" + setstartday;
                startingFrom = DateTime.Parse(sDate);//.AddDays(1);
            }

            if (data.End == "" && data.Start != "")
            {
                data.purchaseDateTo = DateTime.Today.Date;
                endingTo = DateTime.Today.Date;
            }
             if (data.End != "" )
            {
                data.purchaseDateTo = DateTime.Parse(data.End.ToString());
                var endyear = data.purchaseDateTo.Value.Year;
                var endmonth = data.purchaseDateTo.Value.Month;
                var endday = data.purchaseDateTo.Value.Day;
                if (endday < 10)
                    setendday = data.purchaseDateTo.Value.Day.ToString().PadLeft(2, '0');
                else
                    setendday = data.purchaseDateTo.Value.Day.ToString();
                if (endmonth < 10)
                    setendmonth = data.purchaseDateTo.Value.Month.ToString().PadLeft(2, '0');
                else
                    setendmonth = data.purchaseDateTo.Value.Month.ToString();
                var eDate = endyear + "-" + setendmonth + "-" + setendday;
                endingTo = DateTime.Parse(eDate);
            }
            if (data.Start != ""|| data.End !="")
            {

                query = query.Where(a => a.PurchaseDate.Value.Date >= startingFrom.Value.Date && a.PurchaseDate.Value.Date <= endingTo.Value.Date);
                mainClass.Count = query.Count();
                if (query != null)
                    searchResult = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

       

                    


            }
            
            
            
            
            
            
          if(data.BrandId==0 && data.MasterAssetId==0&& data.Start=="" &&data.End==""&&data.AssetName=="" &&data.AssetNameAr=="" && data.GovernorateId == 0 && data.OrganizationId == 0 && data.hospitalId == 0 && data.SubOrganizationId == 0)
            {
           
                if (query != null)
                    searchResult = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();



            }



            if (list.Count == 0 || list is null)
            {
                if (searchResult.Count > 0)
                {
                    foreach (var item in searchResult)
                    {
                        IndexAssetDetailVM.GetData getDataobj = new IndexAssetDetailVM.GetData();
                        getDataobj.Id = item.Id;
                        getDataobj.Code = item.Code;
                        getDataobj.Price = item.Price;
                        getDataobj.BarCode = item.Barcode;
                        getDataobj.Serial = item.SerialNumber;
                        getDataobj.SerialNumber = item.SerialNumber;
                        getDataobj.BarCode = item.Barcode;
                        getDataobj.Model = item.MasterAsset.ModelNumber;
                        getDataobj.MasterAssetId = item.MasterAssetId;
                        getDataobj.PurchaseDate = item.PurchaseDate;
                        getDataobj.HospitalId = item.HospitalId;
                        getDataobj.DepartmentId = item.DepartmentId;
                        getDataobj.HospitalName = item.Hospital.Name;
                        getDataobj.HospitalNameAr = item.Hospital.NameAr;
                        getDataobj.AssetName = item.MasterAsset.Name;
                        getDataobj.AssetNameAr = item.MasterAsset.NameAr;
                        getDataobj.GovernorateId = item.Hospital.Governorate.Id;
                        getDataobj.GovernorateName = item.Hospital.Governorate.Name;
                        getDataobj.GovernorateNameAr = item.Hospital.Governorate.NameAr;
                        getDataobj.CityId = item.Hospital.City != null ? item.Hospital.City.Id :0;
                        getDataobj.CityName = item.Hospital.City!=null?item.Hospital.City.Name:"";
                        getDataobj.CityNameAr = item.Hospital.City != null ? item.Hospital.City.NameAr : "";
                        getDataobj.OrganizationId = item.Hospital.Organization.Id;
                        getDataobj.OrgName = item.Hospital.Organization.Name;
                        getDataobj.OrgNameAr = item.Hospital.Organization.NameAr;
                        getDataobj.SubOrgName = item.Hospital.SubOrganization.Name;
                        getDataobj.SubOrgNameAr = item.Hospital.SubOrganization.NameAr;
                        getDataobj.SubOrganizationId = item.Hospital.SubOrganization.Id;
                        getDataobj.BrandId = item.MasterAsset.Brand != null ? item.MasterAsset.Brand.Id : 0;
                        getDataobj.BrandName = item.MasterAsset.Brand != null ? item.MasterAsset.Brand.Name : "";
                        getDataobj.BrandNameAr = item.MasterAsset.Brand != null ? item.MasterAsset.Brand.NameAr : "";
                        getDataobj.SupplierId = item.Supplier != null ? item.Supplier.Id : 0;
                        getDataobj.SupplierName = item.Supplier != null ? item.Supplier.Name : "";
                        getDataobj.SupplierNameAr = item.Supplier != null ? item.Supplier.NameAr : "";
                       

                      

                        list.Add(getDataobj);
                    }

                }
            }


            mainClass.Results = list;

            return mainClass;


        }
        private IOrderedQueryable<AssetDetail> GetLstForSortAsset()
        {
            return _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Hospital)
                            .Include(a => a.Supplier).Include(a => a.MasterAsset.Brand)
                                    .Include(a => a.Hospital.Governorate).Include(a => a.Hospital.City)
                                    .Include(a => a.Hospital.Organization).Include(a => a.Hospital.SubOrganization)
                                    .OrderBy(a => a.Barcode);

        }


        public IndexAssetDetailVM SortAssetDetail(SortAssetDetail sortObject, int pageNumber, int pageSize)
        {

            IndexAssetDetailVM mainClass = new IndexAssetDetailVM();
            mainClass.Count = _context.AssetDetails.Count();
            var result = new List<IndexAssetDetailVM.GetData>();
            // get all Data as IEnmerable <>
            var assets = new List<AssetDetail>();

            if (sortObject != null)
            {
                if (sortObject.SortBy != "")
                {
                    switch (sortObject.SortBy)
                    {
                        case "الاسم":
                            if (sortObject.SortStatus == "ascending")
                            {
                                assets = GetLstForSortAsset().OrderBy(ww => ww.MasterAsset.NameAr).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                            }
                            else
                            {
                                assets = GetLstForSortAsset().OrderByDescending(ww => ww.MasterAsset.NameAr).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                            }
                            break;

                        case "Name":
                            if (sortObject.SortStatus == "ascending")
                            {
                                assets = GetLstForSortAsset().OrderBy(ww => ww.MasterAsset.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                            }
                            else
                            {
                                assets = GetLstForSortAsset().OrderByDescending(ww => ww.MasterAsset.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                            }

                            break;

                        case "الباركود":
                        case "Barcode":

                            if (sortObject.SortStatus == "ascending")
                            {
                                assets = GetLstForSortAsset().OrderBy(ww => ww.Barcode).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                            }
                            else
                            {
                                assets = GetLstForSortAsset().OrderByDescending(ww => ww.Barcode).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                            }

                            break;

                        case "السيريال":
                        case "Serial":
                            if (sortObject.SortStatus == "ascending")
                            {
                                assets = GetLstForSortAsset().OrderBy(ww => ww.SerialNumber).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                            }
                            else
                            {
                                assets = GetLstForSortAsset().OrderByDescending(ww => ww.SerialNumber).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                            }

                            break;

                        case "رقم الموديل":
                        case "Model Number":
                            if (sortObject.SortStatus == "ascending")
                            {
                                assets = GetLstForSortAsset().OrderBy(ww => ww.MasterAsset.ModelNumber).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                            }
                            else
                            {
                                assets = GetLstForSortAsset().OrderByDescending(ww => ww.MasterAsset.ModelNumber).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                            }
                            break;
                        case "القسم":
                            if (sortObject.SortStatus == "ascending")
                            {
                                assets = GetLstForSortAsset().OrderBy(ww => ww.DepartmentId).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                            }
                            else
                            {
                                assets = GetLstForSortAsset().OrderByDescending(ww => ww.DepartmentId).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                            }
                            break;


                        case "Department":
                            if (sortObject.SortStatus == "ascending")
                            {
                                assets = GetLstForSortAsset().OrderBy(ww => ww.DepartmentId).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                            }
                            else
                            {
                                assets = GetLstForSortAsset().OrderByDescending(ww => ww.DepartmentId).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                            }
                            break;

                        case "الماركة":
                            if (sortObject.SortStatus == "ascending")
                            {
                                assets = GetLstForSortAsset().OrderBy(ww => ww.MasterAsset.Brand.NameAr).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                            }
                            else
                            {
                                assets = GetLstForSortAsset().OrderByDescending(ww => ww.MasterAsset.Brand.NameAr).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                            }
                            break;
                        case "Manufacture":
                            if (sortObject.SortStatus == "ascending")
                            {
                                assets = GetLstForSortAsset().OrderBy(ww => ww.MasterAsset.Brand.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                            }
                            else
                            {
                                assets = GetLstForSortAsset().OrderByDescending(ww => ww.MasterAsset.Brand.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                            }
                            break;
                        case "المورد":
                            if (sortObject.SortStatus == "ascending")
                            {
                                assets = GetLstForSortAsset().OrderBy(ww => ww.SupplierId).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                            }
                            else
                            {
                                assets = GetLstForSortAsset().OrderByDescending(ww => ww.SupplierId).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                            }
                            break;
                        case "Supplier":
                            if (sortObject.SortStatus == "ascending")
                            {
                                assets = GetLstForSortAsset().OrderBy(ww => ww.SupplierId).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                            }
                            else
                            {
                                assets = GetLstForSortAsset().OrderByDescending(ww => ww.SupplierId).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                            }
                            break;
                        case "تاريخ الشراء":
                        case "Purchased Date":
                            if (sortObject.SortStatus == "ascending")
                            {
                                assets = GetLstForSortAsset().OrderBy(ww => ww.PurchaseDate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                            }
                            else
                            {
                                assets = GetLstForSortAsset().OrderByDescending(ww => ww.PurchaseDate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                            }
                            break;



                    }
                }


            }



            foreach (var item in assets)
            {
                IndexAssetDetailVM.GetData assetObject = new IndexAssetDetailVM.GetData();
                assetObject.Id = item.Id;
                assetObject.Code = item.Code;
                assetObject.Price = item.Price;
                assetObject.BarCode = item.Barcode;
                assetObject.Serial = item.SerialNumber;
                assetObject.SerialNumber = item.SerialNumber;
                assetObject.BarCode = item.Barcode;
                assetObject.Model = item.MasterAsset.ModelNumber;
                assetObject.MasterAssetId = item.MasterAssetId;
                assetObject.PurchaseDate = item.PurchaseDate;
                assetObject.HospitalId = item.HospitalId;
                assetObject.DepartmentId = item.DepartmentId;
                assetObject.HospitalName = item.Hospital.Name;
                assetObject.HospitalNameAr = item.Hospital.NameAr;
                assetObject.AssetName = item.MasterAsset.Name;
                assetObject.AssetNameAr = item.MasterAsset.NameAr;
                assetObject.GovernorateId = item.Hospital.Governorate.Id;
                assetObject.GovernorateName = item.Hospital.Governorate.Name;
                assetObject.GovernorateNameAr = item.Hospital.Governorate.NameAr;
                assetObject.CityId = item.Hospital.City!=null? item.Hospital.City.Id:0;
                assetObject.CityName = item.Hospital.City != null ? item.Hospital.City.Name:"";
                assetObject.CityNameAr = item.Hospital.City != null ? item.Hospital.City.NameAr:"";
                assetObject.OrganizationId = item.Hospital.Organization.Id;
                assetObject.OrgName = item.Hospital.Organization.Name;
                assetObject.OrgNameAr = item.Hospital.Organization.NameAr;
                assetObject.SubOrgName = item.Hospital.SubOrganization.Name;
                assetObject.SubOrgNameAr = item.Hospital.SubOrganization.NameAr;
                assetObject.SubOrganizationId = item.Hospital.SubOrganization.Id;
                assetObject.BrandId = item.MasterAsset.Brand != null ? item.MasterAsset.Brand.Id : 0;
                assetObject.BrandName = item.MasterAsset.Brand != null ? item.MasterAsset.Brand.Name : "";
                assetObject.BrandNameAr = item.MasterAsset.Brand != null ? item.MasterAsset.Brand.NameAr : "";
                assetObject.SupplierId = item.Supplier != null ? item.Supplier.Id : 0;
                assetObject.SupplierName = item.Supplier != null ? item.Supplier.Name : "";
                assetObject.SupplierNameAr = item.Supplier != null ? item.Supplier.NameAr : "";
             

               

                result.Add(assetObject);

            }

            mainClass.Results = result;

            return mainClass;
        }
        public IndexAssetDetailVM SortAssetsWithoutSearch(Sort sortObj, int pageNumber, int pageSize)
        {




            IndexAssetDetailVM mainClass = new IndexAssetDetailVM();
            List<IndexAssetDetailVM.GetData> list = new List<IndexAssetDetailVM.GetData>();
            IQueryable<AssetDetail> lstAllAssets = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Hospital)
                                 
                                   .Include(a => a.Hospital.Governorate).Include(a => a.Hospital.City);




            if (lstAllAssets.Count() > 0)
            {
                if (sortObj.GovernorateId != 0)
                {

                    lstAllAssets = lstAllAssets.Where(h => h.Hospital.GovernorateId == sortObj.GovernorateId);
                    if (sortObj.HospitalName != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.Hospital.Name);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.Hospital.Name);
                        }
                    }
                    if (sortObj.HospitalNameAr != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.Hospital.NameAr);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.Hospital.NameAr);
                        }
                    }
                    if (sortObj.BarCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.Barcode);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.Barcode);
                        }
                    }

                    if (sortObj.AssetName != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.MasterAsset.Name);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.MasterAsset.Name);
                        }
                    }
                    if (sortObj.AssetNameAr != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.MasterAsset.NameAr);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.MasterAsset.NameAr);
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.SerialNumber);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.SerialNumber);
                        }
                    }


                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.MasterAsset.ModelNumber);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.MasterAsset.ModelNumber);
                        }
                    }
                    if (sortObj.BrandName != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.MasterAsset.Brand.Name);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.MasterAsset.Brand.Name);
                        }
                    }
                    if (sortObj.BrandNameAr != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.MasterAsset.Brand.Name);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.MasterAsset.Brand.Name);
                        }
                    }
                  
                  


                }


                if (sortObj.HospitalId != 0)
                {

                    lstAllAssets = lstAllAssets.Where(h => h.HospitalId == sortObj.HospitalId);
                    if (sortObj.BarCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.Barcode);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.Barcode);
                        }
                    }
                    if (sortObj.SerialValue != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.SerialNumber);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.SerialNumber);
                        }
                    }
                    if (sortObj.HospitalName != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.Hospital.Name);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.Hospital.Name);
                        }
                    }
                    if (sortObj.HospitalNameAr != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.Hospital.NameAr);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.Hospital.NameAr);
                        }
                    }
                    if (sortObj.AssetName != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.MasterAsset.Name);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.MasterAsset.Name);
                        }
                    }
                    if (sortObj.AssetNameAr != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.MasterAsset.NameAr);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.MasterAsset.NameAr);
                        }
                    }
                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.MasterAsset.ModelNumber);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.MasterAsset.ModelNumber);
                        }
                    }
                    if (sortObj.BrandName != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.MasterAsset.Brand.Name);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.MasterAsset.Brand.Name);
                        }
                    }
                    if (sortObj.BrandNameAr != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.MasterAsset.Brand.Name);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.MasterAsset.Brand.Name);
                        }
                    }
                    if (sortObj.SupplierName != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.Supplier.Name);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.Supplier.Name);
                        }
                    }
                    if (sortObj.SupplierNameAr != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.Supplier.Name);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.Supplier.Name);
                        }
                    }
                }


                if (sortObj.DepartmentId != 0)
                {

                    lstAllAssets = lstAllAssets.Where(h => h.DepartmentId == sortObj.DepartmentId && h.HospitalId == sortObj.HospitalId);

                    if (sortObj.HospitalName != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.Hospital.Name);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.Hospital.Name);
                        }
                    }
                    if (sortObj.HospitalNameAr != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.Hospital.NameAr);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.Hospital.NameAr);
                        }
                    }

                    if (sortObj.AssetName != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.MasterAsset.Name);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.MasterAsset.Name);
                        }
                    }
                    if (sortObj.AssetNameAr != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.MasterAsset.NameAr);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.MasterAsset.NameAr);
                        }
                    }
                    if (sortObj.BarCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.Barcode);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.Barcode);
                        }
                    }
                    if (sortObj.Serial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.SerialNumber);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.SerialNumber);
                        }
                    }


                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.MasterAsset.ModelNumber);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.MasterAsset.ModelNumber);
                        }
                    }
                    if (sortObj.BrandName != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.MasterAsset.Brand.Name);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.MasterAsset.Brand.Name);
                        }
                    }
                    if (sortObj.BrandNameAr != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.MasterAsset.Brand.Name);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.MasterAsset.Brand.Name);
                        }
                    }
                    if (sortObj.SupplierName != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.Supplier.Name);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.Supplier.Name);
                        }
                    }
                    if (sortObj.SupplierNameAr != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.Supplier.Name);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.Supplier.Name);
                        }
                    }

                }


                else
                {

                    if (sortObj.HospitalName != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.Hospital.Name);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.Hospital.Name);
                        }
                    }
                    if (sortObj.HospitalNameAr != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.Hospital.NameAr);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.Hospital.NameAr);
                        }
                    }
                    if (sortObj.AssetName != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.MasterAsset.Name);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.MasterAsset.Name);
                        }
                    }
                    if (sortObj.AssetNameAr != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.MasterAsset.NameAr);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.MasterAsset.NameAr);
                        }
                    }
                    if (sortObj.BarCode != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.Barcode);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.Barcode);
                        }
                    }
                    if (sortObj.Serial != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.SerialNumber);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.SerialNumber);
                        }
                    }


                    if (sortObj.Model != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.MasterAsset.ModelNumber);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.MasterAsset.ModelNumber);
                        }
                    }
                    if (sortObj.BrandName != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.MasterAsset.Brand.Name);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.MasterAsset.Brand.Name);
                        }
                    }
                    if (sortObj.BrandNameAr != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.MasterAsset.Brand.Name);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.MasterAsset.Brand.Name);
                        }
                    }
                    if (sortObj.SupplierName != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.Supplier.Name);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.Supplier.Name);
                        }
                    }
                    if (sortObj.SupplierNameAr != "")
                    {
                        if (sortObj.SortStatus == "descending")
                        {
                            lstAllAssets = lstAllAssets.OrderByDescending(d => d.Supplier.Name);
                        }
                        else
                        {
                            lstAllAssets = lstAllAssets.OrderBy(d => d.Supplier.Name);
                        }
                    }
                }

                foreach (var asset in lstAllAssets)
                {
                    IndexAssetDetailVM.GetData detail = new IndexAssetDetailVM.GetData();
                    /////
                    detail.Id = asset.Id;
                    detail.DepartmentId = asset.DepartmentId != null ? asset.DepartmentId : 0;
                    detail.Code = asset.Code;

                    //   detail.EmployeeId = asset.EmployeeId;
                    detail.Price = asset.Price;
                    detail.BarCode = asset.Barcode;
                    detail.MasterImg = asset.MasterAsset.AssetImg;
                    detail.Serial = asset.SerialNumber;
                    detail.BrandName = asset.MasterAsset.Brand != null ? asset.MasterAsset.Brand.Name : "";
                    detail.BrandNameAr = asset.MasterAsset.Brand != null ? asset.MasterAsset.Brand.NameAr : "";
                    detail.Model = asset.MasterAsset.ModelNumber;
                    detail.SerialNumber = asset.SerialNumber;
                    detail.MasterAssetId = asset.MasterAssetId;
                    detail.PurchaseDate = asset.PurchaseDate;
                    detail.HospitalId = asset.Hospital.Id;
                    detail.HospitalName = asset.Hospital.Name;
                    detail.HospitalNameAr = asset.Hospital.NameAr;
                    detail.AssetName = asset.MasterAsset.Name;
                    detail.AssetNameAr = asset.MasterAsset.NameAr;
                
                    detail.SupplierName = asset.Supplier != null ? asset.Supplier.Name : "";
                    detail.SupplierNameAr = asset.Supplier != null ? asset.Supplier.NameAr : "";
                  
                    detail.GovernorateId = asset.Hospital.GovernorateId;
                    detail.GovernorateName = asset.Hospital.Governorate.Name;
                    detail.GovernorateNameAr = asset.Hospital.Governorate.NameAr;
                    detail.CityId = asset.Hospital.CityId;
                    detail.CityName = asset.Hospital.City != null ? asset.Hospital.City.Name : "";
                    detail.CityNameAr = asset.Hospital.City != null ? asset.Hospital.City.NameAr : "";

                    list.Add(detail);
                }
                var requestsPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                mainClass.Results = requestsPerPage;
                mainClass.Count = list.Count();
                return mainClass;
            }
            return mainClass;
        }

        public List<IndexAssetDetailVM.GetData> FindAllFilteredAssetsForGrouping(FilterHospitalAsset data)
        {
            List<IndexAssetDetailVM.GetData> result = new List<IndexAssetDetailVM.GetData>();
            List<IndexAssetDetailVM.GetData> query = GetLstAssetDetails()
                        .Select(Ass => new IndexAssetDetailVM.GetData
                        {
                            #region MyRegion
                            Id = Ass.Id,
                            AssetName = Ass.MasterAsset.Name,
                            BarCode = Ass.Barcode,
                            SerialNumber = Ass.SerialNumber,
                            Model = Ass.MasterAsset.ModelNumber,
                            GovernorateId = Ass.Hospital.GovernorateId,
                            OrganizationId = Ass.Hospital.OrganizationId,
                            SubOrganizationId = Ass.Hospital.SubOrganizationId,
                            AssetNameAr = Ass.MasterAsset.NameAr,
                            BrandName = Ass.MasterAsset.Brand.Name,
                            BrandNameAr = Ass.MasterAsset.Brand.NameAr,
                            GovernorateName = Ass.Hospital.Governorate.Name,
                            GovernorateNameAr = Ass.Hospital.Governorate.NameAr,
                            SubOrgName = Ass.Hospital.SubOrganization.Name,
                            SubOrgNameAr = Ass.Hospital.SubOrganization.NameAr,
                            CityName = Ass.Hospital.City.Name,
                            CityNameAr = Ass.Hospital.City.NameAr,
                            HospitalId = Ass.HospitalId,
                            HospitalName = Ass.Hospital.Name,
                            HospitalNameAr = Ass.Hospital.NameAr,
                            SupplierName = Ass.Supplier.Name,
                            SupplierNameAr = Ass.Supplier.NameAr,
                            OrgName = Ass.Hospital.Organization.Name,
                            OrgNameAr = Ass.Hospital.Organization.NameAr,
                            PurchaseDate = Ass.PurchaseDate,
                            BrandId = Ass.MasterAsset.BrandId

                            #endregion

                        }).ToList();

            if (data.AssetName != "")
            {
                query = GetLstAssetDetails().Where(ww => ww.MasterAsset.Name == data.AssetName)
                    .Select(Ass => new IndexAssetDetailVM.GetData
                    {
                        #region MyRegion
                        Id = Ass.Id,
                        AssetName = Ass.MasterAsset.Name,
                        BarCode = Ass.Barcode,
                        SerialNumber = Ass.SerialNumber,
                        Model = Ass.MasterAsset.ModelNumber,
                        SubOrgName = Ass.Hospital.SubOrganization.Name,
                        SubOrgNameAr = Ass.Hospital.SubOrganization.NameAr,
                        AssetNameAr = Ass.MasterAsset.NameAr,
                        BrandName = Ass.MasterAsset.Brand.Name,
                        BrandNameAr = Ass.MasterAsset.Brand.NameAr,
                        GovernorateName = Ass.Hospital.Governorate.Name,
                        GovernorateNameAr = Ass.Hospital.Governorate.NameAr,
                        CityName = Ass.Hospital.City.Name,
                        CityNameAr = Ass.Hospital.City.NameAr,
                        HospitalId = Ass.HospitalId,
                        HospitalName = Ass.Hospital.Name,
                        HospitalNameAr = Ass.Hospital.NameAr,
                        SupplierName = Ass.Supplier.Name,
                        SupplierNameAr = Ass.Supplier.NameAr,
                        OrgName = Ass.Hospital.Organization.Name,
                        OrgNameAr = Ass.Hospital.Organization.NameAr,
                        PurchaseDate = Ass.PurchaseDate,
                        BrandId = Ass.MasterAsset.BrandId

                        #endregion

                    }).ToList();

            }
            if (data.AssetNameAr != "")
            {


                query = GetLstAssetDetails().Where(ww => ww.MasterAsset.NameAr == data.AssetNameAr)
                        .Select(Ass => new IndexAssetDetailVM.GetData
                        {
                            #region MyRegion
                            Id = Ass.Id,
                            AssetName = Ass.MasterAsset.Name,
                            BarCode = Ass.Barcode,
                            SerialNumber = Ass.SerialNumber,
                            Model = Ass.MasterAsset.ModelNumber,
                          
                            AssetNameAr = Ass.MasterAsset.NameAr,
                            BrandName = Ass.MasterAsset.Brand.Name,
                            BrandNameAr = Ass.MasterAsset.Brand.NameAr,
                            GovernorateName = Ass.Hospital.Governorate.Name,
                            GovernorateNameAr = Ass.Hospital.Governorate.NameAr,
                            CityName = Ass.Hospital.City.Name,
                            CityNameAr = Ass.Hospital.City.NameAr,
                            HospitalId = Ass.HospitalId,
                            HospitalName = Ass.Hospital.Name,
                            HospitalNameAr = Ass.Hospital.NameAr,
                            SubOrgName = Ass.Hospital.SubOrganization.Name,
                            SubOrgNameAr = Ass.Hospital.SubOrganization.NameAr,
                            SupplierName = Ass.Supplier.Name,
                            SupplierNameAr = Ass.Supplier.NameAr,
                            OrgName = Ass.Hospital.Organization.Name,
                            OrgNameAr = Ass.Hospital.Organization.NameAr,
                            PurchaseDate = Ass.PurchaseDate,
                            BrandId = Ass.MasterAsset.BrandId

                            #endregion

                        }).ToList();




            }

            
            if (data.BrandId != 0)

            {
                if (query is not null)
                {
                    query = query.Where(a => a.BrandId == data.BrandId).ToList();
                }
                else
                {
                    query = GetLstAssetDetails().Where(ww => ww.MasterAsset.BrandId == data.BrandId)
                                 .Select(Ass => new IndexAssetDetailVM.GetData
                                 {
                                     #region MyRegion
                                     Id = Ass.Id,
                                     AssetName = Ass.MasterAsset.Name,
                                     BarCode = Ass.Barcode,
                                     SerialNumber = Ass.SerialNumber,
                                     Model = Ass.MasterAsset.ModelNumber,
                                  
                                     AssetNameAr = Ass.MasterAsset.NameAr,
                                     BrandName = Ass.MasterAsset.Brand.Name,
                                     BrandNameAr = Ass.MasterAsset.Brand.NameAr,
                                     GovernorateName = Ass.Hospital.Governorate.Name,
                                     GovernorateNameAr = Ass.Hospital.Governorate.NameAr,
                                     SubOrgName = Ass.Hospital.SubOrganization.Name,
                                     SubOrgNameAr = Ass.Hospital.SubOrganization.NameAr,
                                     CityName = Ass.Hospital.City.Name,
                                     CityNameAr = Ass.Hospital.City.NameAr,
                                     HospitalId = Ass.HospitalId,
                                     HospitalName = Ass.Hospital.Name,
                                     HospitalNameAr = Ass.Hospital.NameAr,
                                     SupplierName = Ass.Supplier.Name,
                                     SupplierNameAr = Ass.Supplier.NameAr,
                                     OrgName = Ass.Hospital.Organization.Name,
                                     OrgNameAr = Ass.Hospital.Organization.NameAr,
                                     PurchaseDate = Ass.PurchaseDate,
                                     BrandId = Ass.MasterAsset.BrandId

                                     #endregion

                                 }).ToList();

                }

            }


            if (data.MasterAssetId != 0)
            {

                if (query is not null)
                {
                    query = query.Where(a => a.MasterAssetId == data.MasterAssetId).ToList();
                }
                else
                {
                    query = GetLstAssetDetails().Where(ww => ww.MasterAssetId == data.MasterAssetId)
                                   .Select(Ass => new IndexAssetDetailVM.GetData
                                   {
                                       #region MyRegion
                                       Id = Ass.Id,
                                       AssetName = Ass.MasterAsset.Name,
                                       BarCode = Ass.Barcode,
                                       SerialNumber = Ass.SerialNumber,
                                       Model = Ass.MasterAsset.ModelNumber,
                                       SubOrgName = Ass.Hospital.SubOrganization.Name,
                                       SubOrgNameAr = Ass.Hospital.SubOrganization.NameAr,
                                       AssetNameAr = Ass.MasterAsset.NameAr,
                                       BrandName = Ass.MasterAsset.Brand.Name,
                                       BrandNameAr = Ass.MasterAsset.Brand.NameAr,
                                       GovernorateName = Ass.Hospital.Governorate.Name,
                                       GovernorateNameAr = Ass.Hospital.Governorate.NameAr,
                                       CityName = Ass.Hospital.City.Name,
                                       CityNameAr = Ass.Hospital.City.NameAr,
                                       HospitalId = Ass.HospitalId,
                                       HospitalName = Ass.Hospital.Name,
                                       HospitalNameAr = Ass.Hospital.NameAr,
                                       SupplierName = Ass.Supplier.Name,
                                       SupplierNameAr = Ass.Supplier.NameAr,
                                       OrgName = Ass.Hospital.Organization.Name,
                                       OrgNameAr = Ass.Hospital.Organization.NameAr,
                                       PurchaseDate = Ass.PurchaseDate,
                                       BrandId = Ass.MasterAsset.BrandId

                                       #endregion

                                   }).ToList();
                }
            }
            if (data.GovernorateId != 0)
            {

                if (query is not null)
                {
                    query = query.Where(a => a.GovernorateId == data.GovernorateId).ToList();
                }
                else
                {
                    query = GetLstAssetDetails().Where(ww => ww.Hospital.GovernorateId == data.GovernorateId)
                                   .Select(Ass => new IndexAssetDetailVM.GetData
                                   {
                                       #region MyRegion
                                       Id = Ass.Id,
                                       AssetName = Ass.MasterAsset.Name,
                                       BarCode = Ass.Barcode,
                                       SerialNumber = Ass.SerialNumber,
                                       Model = Ass.MasterAsset.ModelNumber,
                                       SubOrgName = Ass.Hospital.SubOrganization.Name,
                                       SubOrgNameAr = Ass.Hospital.SubOrganization.NameAr,
                                       AssetNameAr = Ass.MasterAsset.NameAr,
                                       BrandName = Ass.MasterAsset.Brand.Name,
                                       BrandNameAr = Ass.MasterAsset.Brand.NameAr,
                                       GovernorateName = Ass.Hospital.Governorate.Name,
                                       GovernorateNameAr = Ass.Hospital.Governorate.NameAr,
                                       CityName = Ass.Hospital.City.Name,
                                       CityNameAr = Ass.Hospital.City.NameAr,
                                       HospitalId = Ass.HospitalId,
                                       HospitalName = Ass.Hospital.Name,
                                       HospitalNameAr = Ass.Hospital.NameAr,
                                       SupplierName = Ass.Supplier.Name,
                                       SupplierNameAr = Ass.Supplier.NameAr,
                                       OrgName = Ass.Hospital.Organization.Name,
                                       OrgNameAr = Ass.Hospital.Organization.NameAr,
                                       PurchaseDate = Ass.PurchaseDate,
                                       BrandId = Ass.MasterAsset.BrandId

                                       #endregion

                                   }).ToList();
                }
            }
            if (data.hospitalId != 0)
            {

                if (query is not null)
                {
                    query = query.Where(a => a.HospitalId == data.hospitalId).ToList();
                }
                else
                {
                    query = GetLstAssetDetails().Where(ww => ww.HospitalId == data.hospitalId)
                                   .Select(Ass => new IndexAssetDetailVM.GetData
                                   {
                                       #region MyRegion
                                       Id = Ass.Id,
                                       AssetName = Ass.MasterAsset.Name,
                                       BarCode = Ass.Barcode,
                                       SerialNumber = Ass.SerialNumber,
                                       Model = Ass.MasterAsset.ModelNumber,
                                       SubOrgName = Ass.Hospital.SubOrganization.Name,
                                       SubOrgNameAr = Ass.Hospital.SubOrganization.NameAr,
                                       AssetNameAr = Ass.MasterAsset.NameAr,
                                       BrandName = Ass.MasterAsset.Brand.Name,
                                       BrandNameAr = Ass.MasterAsset.Brand.NameAr,
                                       GovernorateName = Ass.Hospital.Governorate.Name,
                                       GovernorateNameAr = Ass.Hospital.Governorate.NameAr,
                                       CityName = Ass.Hospital.City.Name,
                                       CityNameAr = Ass.Hospital.City.NameAr,
                                       HospitalId = Ass.HospitalId,
                                       HospitalName = Ass.Hospital.Name,
                                       HospitalNameAr = Ass.Hospital.NameAr,
                                       SupplierName = Ass.Supplier.Name,
                                       SupplierNameAr = Ass.Supplier.NameAr,
                                       OrgName = Ass.Hospital.Organization.Name,
                                       OrgNameAr = Ass.Hospital.Organization.NameAr,
                                       PurchaseDate = Ass.PurchaseDate,
                                       BrandId = Ass.MasterAsset.BrandId

                                       #endregion

                                   }).ToList();
                }
            }
            if (data.OrganizationId != 0)
            {

                if (query is not null)
                {
                    query = query.Where(a => a.OrganizationId == data.OrganizationId).ToList();
                }
                else
                {
                    query = GetLstAssetDetails().Where(ww => ww.Hospital.OrganizationId == data.OrganizationId)
                                   .Select(Ass => new IndexAssetDetailVM.GetData
                                   {
                                       #region MyRegion
                                       Id = Ass.Id,
                                       AssetName = Ass.MasterAsset.Name,
                                       BarCode = Ass.Barcode,
                                       SerialNumber = Ass.SerialNumber,
                                       Model = Ass.MasterAsset.ModelNumber,
                                       SubOrgName = Ass.Hospital.SubOrganization.Name,
                                       SubOrgNameAr = Ass.Hospital.SubOrganization.NameAr,
                                       AssetNameAr = Ass.MasterAsset.NameAr,
                                       BrandName = Ass.MasterAsset.Brand.Name,
                                       BrandNameAr = Ass.MasterAsset.Brand.NameAr,
                                       GovernorateName = Ass.Hospital.Governorate.Name,
                                       GovernorateNameAr = Ass.Hospital.Governorate.NameAr,
                                       CityName = Ass.Hospital.City.Name,
                                       CityNameAr = Ass.Hospital.City.NameAr,
                                       HospitalId = Ass.HospitalId,
                                       HospitalName = Ass.Hospital.Name,
                                       HospitalNameAr = Ass.Hospital.NameAr,
                                       SupplierName = Ass.Supplier.Name,
                                       SupplierNameAr = Ass.Supplier.NameAr,
                                       OrgName = Ass.Hospital.Organization.Name,
                                       OrgNameAr = Ass.Hospital.Organization.NameAr,
                                       PurchaseDate = Ass.PurchaseDate,
                                       BrandId = Ass.MasterAsset.BrandId

                                       #endregion

                                   }).ToList();
                }
            }
            if (data.SubOrganizationId != 0)
            {

                if (query is not null)
                {
                    query = query.Where(a => a.SubOrganizationId == data.SubOrganizationId).ToList();
                }
                else
                {
                    query = GetLstAssetDetails().Where(ww => ww.Hospital.SubOrganizationId == data.SubOrganizationId)
                                   .Select(Ass => new IndexAssetDetailVM.GetData
                                   {
                                       #region MyRegion
                                       Id = Ass.Id,
                                       AssetName = Ass.MasterAsset.Name,
                                       BarCode = Ass.Barcode,
                                       SerialNumber = Ass.SerialNumber,
                                       Model = Ass.MasterAsset.ModelNumber,
                                       SubOrgName = Ass.Hospital.SubOrganization.Name,
                                       SubOrgNameAr = Ass.Hospital.SubOrganization.NameAr,
                                       AssetNameAr = Ass.MasterAsset.NameAr,
                                       BrandName = Ass.MasterAsset.Brand.Name,
                                       BrandNameAr = Ass.MasterAsset.Brand.NameAr,
                                       GovernorateName = Ass.Hospital.Governorate.Name,
                                       GovernorateNameAr = Ass.Hospital.Governorate.NameAr,
                                       CityName = Ass.Hospital.City.Name,
                                       CityNameAr = Ass.Hospital.City.NameAr,
                                       HospitalId = Ass.HospitalId,
                                       HospitalName = Ass.Hospital.Name,
                                       HospitalNameAr = Ass.Hospital.NameAr,
                                       SupplierName = Ass.Supplier.Name,
                                       SupplierNameAr = Ass.Supplier.NameAr,
                                       OrgName = Ass.Hospital.Organization.Name,
                                       OrgNameAr = Ass.Hospital.Organization.NameAr,
                                       PurchaseDate = Ass.PurchaseDate,
                                       BrandId = Ass.MasterAsset.BrandId

                                       #endregion

                                   }).ToList();
                }
            }
     


            return query;

        }

        public IndexAssetDetailVM GetHospitalAssetsByGovIdAndDeptIdAndHospitalId(int departmentId, int govId, int hospitalId,  int pageNumber, int pageSize)
        {

            IndexAssetDetailVM mainClass = new IndexAssetDetailVM();
            List<IndexAssetDetailVM.GetData> list = new List<IndexAssetDetailVM.GetData>();

;


        



            var lstAllAssets = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.Hospital).Include(a => a.Hospital.Governorate).Include(a => a.Hospital.City)
                                     .Include(a => a.Hospital.Organization).Include(a => a.Hospital.SubOrganization)
                                                                                             .OrderBy(a => a.Barcode).ToList();


            if (lstAllAssets.Count > 0)
            {
                foreach (var asset in lstAllAssets)
                {


                    IndexAssetDetailVM.GetData detail = new IndexAssetDetailVM.GetData();



                    detail.Id = asset.Id;
                    detail.DepartmentId = asset.DepartmentId != null ? asset.DepartmentId : 0;
                    detail.Code = asset.Code;

                  
                    detail.Price = asset.Price;
                    detail.BarCode = asset.Barcode;
                    detail.MasterImg = asset.MasterAsset.AssetImg;
                    detail.Serial = asset.SerialNumber;
                    detail.BrandName = asset.MasterAsset.Brand != null ? asset.MasterAsset.Brand.Name : "";
                    detail.BrandNameAr = asset.MasterAsset.Brand != null ? asset.MasterAsset.Brand.NameAr : "";
                    detail.Model = asset.MasterAsset.ModelNumber;
                    detail.SerialNumber = asset.SerialNumber;
                    detail.MasterAssetId = asset.MasterAssetId;
                    detail.PurchaseDate = asset.PurchaseDate;
                    detail.HospitalId = asset.Hospital.Id;
                    detail.HospitalName = asset.Hospital.Name;
                    detail.HospitalNameAr = asset.Hospital.NameAr;
                    detail.AssetName = asset.MasterAsset.Name;
                    detail.AssetNameAr = asset.MasterAsset.NameAr;
                  
                    detail.GovernorateId = asset.Hospital.GovernorateId;
                    detail.GovernorateName = asset.Hospital.Governorate.Name;
                    detail.GovernorateNameAr = asset.Hospital.Governorate.NameAr;
                    detail.CityId = asset.Hospital.CityId;
                    detail.CityName = asset.Hospital.City != null ? asset.Hospital.City.Name : "";
                    detail.CityNameAr = asset.Hospital.City != null ? asset.Hospital.City.NameAr : "";
                    detail.OrganizationId = asset.Hospital.OrganizationId;
                    detail.OrgName = asset.Hospital.Organization.Name;
                    detail.OrgNameAr = asset.Hospital.Organization.NameAr;
                    detail.SubOrganizationId = asset.Hospital.SubOrganizationId;
                    detail.SubOrgName = asset.Hospital.SubOrganization.Name;
                    detail.SubOrgNameAr = asset.Hospital.SubOrganization.NameAr;
                    detail.SupplierName = asset.Supplier != null ? asset.Supplier.Name : "";
                    detail.SupplierNameAr = asset.Supplier != null ? asset.Supplier.NameAr : "";
                    detail.QrFilePath = asset.QrFilePath;
                    detail.QrData = asset.QrData;



                    list.Add(detail);
                }





                if (govId != 0)
                {

                    list = list.Where(h => h.GovernorateId == govId).ToList();
                }
                if (hospitalId != 0)
                {
                    list = list.Where(h => h.HospitalId == hospitalId).ToList();
                }
                if (departmentId != 0)
                {


                    list = list.Where(h => h.DepartmentId == departmentId && h.HospitalId == hospitalId).ToList();
                }
                else
                {
                    list = list.ToList();
                }
                var requestsPerPage = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                mainClass.Results = requestsPerPage;
                mainClass.Count = list.Count();
                return mainClass;
            }
            return null;


        }
        public List<BrandGroupVM> GroupAssetDetailsByBrand(FilterHospitalAsset data)
        {
            var AssetModel = FindAllFilteredAssetsForGrouping(data);
            List<BrandGroupVM> lstAssetBrand = new List<BrandGroupVM>();

            var LstBrandsByAssets = AssetModel.GroupBy(a => a.BrandId).ToList();

            if (LstBrandsByAssets is not null)
            {
                if (LstBrandsByAssets.Count > 0)
                {
                    foreach (var item in LstBrandsByAssets)
                    {
                        BrandGroupVM AssetBrandObj = new BrandGroupVM();
                        AssetBrandObj.Id = int.Parse(item.Key.ToString());
                        AssetBrandObj.Name = item.First().BrandName;
                        AssetBrandObj.NameAr = item.First().BrandNameAr;
                        AssetBrandObj.AssetList = AssetModel.ToList().Where(w => w.BrandId == AssetBrandObj.Id).ToList();
                        if (AssetBrandObj.AssetList.Count > 0)
                        {
                            lstAssetBrand.Add(AssetBrandObj);
                        }
                    }

                }
            }


        



            return lstAssetBrand;
        }
        public List<BrandGroupVM> GroupAssetDetailsByGovernorate(FilterHospitalAsset data)
        {




            var AssetModel = FindAllFilteredAssetsForGrouping(data);
            List<BrandGroupVM> lstAssetBrand = new List<BrandGroupVM>();

            var LstBrandsByAssets = AssetModel.GroupBy(a => a.GovernorateId).ToList();

            if (LstBrandsByAssets is not null)
            {
                if (LstBrandsByAssets.Count > 0)
                {
                    foreach (var item in LstBrandsByAssets)
                    {
                        BrandGroupVM AssetBrandObj = new BrandGroupVM();
                        AssetBrandObj.Id = item.Key != null ? int.Parse(item.Key.ToString()) : 0; ;
                        AssetBrandObj.Name = item.First().GovernorateName;
                        AssetBrandObj.NameAr = item.First().GovernorateNameAr;
                        AssetBrandObj.AssetList = AssetModel.Where(w => w.GovernorateId == AssetBrandObj.Id).ToList();
                        if (AssetBrandObj.AssetList.Count > 0)
                        {
                            lstAssetBrand.Add(AssetBrandObj);
                        }
                    }

                }
            }






            return lstAssetBrand;




        }







        public List<BrandGroupVM> GroupAssetDetailsByOrganization(FilterHospitalAsset data)
        {




            var AssetModel = FindAllFilteredAssetsForGrouping(data);
            List<BrandGroupVM> lstAssetBrand = new List<BrandGroupVM>();

            var LstBrandsByAssets = AssetModel.GroupBy(a => a.OrganizationId).ToList();

            if (LstBrandsByAssets is not null)
            {
                if (LstBrandsByAssets.Count > 0)
                {
                    foreach (var item in LstBrandsByAssets)
                    {
                        BrandGroupVM AssetBrandObj = new BrandGroupVM();
                        AssetBrandObj.Id = item.Key != null ? int.Parse(item.Key.ToString()) : 0; ;
                        AssetBrandObj.Name = item.First().OrgName;
                        AssetBrandObj.NameAr = item.First().OrgNameAr;
                        AssetBrandObj.AssetList = AssetModel.Where(w => w.OrganizationId == AssetBrandObj.Id).ToList();
                        if (AssetBrandObj.AssetList.Count > 0)
                        {
                            lstAssetBrand.Add(AssetBrandObj);
                        }
                    }

                }
            }






            return lstAssetBrand;




        }
        public List<BrandGroupVM> GroupAssetDetailsBySubOrganization(FilterHospitalAsset data)
        {




            var AssetModel = FindAllFilteredAssetsForGrouping(data);
            List<BrandGroupVM> lstAssetBrand = new List<BrandGroupVM>();

            var LstBrandsByAssets = AssetModel.GroupBy(a => a.SubOrganizationId).ToList();

            if (LstBrandsByAssets is not null)
            {
                if (LstBrandsByAssets.Count > 0)
                {
                    foreach (var item in LstBrandsByAssets)
                    {
                        BrandGroupVM AssetBrandObj = new BrandGroupVM();
                        AssetBrandObj.Id = item.Key != null ? int.Parse(item.Key.ToString()) : 0; ;
                        AssetBrandObj.Name = item.First().SubOrgName;
                        AssetBrandObj.NameAr = item.First().SubOrgNameAr;
                        AssetBrandObj.AssetList = AssetModel.Where(w => w.SubOrganizationId == AssetBrandObj.Id).ToList();
                        if (AssetBrandObj.AssetList.Count > 0)
                        {
                            lstAssetBrand.Add(AssetBrandObj);
                        }
                    }

                }
            }






            return lstAssetBrand;




        }
        public List<BrandGroupVM> GroupAssetDetailsByHospital(FilterHospitalAsset data)
        {




            var AssetModel = FindAllFilteredAssetsForGrouping(data);
            List<BrandGroupVM> lstAssetBrand = new List<BrandGroupVM>();

            var LstBrandsByAssets = AssetModel.GroupBy(a => a.HospitalId).ToList();

            if (LstBrandsByAssets is not null)
            {
                if (LstBrandsByAssets.Count > 0)
                {
                    foreach (var item in LstBrandsByAssets)
                    {
                        BrandGroupVM AssetBrandObj = new BrandGroupVM();
                        AssetBrandObj.Id = item.Key != null ? int.Parse(item.Key.ToString()) : 0; ;
                        AssetBrandObj.Name = item.First().HospitalName;
                        AssetBrandObj.NameAr = item.First().HospitalNameAr;
                        AssetBrandObj.AssetList = AssetModel.Where(w => w.HospitalId == AssetBrandObj.Id).ToList();
                        if (AssetBrandObj.AssetList.Count > 0)
                        {
                            lstAssetBrand.Add(AssetBrandObj);
                        }
                    }

                }
            }






            return lstAssetBrand;




        }
        public IEnumerable<IndexAssetDetailVM.GetData> GetAll()
        {
            int hospitalTypeNum = 0;

            List<IndexAssetDetailVM.GetData> list = new List<IndexAssetDetailVM.GetData>();
            var lstAssetDetails = _context.AssetDetails.Include(a => a.MasterAsset)
                .Include(a => a.MasterAsset.Brand)

                .Include(a => a.Hospital)
                .Include(a => a.Hospital.Governorate)
                .Include(a => a.Hospital.City)
                .ToList();
            if (lstAssetDetails.Count > 0)
            {
                foreach (var asset in lstAssetDetails)
                {
                    IndexAssetDetailVM.GetData item = new IndexAssetDetailVM.GetData();
                    item.Id = asset.Id;
                    item.Code = asset.Code;
                    item.MasterImg = asset.MasterAsset.AssetImg != "" ? asset.MasterAsset.AssetImg : "";
                    item.Model = asset.MasterAsset.ModelNumber;
                    item.Price = asset.Price;
                    item.Serial = asset.SerialNumber;
                    item.BarCode = asset.Barcode;
                    item.SerialNumber = asset.SerialNumber;
                    item.PurchaseDate = asset.PurchaseDate;
                    item.SupplierId = asset.SupplierId;
                    item.DepartmentId = asset.DepartmentId;
                    item.BrandName = asset.MasterAsset.Brand != null ? asset.MasterAsset.Brand.Name : "";
                    item.BrandNameAr = asset.MasterAsset.Brand != null ? asset.MasterAsset.Brand.NameAr : "";
                    item.HospitalId = asset.Hospital != null ? asset.Hospital.Id : 0;
                    item.HospitalName = asset.HospitalId > 0 ? asset.Hospital.Name : "";
                    item.HospitalNameAr = asset.HospitalId > 0 ? asset.Hospital.NameAr : "";
                    item.AssetName = asset.MasterAssetId > 0 ? asset.MasterAsset.Name : "";
                    item.AssetNameAr = asset.MasterAssetId > 0 ? asset.MasterAsset.NameAr : "";
                    item.GovernorateName = asset.HospitalId > 0 ? asset.Hospital.Governorate.Name : "";
                    item.GovernorateNameAr = asset.HospitalId > 0 ? asset.Hospital.Governorate.NameAr : "";
                    item.CityName = asset.HospitalId > 0 ? asset.Hospital.City.Name : "";
                    item.CityNameAr = asset.HospitalId > 0 ? asset.Hospital.City.NameAr : "";
                    //item.QrFilePath = asset.QrFilePath;
                    if (hospitalTypeNum == 1)
                    {
                        item.QrFilePath = asset.QrFilePath;
                    }
                    if (hospitalTypeNum == 2)
                    {
                        item.QrFilePath = asset.QrData;
                    }
                    if (hospitalTypeNum == 2)
                    {
                        item.QrFilePath = asset.QrData;
                    }
                    item.QrData = asset.QrData;
                    item.CreatedBy = asset.CreatedBy;

                    list.Add(item);
                }
            }
            return list;
        }

        public EditAssetDetailVM GetById(int id)
        {
            var lstAssetDetails = _context.AssetDetails

                                    .Include(a => a.MasterAsset)
                                .Include(a => a.MasterAsset.Brand)


                                .Include(a => a.Hospital)
                                .Include(a => a.Hospital.Governorate)
                                .Include(a => a.Hospital.City)
                                .Include(a => a.Hospital.Organization)
                                .Include(a => a.Hospital.SubOrganization)
                                  .Include(a => a.Supplier)

                               .ToList().Where(a => a.Id == id).ToList();//.FirstOrDefault();

            if (lstAssetDetails.Count() > 0)
            {
                var assetDetailObj = lstAssetDetails[0];
                EditAssetDetailVM item = new EditAssetDetailVM();

                item.Id = assetDetailObj.Id;
                item.CreatedBy = assetDetailObj.CreatedBy;
                item.MasterAssetId = assetDetailObj.MasterAssetId;
                item.AssetName = assetDetailObj.MasterAsset?.Name;
                item.AssetNameAr = assetDetailObj.MasterAsset?.NameAr;
                item.Model = assetDetailObj.MasterAsset?.ModelNumber;
                item.Code = assetDetailObj.Code;
                item.PurchaseDate = assetDetailObj.PurchaseDate != null ? assetDetailObj.PurchaseDate.Value.ToShortDateString() : "";
                item.Price = assetDetailObj.Price;
                item.SerialNumber = assetDetailObj.SerialNumber;
                item.Remarks = assetDetailObj.Remarks;

                item.BarCode = assetDetailObj.Barcode;
                item.InstallationDate = assetDetailObj.InstallationDate != null ? assetDetailObj.InstallationDate.Value.ToShortDateString() : "";
                item.OperationDate = assetDetailObj.OperationDate != null ? assetDetailObj.OperationDate.Value.ToShortDateString() : "";
                item.ReceivingDate = assetDetailObj.ReceivingDate != null ? assetDetailObj.ReceivingDate.Value.ToShortDateString() : "";
                item.PONumber = assetDetailObj.PONumber;
                item.WarrantyExpires = assetDetailObj.WarrantyExpires;

                item.QrFilePath = assetDetailObj.QrFilePath;
                item.QrData = assetDetailObj.QrData;

                item.AssetImg = assetDetailObj.MasterAsset?.AssetImg;

                item.BuildingId = assetDetailObj.BuildingId;

                item.DepartmentId = assetDetailObj.DepartmentId;
                item.SupplierId = assetDetailObj.SupplierId;
                item.HospitalId = assetDetailObj.HospitalId;
                if (assetDetailObj.HospitalId != null)
                {
                    item.HospitalName = assetDetailObj.Hospital?.Name;
                    item.HospitalNameAr = assetDetailObj.Hospital?.NameAr;
                }
                item.MasterAssetId = assetDetailObj.MasterAssetId;
                item.WarrantyStart = assetDetailObj.WarrantyStart != null ? assetDetailObj.WarrantyStart.Value.ToShortDateString() : "";
                item.WarrantyEnd = assetDetailObj.WarrantyEnd != null ? assetDetailObj.WarrantyEnd.Value.ToShortDateString() : "";
                item.CostCenter = assetDetailObj.CostCenter;
                item.DepreciationRate = assetDetailObj.DepreciationRate;
                item.QrFilePath = assetDetailObj.QrFilePath;

                item.GovernorateId = assetDetailObj.Hospital?.GovernorateId;
                item.CityId = assetDetailObj.Hospital?.CityId;
                item.OrganizationId = assetDetailObj.Hospital?.OrganizationId;
                item.SubOrganizationId = assetDetailObj.Hospital?.SubOrganizationId;



                item.SupplierName = assetDetailObj.Supplier != null ? assetDetailObj.Supplier.Name : "";
                item.SupplierNameAr = assetDetailObj.Supplier != null ? assetDetailObj.Supplier.NameAr : "";

                item.BrandId = assetDetailObj.MasterAsset.Brand != null ? assetDetailObj.MasterAsset.BrandId : 0;
                item.BrandName = assetDetailObj.MasterAsset.Brand != null ? assetDetailObj.MasterAsset.Brand.Name : "";
                item.BrandNameAr = assetDetailObj.MasterAsset.Brand != null ? assetDetailObj.MasterAsset.Brand.NameAr : "";

                return item;

            }
            return null;
        }
        public int Add(CreateAssetDetailVM model)
        {
            AssetDetail assetDetailObj = new AssetDetail();
            try
            {
                if (model != null)
                {
                    assetDetailObj.Code = model.Code;
                    if (model.PurchaseDate != "")
                        assetDetailObj.PurchaseDate = DateTime.Parse(model.PurchaseDate);
                    assetDetailObj.Price = model.Price;
                    assetDetailObj.SerialNumber = model.SerialNumber;
                    assetDetailObj.Remarks = model.Remarks;
                    assetDetailObj.Barcode = model.Barcode;
                    if (model.InstallationDate != "")
                        assetDetailObj.InstallationDate = DateTime.Parse(model.InstallationDate);
                    assetDetailObj.RoomId = model.RoomId;
                    assetDetailObj.FloorId = model.FloorId;
                    assetDetailObj.BuildingId = model.BuildingId;
                    //if (model.ReceivingDate != "")
                    //    assetDetailObj.ReceivingDate = DateTime.Parse(model.ReceivingDate);
                    //if (model.OperationDate != "")
                    //    assetDetailObj.OperationDate = DateTime.Parse(model.OperationDate);
                    assetDetailObj.PONumber = model.PONumber;
                    assetDetailObj.DepartmentId = model.DepartmentId;
                    if (model.SupplierId > 0)
                        assetDetailObj.SupplierId = model.SupplierId;
                    assetDetailObj.HospitalId = model.HospitalId;
                    assetDetailObj.MasterAssetId = model.MasterAssetId;
                    //if (model.WarrantyStart != "")
                    //    assetDetailObj.WarrantyStart = DateTime.Parse(model.WarrantyStart);
                    //if (model.WarrantyEnd != "")
                    //    assetDetailObj.WarrantyEnd = DateTime.Parse(model.WarrantyEnd);

                    //assetDetailObj.CreatedBy = model.CreatedBy;
                    //assetDetailObj.DepreciationRate = model.DepreciationRate;
                    //assetDetailObj.CostCenter = model.CostCenter;
                    //assetDetailObj.WarrantyExpires = model.WarrantyExpires;
                    //assetDetailObj.FixCost = model.FixCost;

                    _context.AssetDetails.Add(assetDetailObj);
                    _context.SaveChanges();

                    model.Id = assetDetailObj.Id;
                    int assetDetailId = model.Id;


                    return assetDetailId;
                }
            }
            //catch (Exception ex)
            //{
            //    string str = ex.Message;
            //}
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return assetDetailObj.Id;
        }
        public int Delete(int id)
        {
            var assetDetailObj = _context.AssetDetails.Find(id);
            try
            {
                if (assetDetailObj != null)
                {
                    _context.AssetDetails.Remove(assetDetailObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }

            return 0;
        }
        public int Update(EditAssetDetailVM model)
        {
            try
            {
                var assetDetailObj = _context.AssetDetails.Find(model.Id);
                if (assetDetailObj != null)
                {
                    assetDetailObj.Id = model.Id;
                    assetDetailObj.Code = model.Code;
                    assetDetailObj.Price = model.Price;
                    assetDetailObj.SerialNumber = model.SerialNumber;
                    assetDetailObj.Remarks = model.Remarks;
                    assetDetailObj.Barcode = model.BarCode;
                    assetDetailObj.CreatedBy = model.CreatedBy;
                    assetDetailObj.FixCost = model.FixCost;
                    assetDetailObj.PONumber = model.PONumber;
                    assetDetailObj.DepartmentId = model.DepartmentId;
                    assetDetailObj.SupplierId = model.SupplierId;
                    assetDetailObj.HospitalId = model.HospitalId;
                    assetDetailObj.MasterAssetId = model.MasterAssetId;
                    assetDetailObj.RoomId = model.RoomId;
                    assetDetailObj.FloorId = model.FloorId;
                    assetDetailObj.BuildingId = model.BuildingId;
                    assetDetailObj.WarrantyExpires = model.WarrantyExpires;
                    assetDetailObj.DepreciationRate = model.DepreciationRate;
                    assetDetailObj.CostCenter = model.CostCenter;
                    assetDetailObj.PurchaseDate = model.PurchaseDate != null ? DateTime.Parse(model.PurchaseDate) : null;
                    assetDetailObj.InstallationDate = model.InstallationDate != null ? DateTime.Parse(model.InstallationDate) : null;

                    //assetDetailObj.ReceivingDate = model.ReceivingDate != null ? DateTime.Parse(model.ReceivingDate) : null;
                    //assetDetailObj.OperationDate = model.OperationDate != null ? DateTime.Parse(model.OperationDate) : null;
                    //assetDetailObj.WarrantyStart = model.WarrantyStart != null ? DateTime.Parse(model.WarrantyStart) : null;
                    //assetDetailObj.WarrantyEnd = model.WarrantyEnd != null ? DateTime.Parse(model.WarrantyEnd) : null;

                    _context.Entry(assetDetailObj).State = EntityState.Modified;
                    _context.SaveChanges();
                    return assetDetailObj.Id;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }
        public int CreateAssetDetailDocuments(AssetDetailAttachment attachObj)
        {
            AssetDetailAttachment assetAttachmentObj = new AssetDetailAttachment();
            assetAttachmentObj.AssetDetailId = attachObj.AssetDetailId;
            assetAttachmentObj.Title = attachObj.Title;
            assetAttachmentObj.FileName = attachObj.FileName;
            assetAttachmentObj.HospitalId = attachObj.HospitalId;
            _context.AssetDetailAttachments.Add(assetAttachmentObj);
            _context.SaveChanges();
            int Id = assetAttachmentObj.Id;
            return Id;
        }
        public int DeleteAssetDetailAttachment(int id)
        {
            try
            {
                var attachObj = _context.AssetDetailAttachments.Find(id);
                _context.AssetDetailAttachments.Remove(attachObj);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }
        public IEnumerable<AssetDetailAttachment> GetAttachmentByAssetDetailId(int assetId)
        {
            return _context.AssetDetailAttachments.Where(a => a.AssetDetailId == assetId).ToList();
        }
        public ViewAssetDetailVM ViewAssetDetailById(int id)
        {
            ViewAssetDetailVM model = new ViewAssetDetailVM();
            var lstHospitalAssets = _context.AssetDetails.Include(a => a.Supplier)
                                                    .Include(a => a.MasterAsset).Include(a => a.Hospital)
                                                    .Include(a => a.Hospital.Governorate)
                                                    .Include(a => a.Hospital.City)
                                                    .Include(a => a.Hospital.Organization)
                                                    .Include(a => a.Hospital.SubOrganization)
                                                    .Include(a => a.MasterAsset.Brand)
                                                    .Include(a => a.MasterAsset.Category)
                                                    .Include(a => a.MasterAsset.SubCategory)
                                                    .Include(a => a.MasterAsset.ECRIS)
                                                    .Include(a => a.MasterAsset.AssetPeriority)
                                                    .Include(a => a.MasterAsset.Origin).ToList()
                                                    .Where(a => a.Id == id).ToList();
            if (lstHospitalAssets.Count() > 0)
            {
                var detailObj = lstHospitalAssets[0];
                model.Id = detailObj.Id;
                model.Code = detailObj.Code;

                model.Price = detailObj.Price.ToString();
                model.SerialNumber = detailObj.SerialNumber;
                model.Remarks = detailObj.Remarks;
                model.BarCode = detailObj.Barcode;

                model.PurchaseDate = detailObj.PurchaseDate != null && detailObj.PurchaseDate.Value.Year != 1900 ? detailObj.PurchaseDate.ToString() : "";
                model.InstallationDate = detailObj.InstallationDate != null && detailObj.InstallationDate.Value.Year != 1900 ? detailObj.InstallationDate.Value.ToShortDateString() : "";
                model.ReceivingDate = detailObj.ReceivingDate != null && detailObj.ReceivingDate.Value.Year != 1900 ? detailObj.ReceivingDate.Value.ToShortDateString() : "";
                model.OperationDate = detailObj.OperationDate != null && detailObj.OperationDate.Value.Year != 1900 ? detailObj.OperationDate.Value.ToShortDateString() : "";
                if (detailObj.WarrantyExpires != null)
                {
                    model.WarrantyExpires = detailObj.WarrantyExpires + " Months";
                    model.WarrantyExpiresAr = detailObj.WarrantyExpires + "  شهر";
                }
                model.WarrantyStart = detailObj.WarrantyStart != null ? detailObj.WarrantyStart.Value.ToShortDateString() : "";
                model.WarrantyEnd = detailObj.WarrantyEnd != null ? detailObj.WarrantyEnd.Value.ToShortDateString() : "";
                model.CostCenter = detailObj.CostCenter;
                model.DepreciationRate = detailObj.DepreciationRate;
                model.PONumber = detailObj.PONumber;
                model.QrFilePath = detailObj.QrFilePath;
                model.MasterAssetId = detailObj.MasterAsset.Id;
                model.AssetName = detailObj.MasterAsset.Name;
                model.AssetNameAr = detailObj.MasterAsset.NameAr;
                model.MasterCode = detailObj.MasterAsset.Code;
                model.VersionNumber = detailObj.MasterAsset.VersionNumber;
                model.ModelNumber = detailObj.MasterAsset.ModelNumber;
                model.ExpectedLifeTime = detailObj.MasterAsset.ExpectedLifeTime != null ? (int)detailObj.MasterAsset.ExpectedLifeTime : 0;
                model.Description = detailObj.MasterAsset.Description;
                model.DescriptionAr = detailObj.MasterAsset.DescriptionAr;
                model.Length = detailObj.MasterAsset.Length.ToString();
                model.Width = detailObj.MasterAsset.Width.ToString();
                model.Weight = detailObj.MasterAsset.Weight.ToString();
                model.Height = detailObj.MasterAsset.Height.ToString();
                model.AssetImg = detailObj.MasterAsset.AssetImg;

                if (detailObj.MasterAsset.Category != null)
                {
                    model.CategoryName = detailObj.MasterAsset.Category.Name;
                    model.CategoryNameAr = detailObj.MasterAsset.Category.NameAr;
                }
                if (detailObj.Hospital != null)
                {
                    model.HospitalId = detailObj.Hospital.Id;
                    model.HospitalName = detailObj.Hospital.Name;
                    model.HospitalNameAr = detailObj.Hospital.NameAr;
                }
                if (detailObj.Hospital?.Governorate != null)
                {
                    model.GovernorateName = detailObj.Hospital.Governorate.Name;
                    model.GovernorateNameAr = detailObj.Hospital.Governorate.NameAr;
                }
                if (detailObj.Hospital?.City != null)
                {
                    model.CityName = detailObj.Hospital.City.Name;
                    model.CityNameAr = detailObj.Hospital.City.NameAr;
                }
                if (detailObj.Hospital?.Organization != null)
                {
                    model.OrgName = detailObj.Hospital.Organization.Name;
                    model.OrgNameAr = detailObj.Hospital.Organization.NameAr;
                }

                if (detailObj.Hospital?.SubOrganization != null)
                {
                    model.SubOrgName = detailObj.Hospital.SubOrganization.Name;
                    model.SubOrgNameAr = detailObj.Hospital.SubOrganization.NameAr;
                }
                if (detailObj.Supplier != null)
                {
                    model.SupplierName = detailObj.Supplier.Name;
                    model.SupplierNameAr = detailObj.Supplier.NameAr;
                }
                if (detailObj.MasterAsset.Category != null)
                {
                    model.CategoryName = detailObj.MasterAsset.Category.Name;
                    model.CategoryNameAr = detailObj.MasterAsset.Category.NameAr;
                }
                if (detailObj.MasterAsset.SubCategory != null)
                {
                    model.SubCategoryName = detailObj.MasterAsset.SubCategory.Name;
                    model.SubCategoryNameAr = detailObj.MasterAsset.SubCategory.NameAr;
                }
                if (detailObj.MasterAsset.Origin != null)
                {
                    model.OriginName = detailObj.MasterAsset.Origin.Name;
                    model.OriginNameAr = detailObj.MasterAsset.Origin.NameAr;
                }
                if (detailObj.MasterAsset.Brand != null)
                {
                    model.BrandName = detailObj.MasterAsset.Brand.Name;
                    model.BrandNameAr = detailObj.MasterAsset.Brand.NameAr;
                }
                if (detailObj.MasterAsset.AssetPeriority != null)
                {
                    model.PeriorityName = detailObj.MasterAsset.AssetPeriority.Name;
                    model.PeriorityNameAr = detailObj.MasterAsset.AssetPeriority.NameAr;
                }
            }
            return model;
        }
        public IEnumerable<IndexAssetDetailVM.GetData> AutoCompleteAssetBarCode(string barcode, int hospitalId)
        {


            List<IndexAssetDetailVM.GetData> list = new List<IndexAssetDetailVM.GetData>();
            var lst = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.MasterAsset.Brand)
                .Include(a => a.Hospital).Where(a => a.Barcode.Contains(barcode)).OrderBy(a => a.Barcode).ToList();
            if (hospitalId == 0)
            {
                lst = lst.ToList();
            }
            else
            {
                lst = lst.Where(a => a.HospitalId == hospitalId).ToList();
            }
            if (lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    IndexAssetDetailVM.GetData getDataObj = new IndexAssetDetailVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.Code = item.Code;
                    getDataObj.BarCode = item.Barcode;
                    getDataObj.Price = item.Price;
                    getDataObj.MasterAssetName = item.MasterAsset != null ? item.MasterAsset.Name : "";
                    getDataObj.MasterAssetNameAr = item.MasterAsset != null ? item.MasterAsset.NameAr : "";
                    getDataObj.BrandName = item.MasterAsset.Brand != null ? item.MasterAsset.Brand.Name : "";
                    getDataObj.BrandNameAr = item.MasterAsset.Brand != null ? item.MasterAsset.Brand.NameAr : "";
                    getDataObj.Model = item.MasterAsset != null ? item.MasterAsset.ModelNumber : "";
                    getDataObj.AssetBarCode = item.Barcode;
                    getDataObj.BarCode = item.Barcode;
                    getDataObj.Serial = item.SerialNumber;
                    getDataObj.SerialNumber = item.SerialNumber;
                    getDataObj.MasterAssetId = item.MasterAssetId;
                    getDataObj.PurchaseDate = item.PurchaseDate;
                    getDataObj.HospitalId = item.HospitalId;
                    getDataObj.HospitalName = item.Hospital.Name;
                    getDataObj.HospitalNameAr = item.Hospital.NameAr;
                    getDataObj.AssetName = item.MasterAsset.Name;
                    getDataObj.AssetNameAr = item.MasterAsset.NameAr;

                    list.Add(getDataObj);
                }
            }
            return list;
        }
        public IEnumerable<IndexAssetDetailVM.GetData> AutoCompleteAssetSerial(string serial, int hospitalId)
        {
            List<IndexAssetDetailVM.GetData> list = new List<IndexAssetDetailVM.GetData>();
            var lst = _context.AssetDetails.Include(a => a.MasterAsset).Include(a => a.MasterAsset.Brand)
                .Include(a => a.Hospital).Where(a => a.SerialNumber.Contains(serial)).OrderBy(a => a.SerialNumber).ToList();
            if (hospitalId == 0)
            {
                lst = lst.ToList();
            }
            else
            {
                lst = lst.Where(a => a.HospitalId == hospitalId).ToList();
            }
            if (lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    IndexAssetDetailVM.GetData getDataObj = new IndexAssetDetailVM.GetData();
                    getDataObj.Id = item.Id;
                    getDataObj.Code = item.Code;
                    getDataObj.BarCode = item.Barcode;
                    getDataObj.Price = item.Price;
                    getDataObj.MasterAssetName = item.MasterAsset != null ? item.MasterAsset.Name : "";
                    getDataObj.MasterAssetNameAr = item.MasterAsset != null ? item.MasterAsset.NameAr : "";
                    getDataObj.BrandName = item.MasterAsset.Brand != null ? item.MasterAsset.Brand.Name : "";
                    getDataObj.BrandNameAr = item.MasterAsset.Brand != null ? item.MasterAsset.Brand.NameAr : "";
                    getDataObj.Model = item.MasterAsset != null ? item.MasterAsset.ModelNumber : "";
                    getDataObj.AssetBarCode = item.Barcode;
                    getDataObj.BarCode = item.Barcode;
                    getDataObj.Serial = item.SerialNumber;
                    getDataObj.SerialNumber = item.SerialNumber;
                    getDataObj.MasterAssetId = item.MasterAssetId;
                    getDataObj.PurchaseDate = item.PurchaseDate;
                    getDataObj.HospitalId = item.HospitalId;
                    getDataObj.HospitalName = item.Hospital.Name;
                    getDataObj.HospitalNameAr = item.Hospital.NameAr;
                    getDataObj.AssetName = item.MasterAsset.Name;
                    getDataObj.AssetNameAr = item.MasterAsset.NameAr;
                    list.Add(getDataObj);
                }
            }
            return list;
        }

        public IndexAssetDetailVM GetAll(CountOfAssetsSpecParams @params)
        {
            IEnumerable<AssetDetail>? result = _context.AssetDetails
                .Include(p => p.MasterAsset).Include(p => p.Hospital)
                .Include(p => p.MasterAsset.Brand)
                .Include(p => p.MasterAsset.Category)
              .Include(p => p.MasterAsset.SubCategory)
              .Include(p => p.Hospital.Governorate)
                .ToList();
            IndexAssetDetailVM mainClass = new IndexAssetDetailVM();
            List<IndexAssetDetailVM.GetData> indexCountOfAssetsVMs = new List<IndexAssetDetailVM.GetData>();
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
                IndexAssetDetailVM.GetData data = new IndexAssetDetailVM.GetData();

                data.Id = item.Id;
                data.AssetName = item.MasterAsset?.Name;
                data.Code = item.Code;
                data.AssetNameAr = item.MasterAsset?.NameAr;
                data.BrandName = item.MasterAsset?.Brand?.Name;
                data.BrandNameAr = item.MasterAsset?.Brand?.NameAr;
                data.BarCode = item.Barcode;
                data.SubCatName = item.MasterAsset?.SubCategory != null ? item.MasterAsset.SubCategory.Name : null;
                data.SubCatNameAr = item.MasterAsset?.SubCategory != null ? item.MasterAsset.SubCategory.NameAr : null;
                data.CategoryName = item.MasterAsset?.Category != null ? item.MasterAsset.Category.Name : "";
                data.CategoryNameAr = item.MasterAsset?.Category != null ? item.MasterAsset.Category.NameAr : "";
                data.GovernorateName = item.Hospital?.Governorate != null ? item.Hospital.Governorate.Name : "";
                data.GovernorateNameAr = item.Hospital?.Governorate != null ? item.Hospital.Governorate.NameAr : "";
                data.HospitalName = item.Hospital?.Name;
                data.HospitalNameAr = item.Hospital?.NameAr;
                data.Model = item.MasterAsset?.ModelNumber;
                data.InstallationDate = item.InstallationDate;
                data.Price = item.Price;
                indexCountOfAssetsVMs.Add(data);

            }
            mainClass.Results = indexCountOfAssetsVMs;
            return mainClass;
        }

        public int CreateAssetDetailDocuments(CreateAssetDetailAttachmentVM attachObj)
        {
            AssetDetailAttachment assetAttachmentObj = new AssetDetailAttachment();
            assetAttachmentObj.AssetDetailId = attachObj.AssetDetailId;
            assetAttachmentObj.Title = attachObj.Title;
            assetAttachmentObj.FileName = attachObj.FileName;
            assetAttachmentObj.HospitalId = attachObj.HospitalId;
            _context.AssetDetailAttachments.Add(assetAttachmentObj);
            _context.SaveChanges();
            int Id = assetAttachmentObj.Id;
            return Id;
        }

        public AssetDetailAttachment GetLastDocumentForAssetDetailId(int assetDetailId)
        {
            AssetDetailAttachment documentObj = new AssetDetailAttachment();
            var lstDocuments = _context.AssetDetailAttachments.Where(a => a.AssetDetailId == assetDetailId).OrderBy(a => a.FileName).ToList();
            if (lstDocuments.Count > 0)
            {
                documentObj = lstDocuments.Last();
            }
            return documentObj;

        }


        public GeneratedAssetDetailBCVM GenerateAssetDetailBarcode()
        {
            GeneratedAssetDetailBCVM numberObj = new GeneratedAssetDetailBCVM();
            int barCode = 0;

            var lastId = _context.AssetDetails.ToList();
            if (lastId.Count > 0)
            {
                var code = lastId.LastOrDefault()?.Barcode;

                if (code.Contains('-'))
                {
                    string[] barcodenumber = code.Split("-");
                    var barcode = (int.Parse(barcodenumber[0]) + 1).ToString();
                    var lastcode = barcode.ToString().PadLeft(9, '0');
                    numberObj.BarCode = lastcode;
                }
                else
                {
                    var barcode = (int.Parse(code) + 1).ToString();
                    var lastcode = barcode.ToString().PadLeft(9, '0');
                    numberObj.BarCode = lastcode;
                }
            }
            else
            {
                numberObj.BarCode = (barCode + 1).ToString();
            }
            return numberObj;
        }

        public IEnumerable<IndexCountOfAssetsVM.Data> PyramidGovernorateChart()
        {
            var active = (from gov in _context.Governorates
                          join host in _context.Hospitals on gov.Id equals host.GovernorateId
                          join detail in _context.AssetDetails on host.Id equals detail.HospitalId into assetdetail
                          from govAssetDetailData in assetdetail.DefaultIfEmpty()

                          group new { gov, govAssetDetailData }
                          by new { Id = gov.Id, Name = gov.Name, NameAr = gov.NameAr, Population = gov.Population } into grp
                          select new IndexCountOfAssetsVM.Data
                          {
                              GovernorateId = grp.Key.Id,
                              GovernorateName = grp.Key.Name,
                              GovernorateNameAr = grp.Key.NameAr,
                              Population = grp.Key.Population,
                              Count = grp.Count()
                          }).OrderBy(a => a.Count);

            return active;
        }

        public IEnumerable<IndexCountOfAssetsVM.Data> PyramidGovernorateChartByParams(CountOfAssetsSpecParams @params)
        {
            List<Data> indexCountOfAssetsVMs = new List<Data>();

            var lstAssetGovernorate = (from gov in _context.Governorates
                                       join host in _context.Hospitals on gov.Id equals host.GovernorateId
                                       join detail in _context.AssetDetails on host.Id equals detail.HospitalId into assetdetail
                                       from govAssetDetailData in assetdetail.DefaultIfEmpty()
                                       group new { gov }
                                       by new { Id = gov.Id, Name = gov.Name, NameAr = gov.NameAr } into grp
                                       select new IndexCountOfAssetsVM.Data
                                       {
                                           GovernorateId = grp.Key.Id,
                                           GovernorateName = grp.Key.Name,
                                           GovernorateNameAr = grp.Key.NameAr,
                                           //CategoryId = grp.Key.CategoryId,
                                           Count = grp.Count()
                                       }).OrderBy(a => a.Count).ToList();

            if (@params.CategoryId?.Count > 0)
            {
                lstAssetGovernorate = lstAssetGovernorate.Where(item => @params.CategoryId.Contains(item.CategoryId)).ToList();
            }
            else
            {
                lstAssetGovernorate = lstAssetGovernorate.ToList();
            }
            return lstAssetGovernorate;
        }

        public IEnumerable<Data> PyramidGovernoratePopulationChartByParams(CountOfAssetsSpecParams @params)
        {

            var lstAssetGovernorate = (from gov in _context.Governorates
                                       join host in _context.Hospitals on gov.Id equals host.GovernorateId
                                       join detail in _context.AssetDetails on host.Id equals detail.HospitalId into assetdetail
                                       from govAssetDetailData in assetdetail.DefaultIfEmpty()
                                       group new { gov }
                                       by new { Id = gov.Id, Name = gov.Name, NameAr = gov.NameAr, Population = gov.Population } into grp
                                       select new IndexCountOfAssetsVM.Data
                                       {
                                           GovernorateId = grp.Key.Id,
                                           GovernorateName = grp.Key.Name,
                                           GovernorateNameAr = grp.Key.NameAr,
                                           Population = grp.Key.Population,
                                       }).OrderBy(a => a.Population).ToList();

            //if (@params.CategoryId?.Count > 0)
            //{
            //    lstAssetGovernorate = lstAssetGovernorate.Where(item => @params.CategoryId.Contains(item.CategoryId)).ToList();
            //}
            //else
            //{
            //    lstAssetGovernorate = lstAssetGovernorate.ToList();
            //}
            return lstAssetGovernorate;
        }

        public IEnumerable<ChartCountOfAssetDetailVM> GetAssetsCountByOrganizationsAndCategories(CountOfAssetsSpecParams @params)
        {
            var parents = _context.Organizations.ToList();
            var children = _context.AssetDetails.Include(a => a.Hospital).Include(a => a.Hospital.Governorate).ToList();
            var itemCounts = parents
                .GroupJoin(children,
                    parent => parent.Id,
                    child => child.Hospital?.OrganizationId,
                                        (parent, childGroup) => new ChartCountOfAssetDetailVM
                                        {
                                            OrganizationName = parent.Name,
                                            OrganizationNameAr = parent.NameAr,
                                            Count = childGroup.Count()
                                        });
            return itemCounts;
        }
    }
}

