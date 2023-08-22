using MassTransit;
using MessagingHelper;
using NotificationsService.Consumers;
using NotificationsService.Services;
using NotificationsService.Services.Abstractions;
using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.AddConsumer<VerificationCodeConsumer>();
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

builder.Services.AddScoped<INotificationsSender, NotificationSender>();

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
