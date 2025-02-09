namespace WhatsTodo;

public class WhatsExceptions : Exception
{
    public WhatsExceptions() : base() { }
    
    public WhatsExceptions(string message) : base(message) { }
}