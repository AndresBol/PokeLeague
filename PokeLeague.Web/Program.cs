using Microsoft.EntityFrameworkCore;
using PokeLeague.Application.Profiles;
using PokeLeague.Application.Services.Implementations;
using PokeLeague.Application.Services.Interfaces;
using PokeLeague.Infraestructure.Data;
using PokeLeague.Infraestructure.Repository.Implementations;
using PokeLeague.Infraestructure.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//************
// =====================
// Configurar Dependency Injection
// =====================

//*** Repositories
builder.Services.AddTransient<IRepositoryRole, RepositoryRole>();
builder.Services.AddTransient<IRepositoryUser, RepositoryUser>();
builder.Services.AddTransient<IRepositoryRarity, RepositoryRarity>();
builder.Services.AddTransient<IRepositoryAuction, RepositoryAuction>();
builder.Services.AddTransient<IRepositoryAuctionBid, RepositoryAuctionBid>();
builder.Services.AddTransient<IRepositoryCard, RepositoryCard>();
builder.Services.AddTransient<IRepositoryCategory, RepositoryCategory>();
builder.Services.AddTransient<IRepositoryCategoryCard, RepositoryCategoryCard>();
builder.Services.AddTransient<IRepositoryImage, RepositoryImage>();
builder.Services.AddTransient<IRepositoryLanguage, RepositoryLanguage>();
builder.Services.AddTransient<IRepositoryPurchaseOrder, RepositoryPurchaseOrder>();
builder.Services.AddTransient<IRepositorySet, RepositorySet>();

//*** Services
builder.Services.AddTransient<IServiceRole, ServiceRole>();
builder.Services.AddTransient<IServiceUser, ServiceUser>();
builder.Services.AddTransient<IServiceRarity, ServiceRarity>();
builder.Services.AddTransient<IServiceAuction, ServiceAuction>();
builder.Services.AddTransient<IServiceAuctionBid, ServiceAuctionBid>();
builder.Services.AddTransient<IServiceCard, ServiceCard>();
builder.Services.AddTransient<IServiceCategory, ServiceCategory>();
builder.Services.AddTransient<IServiceLanguage, ServiceLanguage>();
builder.Services.AddTransient<IServicePurchaseOrder, ServicePurchaseOrder>();
builder.Services.AddTransient<IServiceSet, ServiceSet>();

// =====================
// Configurar AutoMapper
// =====================
builder.Services.AddAutoMapper(config =>
{
    //*** Profiles
    config.AddProfile<RoleProfile>();
    config.AddProfile<UserProfile>();
    config.AddProfile<RarityProfile>();
    config.AddProfile<AuctionProfile>();
    config.AddProfile<AuctionBidProfile>();
    config.AddProfile<CardProfile>();
    config.AddProfile<CategoryProfile>();
    config.AddProfile<CategoryCardProfile>();
    config.AddProfile<ImageProfile>();
    config.AddProfile<LanguageProfile>();
    config.AddProfile<PurchaseOrderProfile>();
    config.AddProfile<SetProfile>();
});

// ==============================
// Configurar SQL Server DbContext
// ==============================
var connectionString = builder.Configuration.GetConnectionString("SqlServerDataBase");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "No se encontró la cadena de conexión 'SqlServerDataBase' en appsettings.json / appsettings.Development.json.");
}

builder.Services.AddDbContext<PokeLeagueContext>(options =>
{
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        // Reintentos ante fallos transitorios (recomendado)
        sqlOptions.EnableRetryOnFailure();
    });

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

//************


var app = builder.Build();

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
