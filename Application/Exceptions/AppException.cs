﻿namespace Application.Exceptions;

public class AppException(string message, int statusCode) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}