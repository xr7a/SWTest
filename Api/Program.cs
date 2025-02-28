using Api.Middlewares;
using Application.Extensions;
using DataAccess.Extensions;
using Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDapperContext();
builder.Services.AddFluentMigrator(builder.Configuration);
builder.Services.AddScoped<ExceptionHandlingMiddleware>();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.Services.UseMigrations();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapSwagger();
    app.UseSwaggerUI();
}

app.Run();