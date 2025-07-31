using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPA.DAL.Models
{
    public partial class CountOfAsset:BaseEntity
    {
       
        public int? GovernorateId { get; set; }
        [ForeignKey("GovernorateId")]
        public virtual Governorate? Governorate { get; set; }


        public int? OrganizationId { get; set; }
        [ForeignKey("OrganizationId")]
        public virtual Organization? Organization { get; set; }





        public int? BrandId { get; set; }
        [ForeignKey("BrandId")]
        public virtual Brand? Brand { get; set; }


        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }


        public int? Count { get; set; }
    }
}
