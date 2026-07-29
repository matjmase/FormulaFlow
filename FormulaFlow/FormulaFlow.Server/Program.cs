using FormulaFlow.Data;
using FormulaFlow.Data.Models;
using FormulaFlow.Data.NoSql;
using FormulaFlow.Server.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<FormulaFlowContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddSingleton<NoSqlFormulaFlowContext>(_ =>
    new NoSqlFormulaFlowContext(builder.Configuration.GetConnectionString("NoSqlConnection")));

// Configure Identity with the least restrictive password settings.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 1;
    options.Password.RequiredUniqueChars = 0;
})
    .AddEntityFrameworkStores<FormulaFlowContext>()
    .AddDefaultTokenProviders()
    .AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory>();

/*
 * ==================================
 *              Repository
 * ==================================
 */

/*
 * ==================================
 *              Unit Of Work
 * ==================================
 */

/*
 * ==================================
 *              Intermediate Mappers
 * ==================================
 */

/*
 * ==================================
 *              Mappers
 * ==================================
 */

/*
 * ==================================
 *              Services
 * ==================================
 */


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
});

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply EF Core migrations at startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<FormulaFlowContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var loggerFactory = services.GetService<ILoggerFactory>();
        var logger = loggerFactory?.CreateLogger("FormulaFlow.Data.Migrations");
        logger?.LogError(ex, "An error occurred while migrating the database.");
        Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
        throw;
    }
}

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
