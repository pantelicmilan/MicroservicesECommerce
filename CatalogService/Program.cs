using AuthService;
using CatalogService.Consumers;
using CatalogService.DataAccess;
using CatalogService.EventBus;
using CatalogService.Repositories;
using CatalogService.Repositories.Abstractions;
using MassTransit;
using MessagingHelper;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddMediatR(conf =>
{
    conf.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

builder.Services.AddDbContext<MSSQLDataAccess>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-server"));
});

builder.Services.Configure<FormOptions>(formOptions =>
{
    int maxSizeInMB = 50;
    long maxSizeInBytes = maxSizeInMB * 1024 * 1024;
    formOptions.MultipartBodyLengthLimit = maxSizeInBytes;
});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication().AddJwtBearer(j =>
j.TokenValidationParameters = new TokenValidationParameters
{
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
        builder.Configuration.GetConnectionString("jwt-auth-secret-key"))),
    ValidAudience = "all",
    ValidIssuer = "auth_microservice"
});

builder.Services.AddCors(options =>
{
    {
        options.AddPolicy("ApiGatewayCORS", builder =>
        {
            builder.WithOrigins("https://localhost:7278")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
    }
});

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("RequireUserHaveAdminRole", policy => policy.RequireClaim("acc-role", "Admin"));
    option.AddPolicy("RequireUserIsVerifiedAndWithIdInClaims", policy => {
        policy.RequireClaim("id");
        policy.RequireClaim("mail-verified", "True");
    });
});
builder.Services.AddTransient<IEventBus, EventBus>();

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.AddConsumer<OrderCreatedConsumer>();
    busConfigurator.SetKebabCaseEndpointNameFormatter();
    busConfigurator.UsingRabbitMq((context, configuration) =>
    {
        configuration.ConfigureEndpoints(context);
        RabbitMqCredentials rabbit = new RabbitMqCredentials();
        configuration.Host(new Uri(rabbit.Uri), h =>
        {
            h.Username(rabbit.UserName);
            h.Password(rabbit.Password);
        });
    });
});

var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
         Path.Combine(builder.Environment.ContentRootPath, "ImageStorage")),
    RequestPath = "/productImages" 
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("ApiGatewayCORS");
app.AddGlobalErrorHandler();

app.MapControllers();

app.Run();
