using GrowthTracking.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using System.Text;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using GrowthTracking.Repository.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add scoped services
builder.Services.AddScoped<IConsultationService, ConsultationService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
});
//JWWT config
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
   options.TokenValidationParameters = new TokenValidationParameters
   {
       ValidateIssuer = true,
       ValidateAudience = true,
       ValidateLifetime = true,
       ValidateIssuerSigningKey = true,
       ValidIssuer = builder.Configuration["Jwt:Issuer"],
       ValidAudience = builder.Configuration["Jwt:Audience"],
       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
   };
});

static IEdmModel GetEdmModel()
{
    var odataBuilder = new ODataConventionModelBuilder();
    odataBuilder.EntitySet<Consultation>("Consultation"); // EDM - ENTITY DATA MODEL
    return odataBuilder.GetEdmModel();
}
builder.Services.AddControllers().AddOData(options =>
{
    options.Select().Filter().OrderBy().Expand().SetMaxTop(null).Count();
    options.AddRouteComponents("odata", GetEdmModel());
});

builder.Services.AddSwaggerGen(option =>
{
    ////JWT Config
    option.DescribeAllParametersInCamelCase();
    option.ResolveConflictingActions(conf => conf.First());
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
