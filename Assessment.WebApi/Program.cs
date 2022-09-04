using Assessment.Data.Context;
using Assessment.Data.EFUnitOfWork;
using Assessment.Data.EFUnitOfWork.Interfaces;
using Assessment.Logic.DomainServices;
using Assessment.Logic.DomainServices.Interfaces;
using Assessment.Logic.Utilities;
using Assessment.Logic.Utilities.Interfaces;
using Assessment.Logic.ValidationServices;
using Assessment.Logic.ValidationServices.Interfaces;
using Assessment.WebApi.Filters;
using Assessment.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Limpiar mapeo de los tipos de los claims
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// Add services to the container
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ExceptionFilter));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "PRUEBA TÉCNICA",
        Version = "1.0.0",
        Description = "Documentación de la prueba técnica de la api.",
        Contact = new OpenApiContact
        {
            Name = "Francisco José Torreglosa Anaya",
            Email = "fjtorreglosa@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/franciscotorreglosa/")
        },
        License = new OpenApiLicense
        {
            Name = "Repository",
            Url = new Uri("https://github.com/fjtorreglosaa/AssessmentAranda")
        },
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
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
            new string[] { }
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"),
        b => b.MigrationsAssembly("Assessment.Data")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["key"])),
        ClockSkew = TimeSpan.Zero
    });

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("admin"));
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://www.apirequest.io").AllowAnyMethod().AllowAnyHeader()
        .WithExposedHeaders(new string[] { "numberOfRecords" });
    });
});

builder.Services.AddTransient<IEFUnitOfWork, EFUnitOfWork>();

#region Domain Services Injections

builder.Services.AddTransient<IProductDomainService, ProductDomainService> ();
builder.Services.AddTransient<IImageDomainService, ImageDomainService>();
builder.Services.AddTransient<ICategoryDomainService, CategoryDomainService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();

#endregion

#region Validation Services Injections

builder.Services.AddTransient<IProductValidationService, ProductValidationService>();
builder.Services.AddTransient <IImageValidationService, ImageValidationService> ();
builder.Services.AddTransient<ICategoryValidationService, CategoryValidationService>();

#endregion

builder.Services.AddTransient<ActionLogFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestExcecutionTimeMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
