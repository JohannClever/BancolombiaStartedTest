using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using BancolombiaStarter.Backend.Infrastructure.Authorization.Entities;
using BancolombiaStarter.Backend.Infrastructure.DataAccess;
using BancolombiaStarter.Backend.Infrastructure.DataAccess.SeedData;
using BancolombiaStarter.Backend.Infrastructure.Extensions;
using BancolombiaStarter.Backend.Infrastructure.Mapper;
using System.Text;
using BancolombiaStarter.Backend.Domain.Services;
using BancolombiaStarter.Backend.Domain.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var config = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bancolombia Starter API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter your JWT token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer", // Use "bearer" for JWT
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
       {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

var connectionString = config.GetConnectionString("database");
builder.Services.AddDbContext<PersistenceContext>(opt =>
{
    opt.UseSqlServer(connectionString, sqlopts =>
    {
        sqlopts.MigrationsHistoryTable("_MigrationHistory", config.GetValue<string>("SchemaName"));
    });
});


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<PersistenceContext>()
    .AddDefaultTokenProviders();


var issuer = config.GetValue<string>("Jwt:Issuer");
var audience = config.GetValue<string>("Jwt:Audience");
var key = config.GetValue<string>("Jwt:SecretKey");

var tokenValidationParams = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = false,
    RequireExpirationTime = false,
    ClockSkew = TimeSpan.Zero
};

builder.Services.AddSingleton(tokenValidationParams);

builder.Services
.AddHttpContextAccessor()
.AddAuthorization()
.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = tokenValidationParams;
});

builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);
builder.Services
       .AddPersistence(config)
       .AddJwtServices()
       .AddDomainServices();
builder.Services.AddHttpClient<IKickstarterService,KickstarterService>();
builder.Services.AddSingleton<IOpenAiService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var apiKey = configuration["OpenAI:ApiKey"];
    return new OpenAiService(apiKey);
});


using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PersistenceContext>();
    if (dbContext.Database.IsSqlServer())
    {
        if (dbContext.Database.GetPendingMigrations().Any())
            dbContext.Database.Migrate();

        await IdentitySeedData.Initialize(scope.ServiceProvider);
        await DbSeeder.SeedData(dbContext, scope.ServiceProvider);

        dbContext.CheckSpHasBeenCreated("Sp_Insert_Comment");
    }

}


var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bancolombia Starter API V1");
});


app.UseHttpsRedirection();


app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAllOrigins");

app.Run();

