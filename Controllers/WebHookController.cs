using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace WhatsTodo.Controllers;

[ApiController]
[Route("webhook")]
public class WebHookController : ControllerBase
{
    [HttpGet]
    public IActionResult Get(
        [FromQuery(Name = "hub.mode")] string mode,
        [FromQuery(Name = "hub.verify_token")] string token,
        [FromQuery(Name = "hub.challenge")] string challenge
    )
    {
        if (mode == "subscribe" && token == AppSettings.WebhookVerifyToken)
            return Ok(challenge);

        return StatusCode(403, "Forbidden");
    }

    [HttpPost]
    public async Task<IActionResult> Post()
    {
        using var reader = new StreamReader(Request.Body);
        var bodyContent = await reader.ReadToEndAsync();

        if (string.IsNullOrEmpty(bodyContent))
            return BadRequest("Body vazio");

        try
        {
            var data = JObject.Parse(bodyContent);

            if (data is null)
                return Ok(data);

            var entries = data["entry"]?.Children().ToList();

            if (entries is null)
                return Ok(data);

            foreach (var entry in entries)
            {
                var changes = entry["changes"]?.ToObject<JArray>() ?? new JArray();
                foreach (var change in changes)
                {
                    var value = change["value"] ?? new JObject();
                    var phoneNumberId = value["metadata"]?["phone_number_id"]?.ToString();
                    var messageData = value["messages"]?.ToObject<JArray>() ?? new JArray();

                    foreach (var message in messageData)
                    {
                        if (phoneNumberId is null)
                            continue;

                        if (message["from"]?.ToString() is null)
                            continue;

                        var type = message["type"]?.ToString();
                        var userNumber = message["from"]?.ToString();
                        var userMessage = message["text"]?["body"]?.ToString();

                        if (userMessage is null)
                            continue;

                        await Processor.Handler(new { User = userNumber, Text = userMessage });
                    }
                }
            }
            return Ok(data);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest($"Erro ao parsear JSON: {ex.Message}");
        }
    }
}
