using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;

namespace WhatsTodo;

public static class Bot
{
    private static readonly HttpClient Client = new();
    
    public static void SendMessageTextAsync(string phoneNumber, string message)
    {

        string url = $"https://graph.facebook.com/v21.0/{AppSettings.MetaApiUriNumber}/messages";
        Client.DefaultRequestHeaders.Clear();
        Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AppSettings.ApiKey}");

        var payload = new
        {
            messaging_product = "whatsapp",
            to = phoneNumber,
            type = "text",
            text = new
            {
                body = $"{message}"
            }
        };

        try
        {
            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            var response = Client.PostAsync(url, content).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(responseContent);
                throw new WhatsExceptions(responseContent);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw new  WhatsExceptions($"Send ERR {ex.Message}");
        }
    }

}

