using FormulaFlow.Data;
using FormulaFlow.Data.Models;
using FormulaFlow.Data.NoSql;
using FormulaFlow.Data.NoSql.Models;
using FormulaFlow.Server.Dto;
using FormulaFlow.Server.Identity;
using FormulaFlow.Server.Intermediate.Dto;
using FormulaFlow.Server.Intermediate.Mapper.Database;
using FormulaFlow.Server.Intermediate.Mapper.Frontend;
using FormulaFlow.Server.Intermediate.Model.Canvas.Base;
using FormulaFlow.Server.Intermediate.Model.Card.Base;
using FormulaFlow.Server.Intermediate.Model.Parameter.Base;
using FormulaFlow.Server.Mapper;
using FormulaFlow.Server.Mapper.Base;
using FormulaFlow.Server.NoSql.Repository;
using FormulaFlow.Server.NoSql.Repository.Base;
using FormulaFlow.Server.NoSql.Service;
using FormulaFlow.Server.NoSql.Service.Base;
using FormulaFlow.Server.Repository;
using FormulaFlow.Server.Repository.Base;
using FormulaFlow.Server.Service;
using FormulaFlow.Server.Service.Base;
using FormulaFlow.Server.UnitOfWork;
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
builder.Services.AddTransient<IRepository<StockSymbol>, StockSymbolRepository>();
builder.Services.AddTransient<INoSqlRepository<StockDataEntry>, StockDataEntryRepository>();
builder.Services.AddTransient<IRepository<NetworkCanvas>, OwnerRepository<NetworkCanvas>>();
builder.Services.AddTransient<IRepository<NetworkCard>, RepositoryBase<NetworkCard>>();
builder.Services.AddTransient<IRepository<NetworkCardToNetworkCard>, RepositoryBase<NetworkCardToNetworkCard>>();
builder.Services.AddTransient<IRepository<NetworkParameter>, RepositoryBase<NetworkParameter>>();

/*
 * ==================================
 *              Unit Of Work
 * ==================================
 */
builder.Services.AddTransient<ICanvasUnitOfWork, CanvasUnitOfWork>();

/*
 * ==================================
 *              Intermediate Mappers
 * ==================================
 */
// Database mappers
builder.Services.AddTransient<IMapper<NetworkCanvas, IntermediateCanvas>, DatabaseIntermediateCanvasMapper>();
builder.Services.AddTransient<IMapper<IntermediateCanvas, NetworkCanvas>, DatabaseIntermediateCanvasMapper>();
builder.Services.AddTransient<IMapper<NetworkCard, IntermediateCard>, DatabaseIntermediateCardMapper>();
builder.Services.AddTransient<IMapper<IntermediateCard, NetworkCard>, DatabaseIntermediateCardMapper>();
builder.Services.AddTransient<IMapper<NetworkParameter, IntermediateParameter>, DatabaseIntermediateParameterMapper>();
builder.Services.AddTransient<IMapper<IntermediateParameter, NetworkParameter>, DatabaseIntermediateParameterMapper>();

// Frontend mappers 
builder.Services.AddTransient<IMapper<IntermediateCanvas, StockCanvasDto>, IntermediateDtoCanvasMapper>();
builder.Services.AddTransient<IMapper<StockCanvasDto, IntermediateCanvas>, IntermediateDtoCanvasMapper>();
builder.Services.AddTransient<IMapper<IntermediateCard, StockCardDto>, IntermediateDtoCardMapper>();
builder.Services.AddTransient<IMapper<StockCardDto, IntermediateCard>, IntermediateDtoCardMapper>();
builder.Services.AddTransient<IMapper<IntermediateParameter, StockParameterDto>, IntermediateDtoParameterMapper>();
builder.Services.AddTransient<IMapper<StockParameterDto, IntermediateParameter>, IntermediateDtoParameterMapper>();

/*
 * ==================================
 *              Mappers
 * ==================================
 */
builder.Services.AddTransient<IMapper<StockDataEntryDto, StockDataEntry>, StockDataEntryMapper>();
builder.Services.AddTransient<IMapper<StockDataEntry, StockDataEntryDto>, StockDataEntryMapper>();
builder.Services.AddTransient<IMapper<StockSymbolDto, StockSymbol>, StockSymbolMapper>();
builder.Services.AddTransient<IMapper<StockSymbol, StockSymbolDto>, StockSymbolMapper>();
builder.Services.AddTransient<IMapper<NetworkCanvas, StockCanvasSimpleDto>, StockCanvasSimpleMapper>();
builder.Services.AddTransient<IMapper<StockCanvasSimpleDto, NetworkCanvas>, StockCanvasSimpleMapper>();

/*
 * ==================================
 *              Services
 * ==================================
 */
builder.Services.AddTransient<IServiceBase<StockSymbol, StockSymbolDto>, ServiceBase<StockSymbol, StockSymbolDto>>();
builder.Services.AddTransient<IServiceBase<NetworkCanvas, StockCanvasSimpleDto>, ServiceBase<NetworkCanvas, StockCanvasSimpleDto>>();
builder.Services.AddTransient<ICardCatalogService, CardCatalogService>();
builder.Services.AddTransient<IStockCanvasCompositeService, StockCanvasCompositeService>();
builder.Services.AddTransient<IBackTestService, BackTestService>();
builder.Services.AddTransient<INoSqlService<StockDataEntry, StockDataEntryDto>, StockDataEntryService>();

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
