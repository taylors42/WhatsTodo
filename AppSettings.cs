using System.Diagnostics.CodeAnalysis;

namespace WhatsTodo;

public static class AppSettings
{
    public static string? WebhookVerifyToken { get; set; }
    public static string? ApiKey { get; set; }
    public static string? MetaApiUriNumber { get; set; }
    public static string? ConnectionString {  get; set; }
}
