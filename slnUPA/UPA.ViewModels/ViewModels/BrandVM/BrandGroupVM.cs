using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPA.ViewModels.ViewModels.AssetDetailVM;

namespace  UPA.ViewModels.ViewModels.BrandVM
{
    public class BrandGroupVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? NameAr { get; set; }

        public List<IndexAssetDetailVM.GetData> AssetList { get; set; }
    }
}
