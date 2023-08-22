using AuthService;
using AuthService.DataAccess;
using AuthService.EventBus;
using AuthService.Repositories;
using AuthService.Repositories.Abstractions;
using AuthService.Services;
using AuthService.Services.Abstractions;
using FluentValidation;
using MassTransit;
using MessagingHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddScoped<IJwtTokenHandler, JwtTokenHandler>();
builder.Services.AddScoped<IMemoryCachingHandler, MemoryCachingHandler>();

builder.Services.AddMemoryCache(config=>{});

builder.Services.AddMediatR(conf =>
{
    conf.RegisterServicesFromAssemblies(typeof(Program).Assembly);
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

builder.Services.AddDbContext<MSSQLDataAccess>(options =>
{  
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-server"));
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

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("RequireUserIsNotVerified", policy => policy.RequireClaim("mail-verified", "False"));
    option.AddPolicy("RequireUserIsVerifiedAndWithIdInClaims", policy => {
        policy.RequireClaim("id");
        policy.RequireClaim("mail-verified", "True");
    });
});

builder.Services.AddMassTransit(busConfigurator =>
{
    RabbitMqCredentials rabbit = new RabbitMqCredentials();
    busConfigurator.SetKebabCaseEndpointNameFormatter();
    busConfigurator.UsingRabbitMq((context, configuration) =>
    {
        configuration.ConfigureEndpoints(context);
      
        configuration.Host(new Uri(rabbit.Uri), h =>
        {
            h.Username(rabbit.UserName);
            h.Password(rabbit.Password);
        });
    });
});

builder.Services.AddAuthentication().AddJwtBearer(j =>
j.TokenValidationParameters = new TokenValidationParameters
{
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
        builder.Configuration.GetConnectionString("jwt-auth-secret-key"))),
    ValidAudience = "all",
    ValidIssuer = "auth_microservice"
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IEventBus, EventBus>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("ApiGatewayCORS");
app.MapControllers();

app.AddGlobalErrorHandler();

app.Run();
