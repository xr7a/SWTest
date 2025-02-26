using DataAccess.Extensions;
using Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDapperContext();
builder.Services.AddFluentMigrator(builder.Configuration);
var app = builder.Build();

app.Services.UseMigrations();
app.Run();

