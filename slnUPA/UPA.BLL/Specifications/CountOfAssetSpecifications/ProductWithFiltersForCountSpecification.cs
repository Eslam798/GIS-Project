
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPA.DAL.Models;

namespace UPA.BLL.Specifications
{
    public class ProductWithFiltersForCountSpecification : BaseSpecification<CountOfAsset>
    {
        //public ProductWithFiltersForCountSpecification(CountOfAssetsSpecParams productParams)
        //    : base(P =>
        //            (string.IsNullOrEmpty(productParams.Search) || P.Category.Name.ToLower().Contains(productParams.Search)) &&
        //             (!productParams.CategoryId.HasValue || P.CategoryId == productParams.CategoryId.Value) &&
        //             (!productParams.TypeId.HasValue || P.BrandId == productParams.TypeId.Value))
        //{
        //}

        public ProductWithFiltersForCountSpecification(CountOfAssetsSpecParams productParams)
        {

        }

    }
}