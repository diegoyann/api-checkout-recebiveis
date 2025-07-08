using Microsoft.EntityFrameworkCore;
using Size.Application.Interfaces;
using Size.Application.Services;
using Size.Application.Converters;
using Size.Domain.Interfaces;
using Size.Domain.Interfaces.Repositories;
using Size.Infrastructure.Data;
using Size.Infrastructure.Repositories;
using SizeApi.Application.Interfaces;
using Size.Api;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:5000");

builder.Services.AddDbContext<SizeContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        // Configuração para falhas transitórias
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
        
        sqlOptions.CommandTimeout(60);
    });
    
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

builder.Services.AddScoped<IEmpresaRepo, EmpresaRepo>();
builder.Services.AddScoped<INotaFiscalRepo, NotaFiscalRepo>();
builder.Services.AddScoped<ICarrinhoAntecipacaoRepo, CarrinhoAntecipacaoRepo>();

builder.Services.AddScoped<IServicoEmpresa, ServicoEmpresa>();
builder.Services.AddScoped<IServicoNotaFiscal, ServicoNotaFiscal>();
builder.Services.AddScoped<IServicoCarrinho, ServicoCarrinho>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DecimalJsonConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = null; 
        options.JsonSerializerOptions.WriteIndented = true; 
    });

builder.Services.AddHealthChecks()
    .AddDbContextCheck<SizeContext>("database", tags: new[] { "db", "sql" });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swaggerGenOptions =>
{
    swaggerGenOptions.SwaggerDoc("v1", new() { 
        Title = "Size API", 
        Version = "v1.0",
        Description = "API para Sistema de Antecipação de Recebíveis",
        Contact = new()
        {
            Name = "Size API - Diego Yann"
        }
    });
    
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        swaggerGenOptions.IncludeXmlComments(xmlPath);
    }
    
    swaggerGenOptions.TagActionsBy(api => new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"] });
    swaggerGenOptions.DocInclusionPredicate((name, api) => true);
    
    swaggerGenOptions.EnableAnnotations();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(swaggerUiOptions =>
{
    swaggerUiOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "Size API v1.0");
    swaggerUiOptions.RoutePrefix = string.Empty; 
    swaggerUiOptions.DocumentTitle = "Size API - Sistema de Antecipação de Recebíveis";
    swaggerUiOptions.DefaultModelsExpandDepth(-1); // Oculta os modelos por padrão
    swaggerUiOptions.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List); // Mostra apenas a lista de endpoints
    swaggerUiOptions.EnableDeepLinking();
    swaggerUiOptions.DisplayOperationId();
});

await ConfigurarBancoDadosAsync(app);

static async Task ConfigurarBancoDadosAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<SizeContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var environment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
    
    const int maxRetries = 30;
    var delay = TimeSpan.FromSeconds(2);
    
    for (int i = 0; i < maxRetries; i++)
    {
        try
        {
            logger.LogInformation("Tentativa {Attempt}/{MaxRetries} - Conectando ao banco de dados...", i + 1, maxRetries);
            
            await context.Database.CanConnectAsync();
            
            if (context.Database.GetPendingMigrations().Any())
            {
                logger.LogInformation("Aplicando migrations pendentes...");
                await context.Database.MigrateAsync();
            }
            else
            {
                await context.Database.EnsureCreatedAsync();
            }
            
            logger.LogInformation("Banco de dados configurado com sucesso!");
            
            if (environment.IsDevelopment())
            {
                var connectionString = context.Database.GetConnectionString();
                logger.LogInformation("String de conexão: {ConnectionString}", connectionString?.Replace("Password=", "Password=***"));
            }
            
            break;
        }
        catch (Exception ex)
        {
            logger.LogWarning("Erro ao conectar com o banco (tentativa {Attempt}/{MaxRetries}): {Error}", i + 1, maxRetries, ex.Message);
            
            if (i == maxRetries - 1)
            {
                logger.LogError("Falha ao conectar com o banco após {MaxRetries} tentativas. Detalhes: {ExceptionDetails}", maxRetries, ex);
                throw;
            }
            
            await Task.Delay(delay);
        }
    }
}

app.UseAuthorization();

// Configurar Health Checks endpoints
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(x => new
            {
                name = x.Key,
                status = x.Value.Status.ToString(),
                exception = x.Value.Exception?.Message,
                duration = x.Value.Duration.ToString()
            }),
            duration = report.TotalDuration.ToString()
        };
        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
    }
});

app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});

app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => false
});

app.MapControllers();

app.MapFallback(() => Results.Redirect("/"));



app.Run();