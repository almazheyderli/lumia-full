using Lumbia.Bussiness.Service.Abstracts;
using Lumbia.Bussiness.Service.Concretes;
using Lumbia.Core.Models;
using Lumbia.Core.RepositoryAbstracts;
using Lumbia.Data.DAL;
using Lumbia.Data.DAL.RepositoryConcretes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer("Server=WIN-0F0TGHD6FSA\\SQLEXPRESS;Database=LumbiaTask;Trusted_Connection=true;Integrated Security=true;Encrypt=false");

}
);
builder.Services.AddIdentity<User, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 8;
    opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";
    opt.Lockout.MaxFailedAccessAttempts = 3;
    opt.User.RequireUniqueEmail = true;

}).AddEntityFrameworkStores<AppDbContext>();
    

builder.Services.AddScoped<ITeamService,TeamService>();
builder.Services.AddScoped<ITeamRepository,TeamRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
