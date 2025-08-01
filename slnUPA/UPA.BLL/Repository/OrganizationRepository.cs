﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using UPA.BLL.Interfaces;
using UPA.DAl;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.OrganizationVM;

namespace UPA.BLL.Repository
{
   public class OrganizationRepository :  IOrganizationService
    {

        private readonly UPAModel _context;
        private string msg;

        public OrganizationRepository(UPAModel db)
        {
            _context = db;
        }

        public List<Organization> ListOrganizations()
        {
          return _context.Organizations.ToList();
        }



        public EditOrganizationVM GetById(int id)
        {
            var organizationObj = _context.Organizations.Where(a => a.Id == id).Select(item => new EditOrganizationVM
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                NameAr = item.NameAr,
                Address = item.Address,
                AddressAr = item.AddressAr,
                Email = item.Email,
                Mobile = item.Mobile,
                DirectorName = item.DirectorName,
                DirectorNameAr = item.DirectorNameAr
            }).First();



            return organizationObj;
        }




        public IEnumerable<IndexOrganizationVM.GetData> GetAll()
        {
            var lstOrganizations = _context.Organizations.ToList().Select(item => new IndexOrganizationVM.GetData
            {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr,
                Mobile = item.Mobile,
                Code = item.Code
            });

            return lstOrganizations;
        }

        public int Add(CreateOrganizationVM organizationVM)
        {
            Organization organizationObj = new Organization();
            try
            {
                if (organizationVM != null)
                {

                    organizationObj.Code = organizationVM.Code;
                    organizationObj.Name = organizationVM.Name;
                    organizationObj.NameAr = organizationVM.NameAr;
                    organizationObj.Address = organizationVM.Address;
                    organizationObj.AddressAr = organizationVM.AddressAr;
                    organizationObj.Email = organizationVM.Email;
                    organizationObj.Mobile = organizationVM.Mobile;
                    organizationObj.DirectorName = organizationVM.DirectorName;
                    organizationObj.DirectorNameAr = organizationVM.DirectorNameAr;
                    _context.Organizations.Add(organizationObj);
                    _context.SaveChanges();


                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return organizationObj.Id;
        }

        public int Delete(int id)
        {
            var organizationObj = _context.Organizations.Find(id);
            try
            {
                if (organizationObj != null)
                {
                    _context.Organizations.Remove(organizationObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return 0;
        }

        public int Update(EditOrganizationVM organizationVM)
        {
            try
            {

                var organizationObj = _context.Organizations.Find(organizationVM.Id);
                if (organizationObj != null)
                {
                    organizationObj.Code = organizationVM.Code;
                    organizationObj.Name = organizationVM.Name;
                    organizationObj.NameAr = organizationVM.NameAr;
                    organizationObj.Address = organizationVM.Address;
                    organizationObj.AddressAr = organizationVM.AddressAr;
                    organizationObj.Email = organizationVM.Email;
                    organizationObj.Mobile = organizationVM.Mobile;
                    organizationObj.DirectorName = organizationVM.DirectorName;
                    organizationObj.DirectorNameAr = organizationVM.DirectorNameAr;
                    _context.Entry(organizationObj).State = EntityState.Modified;
                    _context.SaveChanges();
                    return organizationObj.Id;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }

        public IEnumerable<Organization> GetAllOrganizations()
        {
            return _context.Organizations.ToList();
        }

    }
}
