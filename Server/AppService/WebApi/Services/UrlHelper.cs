﻿using Catalog.Infrastructure;

namespace Catalog.WebApi.Services;

public class UrlHelper : IUrlHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UrlHelper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetHostUrl()
    {
        var request = _httpContextAccessor.HttpContext!.Request;

        string host = $"{request.Scheme}://{request.Host}";

        return $"{host}";
    }

    public string? CreateImageUrl(string? id)
    {
        if (id is null) return null;

        var host = GetHostUrl();

        int port = 8080;

        return $"{host}:{port}/content/images/{id}";
    }
}