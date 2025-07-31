
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace UPA.DAL.Models
{
    public partial class Governorate
    {
        public int Id { get; set; }

        [StringLength(5)]
        public string? Code { get; set; }

        [StringLength(50)]
        public string? Name { get; set; }

        [StringLength(50)]
        public string? NameAr { get; set; }

        [Column(TypeName = "decimal(18, 3)")]
        public decimal? Population { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Area { get; set; }
        [Column(TypeName = "decimal(18, 8)")]
        public decimal? Latitude { get; set; }
        [Column(TypeName = "decimal(18, 8)")]
        public decimal? Longtitude { get; set; }

        [StringLength(50)]
        public string? Logo { get; set; }
    }
}
