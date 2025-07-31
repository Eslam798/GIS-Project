using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPA.ViewModels.ViewModels.GovernorateVM
{
    public class IndexGovernorateVM
    {
        public List<GetData>? Results { get; set; }


        public class GetData
        {
            public int Id { get; set; }
            public int? GovernorateId { get; set; }
            public string? Code { get; set; }
            public string? GovernorateName { get; set; }
            public string? GovernorateNameAr { get; set; }
            public string? Logo { get; set; }
        }
    }
}
