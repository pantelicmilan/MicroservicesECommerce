namespace MessagingHelper;

public readonly struct RabbitMqCredentials
{
    public readonly string UserName = "guest";
    public readonly string Password = "guest";
    public readonly string Uri = "amqp://localhost:5672/";
    public RabbitMqCredentials(){}
}
