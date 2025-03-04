﻿using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace And9.Integration.Steam.HealthChecks;

public class SteamHealthCheck : IHealthCheck
{
    private readonly HttpClient _httpClient;
    public SteamHealthCheck(IHttpClientFactory httpClientFactory) => _httpClient = httpClientFactory.CreateClient("SteamApi");

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            HttpResponseMessage result = await _httpClient
                .SendAsync(
                    new(HttpMethod.Get, "ISteamNews/GetNewsForApp/v0002/?appid=244850&count=0"),
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken).ConfigureAwait(false);
            return result.IsSuccessStatusCode ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy(result.StatusCode.ToString("G"));
        }
        catch
        {
            return HealthCheckResult.Unhealthy();
        }
    }
}