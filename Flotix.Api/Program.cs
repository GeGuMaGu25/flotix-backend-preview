using System.Text.Json.Serialization;
using Flotix.Api.Iam.Domain;
using Flotix.Api.Iam.Security;
using Flotix.Api.Shared.Domain;
using Flotix.Api.Shared.Persistence;
using Flotix.Api.Shared.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers(options => options.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseParameterTransformer())))
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.RespectNullableAnnotations = true;
        options.JsonSerializerOptions.RespectRequiredConstructorParameters = true;
    });
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ProblemExceptionHandler>();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy
    .WithOrigins(builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? [])
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()));

// Persistence: EF Core on PostgreSQL behind the Repository and Unit of Work abstractions.
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("Flotix"),
    npgsql => npgsql.ConfigureDataSource(dataSource => dataSource.EnableDynamicJson())));
builder.Services.AddScoped<IUnitOfWork>(services => services.GetRequiredService<AppDbContext>());
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// IAM: password hashing and bearer tokens with ASP.NET Core built-ins (no third-party dependencies).
builder.Services.AddDataProtection();
builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddSingleton<ITokenService, DataProtectionTokenService>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseCors();

// IAM (disconnected): uncomment to require a bearer token from POST /api/v1/authentication/sign-in on every
// endpoint not marked [AllowAnonymous], and to enforce [AllowRoles]. SignalR hubs read it from ?access_token=.
// app.UseMiddleware<IamMiddleware>();

app.MapOpenApi().AllowAnonymous();
app.MapScalarApiReference().AllowAnonymous();
app.MapControllers();

app.Run();
