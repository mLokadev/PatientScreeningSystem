using Microsoft.EntityFrameworkCore;
using PatientScreeningSystem.Data;
using DinkToPdf.Contracts;
using DinkToPdf;
using PatientScreeningSystem.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Configure DbContext with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add DinkToPdf
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));



// Add MVC services
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Set the default route to HomeController and Index action
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
