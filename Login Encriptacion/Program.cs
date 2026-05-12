using Login_Encriptacion.Data;
using Login_Encriptacion.Services;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

var builder = WebApplication.CreateBuilder(args);

Batteries.Init();

builder.Services.AddDbContext<EncriptacionDbContext>(opc =>
    opc.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<CryptoService>();


var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var db= scope.ServiceProvider.GetRequiredService<EncriptacionDbContext>();
    var cripto = scope.ServiceProvider.GetRequiredService<CryptoService>();

    db.Database.Migrate();

    if (!db.Usuarios.Any())
    {
        db.Usuarios.Add(new Login_Encriptacion.Models.Usuario
        {
            NombreUsuario = "admin",
            PasswordHash = cripto.HashPassword("1234")
        });
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
