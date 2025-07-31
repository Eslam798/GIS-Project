using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace UPA.DAL.Models
{
    public partial class Category
    {
        public int Id { get; set; }

        [StringLength(5)]
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? NameAr { get; set; }

    }
}
