using Microsoft.EntityFrameworkCore;
using RoadDefect.Api.Data;
using Microsoft.OpenApi.Models;
using RoadDefect.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RoadDefect.Api.Helpers;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------------------
// 1. Add Services (MUST be before Build())
// ------------------------------------------------------

builder.Services.AddControllers();

// Register Application Services
builder.Services.AddScoped<IDefectService, DefectService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWorkOrderService, WorkOrderService>();
builder.Services.AddScoped<IDefectImageService, DefectImageService>();
builder.Services.AddHttpContextAccessor();

// Token generator (must be before Build)
builder.Services.AddScoped<JwtTokenGenerator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Road Defect API",
        Version = "v1"
    });

    // ?? ADD JWT SUPPORT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer YOUR_TOKEN"
    });

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
            new string[] {}
        }
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure EF Core
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? "Server=localhost;Database=RoadDefectDb;Trusted_Connection=True;TrustServerCertificate=True;";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
        };
    });

// ------------------------------------------------------
// 2. Build the App
// ------------------------------------------------------
var app = builder.Build();

// ------------------------------------------------------
// 3. Middleware Pipeline
// ------------------------------------------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Serve static files (images)
app.UseStaticFiles();

// Authentication BEFORE Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
