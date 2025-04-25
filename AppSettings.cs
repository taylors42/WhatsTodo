using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace WhatsTodo;

public static class AppSettings
{
    [JsonIgnore]
    public static string? WebhookVerifyToken { get; set; }
    [JsonIgnore]
    public static string? ApiKey { get; set; }
    [JsonIgnore]
    public static string? MetaApiUriNumber { get; set; }
    [JsonIgnore]
    public static string? ConnectionString {  get; set; }
}
