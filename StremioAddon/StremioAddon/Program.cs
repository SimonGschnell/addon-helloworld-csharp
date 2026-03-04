using dotenv.net;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddCors();

var app = builder.Build();

app.UseCors(b => b
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
);

app.MapControllers();
app.Run();
