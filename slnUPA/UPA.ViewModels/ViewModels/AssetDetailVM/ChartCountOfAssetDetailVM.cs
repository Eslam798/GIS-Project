using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPA.DAL.Models;

namespace UPA.ViewModels.ViewModels.AssetDetailVM
{
    public class ChartCountOfAssetDetailVM
    {
        public int Count { get; set; }

        public int GovernorateCount { get; set; }
        public int? OrganizationId { get; set; }

        public Organization? OrganizationObj { get; set; }

        public string? OrganizationName { get; set; }
        public string? OrganizationNameAr { get; set; }


        public List<CountGovernorateVM?>? ListGovernorates { get; set; }
    }

    public class CountGovernorateVM
    {
        public int Count { get; set; }
        public string? OrganizationName { get; set; }
        public string? OrganizationNameAr { get; set; }

       
    }


    public class ListCountGovernorateVM
    {
        public List<int?>? CountAssetsInGovernorate { get; set; }
    }

}
