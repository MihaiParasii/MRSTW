using Notification.Api.Services;

namespace Notification.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddSingleton<IEmailService, EmailService>();
        builder.Services.AddScoped<INotification, EmailNotification>();
        builder.Services.AddSingleton<NotificationManager, NotificationManager>();
        builder.Services.AddHostedService<RabbitMqListener>();
        builder.Services.AddHttpClient();


        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        var service = app.Services.GetService<NotificationManager>();
        service!.Subscribe(app.Services.GetService<EmailNotification>()!);

        app.UseHttpsRedirection();


        app.Run();
    }
}
