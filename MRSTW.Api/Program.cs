using System.Reflection;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MRSTW.Api.Services;
using MRSTW.Api.UnitOfWork;
using MRSTW.Api.Validators;
using MRSTW.BusinessLogicLayer.Common.Interfaces;
using MRSTW.BusinessLogicLayer.Mappings;
using MRSTW.BusinessLogicLayer.Services;
using MRSTW.BusinessLogicLayer.Validators;
using MRSTW.DataAccessLayer.Data;
using MRSTW.DataAccessLayer.Data.Repositories;
using MRSTW.DataAccessLayer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace MRSTW.Api;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        DotNetEnv.Env.Load();
        
        // JWT Authentication
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
        
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],

                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        string awsSecretAccessKey = DotNetEnv.Env.GetString("AWS_SECRET_ACCESS_KEY");
        string awsAccessKeyId = DotNetEnv.Env.GetString("AWS_ACCESS_KEY_ID");
        RegionEndpoint region = RegionEndpoint.USEast1;
        
        string? connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
                                   builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<AppDbContext>(options => { options.UseNpgsql(connectionString); });

        // TODO There was the problem
        // services.AddScoped<AppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<IDealRepository, DealRepository>();
        builder.Services.AddScoped<ISubcategoryRepository, SubcategoryRepository>();


        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


        builder.Services.AddScoped<IRabbitMqService, RabbitMqService>();

        builder.Services.AddValidatorsFromAssemblyContaining<DealValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<CategoryValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<SubcategoryValidator>();


        builder.Services.AddScoped<IBusinessUnitOfWork, BusinessUnitOfWork>();

        builder.Services.AddScoped<IDealService, DealService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<ISubcategoryService, SubcategoryService>();

        builder.Services.AddControllers();

        builder.Services.AddAutoMapper(typeof(CategoryMappingProfile), typeof(SubcategoryMappingProfile),
            typeof(DealMappingProfile));

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "MRSTW API", Version = "v1" });
            
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your valid token.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIs...\""
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
                    Array.Empty<string>()
                }
            });
        });


        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        builder.Services.AddValidatorsFromAssemblyContaining<CreateSubcategoryRequestValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryRequestValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateDealRequestValidator>();

        builder.Services.AddValidatorsFromAssemblyContaining<UpdateSubcategoryRequestValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UpdateCategoryRequestValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UpdateDealRequestValidator>();

        AWSCredentials awsCredentials = new BasicAWSCredentials(awsAccessKeyId, awsSecretAccessKey);
        
        AWSOptions awsOptions = new AWSOptions()
        {
            Credentials = awsCredentials,
            Region = region
        };

        builder.Services.AddAWSService<IAmazonS3>(awsOptions);

        builder.Services.AddScoped<AmazonS3Service, AmazonS3Service>();

        builder.Services.AddScoped<IApiUnitOfWork, ApiUnitOfWork>();

        builder.Services.AddScoped<IAuthService, AuthService>();

        builder.Services.AddEndpointsApiExplorer();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            await using var serviceScope = app.Services.CreateAsyncScope();

            await using var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.EnsureCreatedAsync();
            // await context.Database.MigrateAsync();
        }

        // if (app.Environment.IsDevelopment())
        // {
        //     await using var serviceScope = app.Services.CreateAsyncScope();
        //
        //     await using var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        //     // await context.Database.EnsureCreatedAsync();
        //     await context.Database.MigrateAsync();
        // }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllers();

        await app.RunAsync();
    }
}
