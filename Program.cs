using WhatsTodo.Database;

namespace WhatsTodo;

public static class Program
{
    public static async Task Main(string[] args)
    {
        if (!Database.Database.CreateDb())
            throw new WhatsExceptions("Error creating database");

        var builder = WebApplication.CreateBuilder(args);

        AppSettings.WebhookVerifyToken = builder.Configuration["WebhookVerifyToken"];
        AppSettings.ApiKey = builder.Configuration["ApiKey"];
        AppSettings.MetaApiUriNumber = builder.Configuration["MetaApiUriNumber"];

        builder.Services.AddControllers();
        var app = builder.Build();

        NotificationSystem.Start();

        app.UseHttpsRedirection();
        app.MapControllers();
        app.Urls.Add("https://localhost:82");
        app.MapGet("/", () => Results.Ok(new { result = "API OK" }));

        app.Lifetime.ApplicationStopping.Register(() => 
        {
            NotificationSystem.Stop();
        });

        await app.RunAsync();
    }
}