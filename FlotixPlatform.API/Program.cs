using System.Text;
using System.Text.Json.Serialization;
using FlotixPlatform.API.Analytics.Application.Internal.CommandServices;
using FlotixPlatform.API.Analytics.Application.Internal.QueryServices;
using FlotixPlatform.API.Analytics.Application.Services;
using FlotixPlatform.API.Analytics.Domain.Repositories;
using FlotixPlatform.API.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FlotixPlatform.API.Commerce.Application.Internal.CommandServices;
using FlotixPlatform.API.Commerce.Application.Internal.QueryServices;
using FlotixPlatform.API.Commerce.Application.Services;
using FlotixPlatform.API.Commerce.Domain.Repositories;
using FlotixPlatform.API.Commerce.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FlotixPlatform.API.Fleet.Application.Internal.CommandServices;
using FlotixPlatform.API.Fleet.Application.Internal.QueryServices;
using FlotixPlatform.API.Fleet.Application.Services;
using FlotixPlatform.API.Fleet.Domain.Repositories;
using FlotixPlatform.API.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FlotixPlatform.API.Fuel.Application.Internal.CommandServices;
using FlotixPlatform.API.Fuel.Application.Internal.QueryServices;
using FlotixPlatform.API.Fuel.Application.Services;
using FlotixPlatform.API.Fuel.Domain.Repositories;
using FlotixPlatform.API.Fuel.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FlotixPlatform.API.IAM.Application.Internal.CommandServices;
using FlotixPlatform.API.IAM.Application.Internal.OutboundServices;
using FlotixPlatform.API.IAM.Application.Internal.QueryServices;
using FlotixPlatform.API.IAM.Application.Services;
using FlotixPlatform.API.IAM.Domain.Repositories;
using FlotixPlatform.API.IAM.Infrastructure.Hashing.AspNetCore.Services;
using FlotixPlatform.API.IAM.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FlotixPlatform.API.IAM.Infrastructure.Pipeline.Middleware.Extensions;
using FlotixPlatform.API.IAM.Infrastructure.Tokens.JWT.Configuration;
using FlotixPlatform.API.IAM.Infrastructure.Tokens.JWT.Services;
using FlotixPlatform.API.Incidents.Application.Internal.CommandServices;
using FlotixPlatform.API.Incidents.Application.Internal.QueryServices;
using FlotixPlatform.API.Incidents.Application.Services;
using FlotixPlatform.API.Incidents.Domain.Repositories;
using FlotixPlatform.API.Incidents.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FlotixPlatform.API.Maintenance.Application.Internal.CommandServices;
using FlotixPlatform.API.Maintenance.Application.Internal.QueryServices;
using FlotixPlatform.API.Maintenance.Application.Services;
using FlotixPlatform.API.Maintenance.Domain.Repositories;
using FlotixPlatform.API.Maintenance.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FlotixPlatform.API.Monitoring.Application.Internal.CommandServices;
using FlotixPlatform.API.Monitoring.Application.Internal.OutboundServices;
using FlotixPlatform.API.Monitoring.Application.Internal.QueryServices;
using FlotixPlatform.API.Monitoring.Application.Services;
using FlotixPlatform.API.Monitoring.Domain.Repositories;
using FlotixPlatform.API.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FlotixPlatform.API.Monitoring.Infrastructure.Processing;
using FlotixPlatform.API.Monitoring.Infrastructure.Queues.Channels;
using FlotixPlatform.API.Monitoring.Infrastructure.Realtime.SignalR.Services;
using FlotixPlatform.API.Monitoring.Interfaces.Hubs;
using FlotixPlatform.API.Notifications.Application.Internal.CommandServices;
using FlotixPlatform.API.Notifications.Application.Internal.QueryServices;
using FlotixPlatform.API.Notifications.Application.Services;
using FlotixPlatform.API.Notifications.Domain.Repositories;
using FlotixPlatform.API.Notifications.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using FlotixPlatform.API.Notifications.Interfaces.Hubs;
using FlotixPlatform.API.Resources;
using FlotixPlatform.API.Shared.Domain.Repositories;
using FlotixPlatform.API.Shared.Infrastructure.Interfaces.Asp.Configuration;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Scalar.AspNetCore;
using IAMLocalizedOpenApiAssembler = FlotixPlatform.API.IAM.Interfaces.Rest.Transform.LocalizedOpenApiAssembler;
using FleetLocalizedOpenApiAssembler = FlotixPlatform.API.Fleet.Interfaces.Rest.Transform.LocalizedOpenApiAssembler;
using FuelLocalizedOpenApiAssembler = FlotixPlatform.API.Fuel.Interfaces.Rest.Transform.LocalizedOpenApiAssembler;
using MaintenanceLocalizedOpenApiAssembler = FlotixPlatform.API.Maintenance.Interfaces.Rest.Transform.LocalizedOpenApiAssembler;
using IncidentsLocalizedOpenApiAssembler = FlotixPlatform.API.Incidents.Interfaces.Rest.Transform.LocalizedOpenApiAssembler;
using MonitoringLocalizedOpenApiAssembler = FlotixPlatform.API.Monitoring.Interfaces.Rest.Transform.LocalizedOpenApiAssembler;
using NotificationsLocalizedOpenApiAssembler = FlotixPlatform.API.Notifications.Interfaces.Rest.Transform.LocalizedOpenApiAssembler;
using CommerceLocalizedOpenApiAssembler = FlotixPlatform.API.Commerce.Interfaces.Rest.Transform.LocalizedOpenApiAssembler;
using AnalyticsLocalizedOpenApiAssembler = FlotixPlatform.API.Analytics.Interfaces.Rest.Transform.LocalizedOpenApiAssembler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure Lower Case URLs
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Localization Configuration
builder.Services.AddLocalization();

// Configure Kebab Case Route Naming Convention
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .AddDataAnnotationsLocalization(options =>
        options.DataAnnotationLocalizerProvider = (_, factory) => factory.Create(typeof(SharedResource)))
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var localizer = context.HttpContext.RequestServices
                .GetRequiredService<IStringLocalizer<SharedResource>>();
            var problemDetailsFactory = context.HttpContext.RequestServices
                .GetRequiredService<ProblemDetailsFactory>();
            var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                context.HttpContext,
                context.ModelState,
                statusCode: StatusCodes.Status400BadRequest,
                title: localizer["InvalidModelStateTitle"].Value);

            var result = new BadRequestObjectResult(problemDetails);
            result.ContentTypes.Add("application/problem+json");
            return result;
        };
    });

// Register RFC 7807 ProblemDetails payloads for centralized exception handling.
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        if (context.ProblemDetails.Status is null or >= 500)
        {
            var localizer = context.HttpContext.RequestServices.GetRequiredService<IStringLocalizer<SharedResource>>();
            context.ProblemDetails.Title ??= localizer["UnexpectedServerError"].Value;
            context.ProblemDetails.Detail ??= localizer["UnexpectedErrorProcessingRequest"].Value;
        }
    };
});

// Serialize enums by name (also used by the OpenAPI document generator).
builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Generate the OpenAPI document with ASP.NET Core's built-in support.
builder.Services.AddOpenApi(options =>
{
    options.AddOperationTransformer(IAMLocalizedOpenApiAssembler.TransformOperationAsync);
    options.AddSchemaTransformer(IAMLocalizedOpenApiAssembler.TransformSchemaAsync);
    options.AddOperationTransformer(FleetLocalizedOpenApiAssembler.TransformOperationAsync);
    options.AddSchemaTransformer(FleetLocalizedOpenApiAssembler.TransformSchemaAsync);
    options.AddOperationTransformer(FuelLocalizedOpenApiAssembler.TransformOperationAsync);
    options.AddSchemaTransformer(FuelLocalizedOpenApiAssembler.TransformSchemaAsync);
    options.AddOperationTransformer(MaintenanceLocalizedOpenApiAssembler.TransformOperationAsync);
    options.AddSchemaTransformer(MaintenanceLocalizedOpenApiAssembler.TransformSchemaAsync);
    options.AddOperationTransformer(IncidentsLocalizedOpenApiAssembler.TransformOperationAsync);
    options.AddSchemaTransformer(IncidentsLocalizedOpenApiAssembler.TransformSchemaAsync);
    options.AddOperationTransformer(MonitoringLocalizedOpenApiAssembler.TransformOperationAsync);
    options.AddSchemaTransformer(MonitoringLocalizedOpenApiAssembler.TransformSchemaAsync);
    options.AddOperationTransformer(NotificationsLocalizedOpenApiAssembler.TransformOperationAsync);
    options.AddSchemaTransformer(NotificationsLocalizedOpenApiAssembler.TransformSchemaAsync);
    options.AddOperationTransformer(CommerceLocalizedOpenApiAssembler.TransformOperationAsync);
    options.AddSchemaTransformer(CommerceLocalizedOpenApiAssembler.TransformSchemaAsync);
    options.AddOperationTransformer(AnalyticsLocalizedOpenApiAssembler.TransformOperationAsync);
    options.AddSchemaTransformer(AnalyticsLocalizedOpenApiAssembler.TransformSchemaAsync);
});

// Real-time communication with SignalR.
builder.Services.AddSignalR()
    .AddJsonProtocol(options => options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Allow the configured front-end origins (credentials are required by SignalR).
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy
    .WithOrigins(builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? [])
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()));

// Add Database Connection

// Configure Database Context and route EF logs through the app logger pipeline.
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(connectionStringTemplate))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");

    var connectionString = Environment.ExpandEnvironmentVariables(connectionStringTemplate);
    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");

    options.UseNpgsql(connectionString, npgsql => npgsql.ConfigureDataSource(dataSource => dataSource.EnableDynamicJson()))
        .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
        .EnableDetailedErrors();

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

// Configure Dependency Injection

// Shared Bounded Context Injection Configuration
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Identity, Profiles & Security Bounded Context Injection Configuration
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Vehicle & Fleet Management Bounded Context Injection Configuration
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleCommandService, VehicleCommandService>();
builder.Services.AddScoped<IVehicleQueryService, VehicleQueryService>();

// Fuel Control Bounded Context Injection Configuration
builder.Services.AddScoped<IFuelRecordRepository, FuelRecordRepository>();
builder.Services.AddScoped<IFuelRecordCommandService, FuelRecordCommandService>();
builder.Services.AddScoped<IFuelRecordQueryService, FuelRecordQueryService>();

// Maintenance Management Bounded Context Injection Configuration
builder.Services.AddScoped<IMaintenanceRequestRepository, MaintenanceRequestRepository>();
builder.Services.AddScoped<IMaintenanceRequestCommandService, MaintenanceRequestCommandService>();
builder.Services.AddScoped<IMaintenanceRequestQueryService, MaintenanceRequestQueryService>();

// Incident Management Bounded Context Injection Configuration
builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();
builder.Services.AddScoped<IIncidentCommandService, IncidentCommandService>();
builder.Services.AddScoped<IIncidentQueryService, IncidentQueryService>();

// Real-Time Monitoring / Fleet Tracking Bounded Context Injection Configuration
builder.Services.AddScoped<IIotDeviceRepository, IotDeviceRepository>();
builder.Services.AddScoped<IIotDeviceCommandService, IotDeviceCommandService>();
builder.Services.AddScoped<IIotDeviceQueryService, IotDeviceQueryService>();
builder.Services.AddScoped<ITelemetryDataRepository, TelemetryDataRepository>();
builder.Services.AddScoped<ITelemetryDataCommandService, TelemetryDataCommandService>();
builder.Services.AddScoped<ITelemetryDataQueryService, TelemetryDataQueryService>();
builder.Services.AddSingleton<ITelemetryDataQueue, TelemetryDataQueue>();
builder.Services.AddScoped<IVehicleLocationNotifier, VehicleLocationNotifier>();
builder.Services.AddHostedService<TelemetryDataProcessor>();

// Alerts & Notifications Bounded Context Injection Configuration
builder.Services.AddScoped<IAlertRepository, AlertRepository>();
builder.Services.AddScoped<IAlertCommandService, AlertCommandService>();
builder.Services.AddScoped<IAlertQueryService, AlertQueryService>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationCommandService, NotificationCommandService>();
builder.Services.AddScoped<INotificationQueryService, NotificationQueryService>();

// Digital Experience / IoT Commerce Bounded Context Injection Configuration
builder.Services.AddScoped<IIotOrderRepository, IotOrderRepository>();
builder.Services.AddScoped<IIotOrderCommandService, IotOrderCommandService>();
builder.Services.AddScoped<IIotOrderQueryService, IotOrderQueryService>();

// Reporting & Analytics Bounded Context Injection Configuration
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IReportCommandService, ReportCommandService>();
builder.Services.AddScoped<IReportQueryService, ReportQueryService>();

// IAM JWT settings (the secret comes from TokenSettings__Secret in production and must have at least 32 bytes).
builder.Services.AddOptions<TokenSettings>()
    .Bind(builder.Configuration.GetSection(nameof(TokenSettings)))
    .Validate(settings => Encoding.UTF8.GetByteCount(settings.Secret) >= 32, "The JWT secret must have at least 32 bytes.")
    .ValidateOnStart();

var app = builder.Build();

// Apply pending migrations on startup (safe to call even when schema is up to date)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

// Localization must run before OpenAPI so generated documentation follows the request culture.
string[] supportedCultures = ["en", "en-US", "es", "es-PE"];
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
localizationOptions.ApplyCurrentCultureToResponseHeaders = true;
app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();

app.UseCors();

// IAM request authorization (disconnected): uncomment to require a bearer token from
// POST /api/v1/authentication/sign-in on every endpoint not marked [AllowAnonymous],
// and to enforce [Authorize(Role...)]. SignalR hubs read the token from ?access_token=.
// app.UseRequestAuthorization();

app.UseAuthorization();

// OpenAPI and Scalar are kept available in all environments to preserve the existing API explorer behavior.
app.MapOpenApi().AllowAnonymous();
app.MapScalarApiReference().AllowAnonymous();
app.MapControllers();
app.MapHub<MonitoringHub>("/hubs/monitoring");
app.MapHub<NotificationsHub>("/hubs/notifications");

app.Run();
