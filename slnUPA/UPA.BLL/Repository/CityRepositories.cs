using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using UPA.BLL.Interfaces;
using UPA.DAl;
using UPA.DAL.Models;
using UPA.ViewModels.ViewModels.CityVM;

namespace UPA.BLL.Repository
{
    public class CityRepositories : ICityService
    {
        private UPAModel _context;


        public CityRepositories(UPAModel context)
        {
            _context = context;
        }


        public City GetById(int id)
        {
            var CityObj = _context.Cities.Find(id);
            return CityObj;
        }

        public IEnumerable<IndexCityVM.GetData> ListCities()
        {
            var lstCitys = _context.Cities.ToList().Select(item => new IndexCityVM.GetData
            {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr,
                Latitude = item.Latitude,
                Longtitude = item.Longtitude,
                Code = item.Code
            });

            return lstCitys;
        }
        public IEnumerable<City> GetCities()
        {
            var lstCitys = _context.Cities.ToList();

            return lstCitys;
        }

        public int Add(CreateCityVM cityVM)
        {
            City cityObj = new City();
            try
            {
                if (cityVM != null)
                {
                    cityObj.Code = cityVM.Code;
                    cityObj.Name = cityVM.Name;
                    cityObj.NameAr = cityVM.NameAr;
                    cityObj.Latitude = cityVM.Latitude;
                    cityObj.Longtitude = cityVM.Longtitude;
                    cityObj.GovernorateId = cityVM.GovernorateId != null ? (int)cityVM.GovernorateId : 0;
                    _context.Cities.Add(cityObj);
                    _context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return cityObj.Id;
        }

        public int Delete(int id)
        {
            var CityObj = _context.Cities.Find(id);
            try
            {
                if (CityObj != null)
                {
                    _context.Cities.Remove(CityObj);
                    return _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }

            return 0;
        }

        public int Update(EditCityVM cityVM)
        {
            try
            {
                var cityObj = _context.Cities.Find(cityVM.Id);
                if (cityObj != null)
                {
                    cityObj.Id = cityVM.Id;
                    cityObj.Code = cityVM.Code;
                    cityObj.Name = cityVM.Name;
                    cityObj.NameAr = cityVM.NameAr;
                    cityObj.GovernorateId = cityVM.GovernorateId != null ? (int)cityVM.GovernorateId : 0;
                    cityObj.Latitude = cityVM.Latitude;
                    cityObj.Longtitude = cityVM.Longtitude;
                    _context.Entry(cityObj).State = EntityState.Modified;
                    _context.SaveChanges();
                    return cityObj.Id;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return 0;
        }

        public IEnumerable<IndexCityVM.GetData> GetCitiesByGovernorateId(int govId)
        {
            var lstCities = _context.Cities.ToList().Where(a => a.GovernorateId == govId).Select(item => new IndexCityVM.GetData
            {
                Id = item.Id,
                Name = item.Name,
                NameAr = item.NameAr,
                Code = item.Code
            });

            return lstCities;
        }

    }
}