using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection;
using UPA.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace UPA.DAl
{
    public  class UPAModel :IdentityDbContext<AppUser>{ 
        public UPAModel(DbContextOptions<UPAModel> options) : base(options)
        {

        }
        public virtual DbSet<CountOfAsset> CountOfAssets { get; set; }

        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<Governorate> Governorates { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<SubOrganization> SubOrganizations { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }

        public DbSet<Origin> Origins { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierAttachment> SupplierAttachments { get; set; }
        public DbSet<AssetPeriority> AssetPeriorities { get; set; }
        public DbSet<MasterAssetAttachment> MasterAssetAttachments { get; set; }
        public DbSet<AssetDetailAttachment> AssetDetailAttachments { get; set; }
        public DbSet<ECRI> ECRIS { get; set; }


        public DbSet<MasterAsset> MasterAssets { get; set; }
        public DbSet<AssetDetail> AssetDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}

