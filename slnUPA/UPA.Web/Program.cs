using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using UPA.Web.Extensions;
using UPA.DAl;

using Microsoft.AspNetCore.Identity;
using UPA.DAL.Models;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UPAModel>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("UPAConn")));

builder.Services.AddIdentity<AppUser, IdentityRole>()
     .AddEntityFrameworkStores<UPAModel>()
    .AddDefaultTokenProviders();

builder.Services.AddApplicationServices();
builder.Services.AddCors();
//builder.Services.AddIdentityServices(builder.Configuration);
//builder.Services.AddSwaggerDocumentation();
var app = builder.Build();
using var host = app.Services.CreateScope();
var services = host.ServiceProvider;
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

app.UseCors("AllowAll");

var LoggerFactory = services.GetRequiredService<ILoggerFactory>();
try
{
    var context = services.GetRequiredService<UPAModel>();
    await context.Database.MigrateAsync();

    //var identityContext = services.GetRequiredService<AppIdentityDbContext>();
    //await identityContext.Database.MigrateAsync();
    //var userManger = services.GetRequiredService<UserManager<AppUser>>();

}
catch (Exception ex)
{
    var logger = LoggerFactory.CreateLogger<Program>();
    logger.LogError(ex, "an error occured while migraton");
}
// Configure the HTTP request pipeline.
//app.UseMiddleware<ExceptionMiddleware>();
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseRouting();





app.UseCors(
    options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
    );
//app.Use(async (context, next) =>
//{
//    await next();
//    if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
//    {
//        context.Request.Path = "/index.html";
//        await next();
//    }
//});
app.UseDefaultFiles();
app.UseStaticFiles(); // For the wwwroot folder.
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "UploadedAttachments")),
    RequestPath = "/UploadedAttachments",
    EnableDirectoryBrowsing = true
});


app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Urls.Add("http://0.0.0.0:5000");


app.Run();
