﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blog.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Query.TryGetValue(Configuration.ApiKeyName, out var extractApiKey))
        {
            context.Result = new ContentResult()
            {
                StatusCode = 401,
                Content = "Apikey not found!"
            };
            return;
        }

        if (!Configuration.ApiKey.Equals(extractApiKey))
        {
            context.Result = new ContentResult()
            {
                StatusCode = 403,
                Content = "Not allowed!"
            };
            return;
        }
        await next();
    }
}