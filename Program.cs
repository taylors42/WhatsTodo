
using Microsoft.EntityFrameworkCore;
using WhatsTodo.Models;

namespace WhatsTodo;

public static class Program
{
    public static async Task Main()
    {
        var builder = WebApplication.CreateBuilder();

        #region AppSettings

        AppSettings.ConnectionString = builder.Configuration["ConnectionString"];
        AppSettings.WebhookVerifyToken = builder.Configuration["WebhookVerifyToken"];
        AppSettings.ApiKey = builder.Configuration["ApiKey"];
        AppSettings.MetaApiUriNumber = builder.Configuration["MetaApiUriNumber"];

        #endregion

        if (AppSettings.ConnectionString is null)
            throw new Exception("Connection String is Null");

        builder.Services.AddDbContext<PaxDbContext>(options =>
            options
            .UseNpgsql(builder.Configuration
            .GetConnectionString(AppSettings.ConnectionString)));

        builder.Services.AddControllers();

        var app = builder.Build();

        NotificationSystem.Start();

        app.UseHttpsRedirection();

        app.MapControllers();

        app.Urls.Add("https://localhost:82");

        app.MapGet("/", () => Results.Ok(new { result = "API OK" }));

        app.Lifetime.ApplicationStopping.Register(NotificationSystem.Stop);

        await app.RunAsync();
    }
    public static async Task<DateTime> GBH()
    {
        using var client = new HttpClient();
        var result = await client.GetAsync("http://taylors42.com.br:3000/datetime");
        var responseBody = await result.Content.ReadAsStringAsync();
        return DateTime.Parse(responseBody).ToUniversalTime().AddHours(-3);
    }
}