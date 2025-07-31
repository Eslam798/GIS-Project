using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPA.DAL.Models
{
    public partial class Brand
    {
        public int Id { get; set; }
        [StringLength(5)]
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? NameAr { get; set; }
    }
}
