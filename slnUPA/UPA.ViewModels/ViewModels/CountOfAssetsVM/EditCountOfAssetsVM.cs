using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPA.ViewModels.ViewModels.CountOfAssetsVM
{
    public class EditCountOfAssetsVM
    {
        public int Id { get; set; }
        public int? GovernorateId { get; set; }
        public string? GovernorateName { get; set; }
        public string? GovernorateNameAr { get; set; }
        public int? OrganizationId { get; set; }
        public string? OrganizationName { get; set; }
        public string? OrganizationNameAr { get; set; }
        public int? BrandId { get; set; }
        public string? BrandName { get; set; }
        public string? BrandNameAr { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryNameAr { get; set; }

        public int? Count { get; set; }
    }
}
