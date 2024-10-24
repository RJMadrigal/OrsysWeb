using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SistemaOrdenes.Models;
using SistemaOrdenes.Services;
using SistemaOrdenes.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);



// Agregar servicios al contenedor
builder.Services.AddDbContext<DbProyectoAnalisisIiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));




builder.Services.AddTransient<IRepositorioUsuarios, RepositorioUsuarios>();
builder.Services.AddTransient<IRepositorioOrdenes, RepositorioOrdenes>();



builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

//CONFIGURACION DE COOKIES Y SERVICIOS
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login";
        options.LogoutPath = "/Login/Logout";
    });
builder.Services.AddControllersWithViews();




builder.Services.AddScoped<IEmailService,EmailService>(); 
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<GenerateToken>();
builder.Services.AddScoped<HashSHA256>();

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=login}/{id?}");

app.Run();
