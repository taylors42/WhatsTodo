using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WhatsTodo;

public static class Bot
{
    private static readonly HttpClient Client = new();
    

    public static async Task SendMessageTextAsync(string phoneNumber, string message)
    {
        if (AppSettings.ApiKey is null || AppSettings.MetaApiUriNumber is null || AppSettings.ApiKey is null)
            throw new WhatsExceptions("Error on AppSettings, something is null");

        string url = $"https://graph.facebook.com/v21.0/{AppSettings.MetaApiUriNumber}/messages";
        Client.DefaultRequestHeaders.Clear();
        Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AppSettings.ApiKey}");
        phoneNumber = FormatBrazilianPhoneNumber(phoneNumber);
        var payload = new
        {
            messaging_product = "whatsapp",
            to = phoneNumber,
            type = "text",
            text = new { body = $"{message}" },
        };

        try
        {
            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            var response = await Client.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("SEND NOK");
                throw new WhatsExceptions(responseContent);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("SEND NOK");
            throw new WhatsExceptions($"Send ERR {ex.Message}");
        }
    }

    private static string FormatBrazilianPhoneNumber(string phoneNumber)
    {
        string cleanNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());

        bool startsWith55 = cleanNumber.StartsWith("55");

        if (startsWith55)
        {
            cleanNumber = cleanNumber.Substring(2);
        }

        string ddd = cleanNumber.Substring(0, 2);

        string number = cleanNumber.Substring(2);

        if (number.Length == 8)
        {
            number = "9" + number;
        }

        return "55" + ddd + number;
    }
}

