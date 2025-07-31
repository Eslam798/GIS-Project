using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPA.DAL.Models;

namespace UPA.BLL.Specifications
{
    public class CountOfAssetsSpecParams :  BaseSpecification<CountOfAsset>
    {
        private const int MaxPageSize = 21;

        public int PageIndex { get; set; } = 1;

        private int _pageSize = 20;
        public decimal price { get; set; }
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
        public DateTime? purchaseDateFrom { get; set; }
        public DateTime? purchaseDateTo { get; set; }



        public string? Start { get; set; }
        public string? End { get; set; }
        public List<int?>? CategoryId { get; set; }
        public List<int?>? GovId { get; set; }
       
        public List<int?>? OrgId { get; set; }
        public List<int?>? BrandId { get; set; }

        public List<int?>? SubOrgId { get; set; }

        public List<int?>? Count { get; set; }
        public string? Sort { get; set; }
        public string? SortStatus { get; set; }
        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }
    }
}
