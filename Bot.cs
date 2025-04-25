using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WhatsTodo.Models;

namespace WhatsTodo;

public static class Bot
{
    private static readonly HttpClient _client = new();

    public static async Task SndMsg(string phoneNumber, string message)
    {
        if (AppSettings.ApiKey is null || AppSettings.MetaApiUriNumber is null || AppSettings.ApiKey is null)
            throw new Exception("Error on AppSettings, something is null");

        string url = $"https://graph.facebook.com/v21.0/{AppSettings.MetaApiUriNumber}/messages";
        
        _client.DefaultRequestHeaders.Clear();

        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AppSettings.ApiKey}");

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

            var response = await _client.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode is false)
            {
                Console.WriteLine("SEND NOK");
                throw new Exception(responseContent);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("SEND NOK");
            throw new Exception($"Send ERR {ex.Message}");
        }
    }
}
