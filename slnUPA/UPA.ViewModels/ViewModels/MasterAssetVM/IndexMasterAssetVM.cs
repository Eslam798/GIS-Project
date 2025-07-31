using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPA.DAL.Models;

namespace UPA.ViewModels.ViewModels.MasterAssetVM
{
    public class IndexMasterAssetVM
    {

        public List<GetData>? Results { get; set; }
        public int Count { get; set; }

        public class GetData
        {
            public int Id { get; set; }
            public string? Code { get; set; }
            public string? Name { get; set; }
            public string? NameAr { get; set; }

            public string? AssetName { get; set; }
            public string? AssetNameAr { get; set; }



            public string? PMColor { get; set; }
            public string? PMBGColor { get; set; }



            public int? ECRIId { get; set; }
            public string? ECRIName { get; set; }
            public string? ECRINameAr { get; set; }
            public string? Model { get; set; }
            public string? ModelNumber { get; set; }
            public int? OriginId { get; set; }
            public string? OriginName { get; set; }
            public string? OriginNameAr { get; set; }
            public int? BrandId { get; set; }
            public string? BrandName { get; set; }
            public string? BrandNameAr { get; set; }

            public int? CategoryId { get; set; }
            public string? CategoryName { get; set; }
            public string? CategoryNameAr { get; set; }

            public int? SubCategoryId { get; set; }
            public string? SubCategoryName { get; set; }
            public string? SubCategoryNameAr { get; set; }



            public string? SerialNumber { get; set; }
            public string? BarCode { get; set; }

            public string? Description { get; set; }
            public string? DescriptionAr { get; set; }



            public int? ExpectedLifeTime { get; set; }
            public string? VersionNumber { get; set; }
            public int? PeriorityId { get; set; }
                   public int? PMTimeId { get; set; }
            public double? Length { get; set; }
            public double? Height { get; set; }
            public double? Width { get; set; }
            public double? Weight { get; set; }

            [StringLength(10)]
            public string? Power { get; set; }
            [StringLength(10)]
            public string? Voltage { get; set; }

            [StringLength(10)]
            public string? Ampair { get; set; }

            [StringLength(10)]
            public string? Frequency { get; set; }

            [StringLength(10)]
            public string? ElectricRequirement { get; set; }

            public string? AssetImg { get; set; }


        }
    }
}
