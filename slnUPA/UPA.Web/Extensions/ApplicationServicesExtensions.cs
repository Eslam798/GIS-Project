using UPA.BLL.Interfaces;
using UPA.BLL.Repository;
using UPA.BLL.Repositories;
using UPA.Web.Helpers;
using UPA.DAL.Models;
using Asset.Core.Repositories;
using UPA.BLL.Interface;

namespace UPA.Web.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddScoped<ICountOfAssetService, CountOfAssetRepository>();
            services.AddScoped<IGovernorateService, GovernorateRepository>();
            services.AddScoped<ICityService, CityRepositories>();
            services.AddScoped<IBrandService, BrandRepository>();
            services.AddScoped<ICategoryService, CategoryRepository>();
            services.AddScoped<ISubCategoryService, SubCategoryRepository>();
            services.AddScoped<IOrganizationService, OrganizationRepository>();
            services.AddScoped<ISubOrganizationService, SubOrganizationRepository>();
            services.AddScoped<IHospitalService, HospitalRepositories>();
            services.AddScoped<IMasterAssetService, MasterAssetRepositories>();
            services.AddScoped<IAssetDetailService, AssetDetailRepositories>();
            services.AddScoped<IOriginService, OriginRepositories>();
            services.AddScoped<IECRIService, ECRIRepositories>();
            services.AddScoped<ISupplierService, SupplierRepositories>();
            services.AddScoped<IAssetPeriorityService, AssetPeriorityRepositories>();
            services.AddScoped<ISetting, SettingService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
            services.AddAutoMapper(typeof(MappingProfile));

            //services.Configure<ApiBehaviorOptions>(options =>
            //{
            //    options.InvalidModelStateResponseFactory = actionContext =>
            //    {
            //        var errors = actionContext.ModelState.Where(M => M.Value.Errors.Count > 0)
            //                                .SelectMany(M => M.Value.Errors)
            //                                .Select(E => E.ErrorMessage).ToArray();
            //        var errorResponse = new ApiValidationErrorResponse()
            //        {
            //            Errors = errors
            //        };
            //        return new BadRequestObjectResult(errorResponse);
            //    };
            //});

            return services;
        }
    }
}
