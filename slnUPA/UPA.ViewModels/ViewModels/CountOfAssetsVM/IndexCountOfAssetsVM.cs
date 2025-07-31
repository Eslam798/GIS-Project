using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPA.ViewModels.ViewModels.CountOfAssetsVM
{
    public class IndexCountOfAssetsVM
    {

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public List<Data>? Results { get; set; }
        public class Data
        {

            public string? GovernorateName { get; set; }
            public string? GovernorateNameAr { get; set; }
            public string? OrganizationName { get; set; }
            public string? OrganizationNameAr { get; set; }

            public string? BrandName { get; set; }
            public string? BrandNameAr { get; set; }
            public string? CategoryName { get; set; }

            public string? CategoryNameAr { get; set; }
            public int? Count { get; set; }


            public int Id { get; set; }

            public int? GovernorateId { get; set; }
            public int? OrganizationId { get; set; }
            public int? BrandId { get; set; }
            public int? CategoryId { get; set; }

            public decimal? Population { get; set; }

        }

    }


   
}
