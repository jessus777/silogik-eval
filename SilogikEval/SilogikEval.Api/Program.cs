using SilogikEval.Api.Extensions;
using SilogikEval.Application;
using SilogikEval.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddOpenApi();
builder.Services.AddAppCors();
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAppCors();
app.UseErrorLocalization();

// Endpoints
app.MapApiEndpoints();

app.Run();
