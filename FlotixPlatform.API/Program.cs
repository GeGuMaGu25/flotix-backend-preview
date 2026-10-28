using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Generate the OpenAPI document with ASP.NET Core's built-in support.
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.

// OpenAPI and Scalar are kept available in all environments.
app.MapOpenApi();
app.MapScalarApiReference();
app.MapControllers();

app.Run();
