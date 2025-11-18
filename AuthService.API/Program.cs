using System.Text.Json.Serialization;
using AuthService.API.Configuration;
using AuthService.API.Middlewares;
using AuthService.BO.Common;
using AuthService.DAO;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var dbSection = builder.Configuration.GetSection("Database");
var server = dbSection["Server"];
var port = dbSection["Port"];
var database = dbSection["DataName"];
var user = dbSection["UserId"];
var password = dbSection["Password"];

var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? server;
var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? port;
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? database;
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? user;
var dbPass = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? password;

var connectionString =
    $"Server={dbHost};Port={dbPort};Database={dbName};User Id={dbUser};Password={dbPass};SslMode=Required;";

builder.Services.AddDbContext<AuthServiceDbContext>(options =>
{
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 31)),
        mySqlOptions =>
            mySqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null
            )
    );
});

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddAppDI();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowExpoApp",
        policy =>
            policy
                //.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .SetIsOriginAllowed(_ => true)
    );
});

builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }


app.UseAppExceptionHandler();
app.UseCors("AllowExpoApp");
app.UseHttpsRedirection();
app.UseMiddleware<JwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
