using MassTransit;
using MessagingHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrderService;
using OrderService.Consumers;
using OrderService.DataAccess;
using OrderService.EventBus;
using OrderService.Repositories;
using OrderService.Repositories.Abstractions;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<ProductCreatedConsumer>();
    busConfigurator.AddConsumer<ProductEditedConsumer>();
    busConfigurator.AddConsumer<ProductDeletedConsumer>();

    busConfigurator.AddConsumer<UserEditedConsumer>();
    busConfigurator.AddConsumer<UserCreatedConsumer>();
    busConfigurator.AddConsumer<UserDeletedConsumer>();
    busConfigurator.UsingRabbitMq((context, configuration) =>
    {
        RabbitMqCredentials rabbit = new RabbitMqCredentials();
        configuration.ConfigureEndpoints(context);
        configuration.Host(new Uri(rabbit.Uri), h =>
        {
            h.Username(rabbit.UserName);
            h.Password(rabbit.Password);
        });
    });
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

builder.Services.AddMediatR(conf =>
{
    conf.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddDbContext<MSSQLDataAccess>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-server"));
});
builder.Services.AddTransient<IEventBus, EventBus>();

builder.Services.AddAuthentication().AddJwtBearer(j =>
j.TokenValidationParameters = new TokenValidationParameters
{
    IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(builder.Configuration.GetConnectionString("jwt-auth-secret-key"))),
    ValidAudience = "all",
    ValidIssuer = "auth_microservice"
});

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("RequireUserHaveAdminRole", policy => policy.RequireClaim("acc-role", "Admin"));
    option.AddPolicy("RequireUserIsVerifiedAndWithIdInClaims", policy => {
        policy.RequireClaim("id");
        policy.RequireClaim("mail-verified", "True");
    });
});

builder.Services.AddHttpContextAccessor();

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
app.AddGlobalErrorHandler();
app.MapControllers();

app.Run();
