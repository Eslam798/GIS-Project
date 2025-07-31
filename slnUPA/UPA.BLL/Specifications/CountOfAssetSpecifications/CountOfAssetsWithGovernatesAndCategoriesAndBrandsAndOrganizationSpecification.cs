
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UPA.BLL.Specifications;
using UPA.DAL.Models;

namespace UPA.BLL.Specifications
{
    public class CountOfAssetsWithGovernatesAndCategoriesAndBrandsAndOrganizationSpecification : BaseSpecification<CountOfAsset>
    {
        /// This Constructor Use When Need to Get All Products
        public CountOfAssetsWithGovernatesAndCategoriesAndBrandsAndOrganizationSpecification(CountOfAssetsSpecParams productParams)
            : base()
        {
            AddInclude(P => P.Brand);
            AddInclude(P => P.Category);
            AddInclude(P => P.Governorate);
            AddInclude(P => P.Organization);
            AddOrderBy(P => P.Governorate);
            //AddOrderBy(P => P.Organization);
            ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "CategotyName":
                        AddOrderBy(P => P.Category.Name);
                        break;
                    case "BrandName":
                        AddOrderByDescending(P => P.Brand.Name);
                        break;
                    default:
                        AddOrderBy(P => P.Category.Name);
                        break;
                }
            }
        }
    }
}
