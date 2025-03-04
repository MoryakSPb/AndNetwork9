﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using And9.Gateway.Clan.Auth;
using And9.Gateway.Clan.ConsumerStrategies;
using And9.Gateway.Clan.Hubs;
using And9.Gateway.Clan.Hubs.Model;
using And9.Gateway.Clan.Senders;
using And9.Gateway.Clan.Senders.Models;
using And9.Integration.Discord.Senders;
using And9.Lib.API;
using And9.Lib.Broker;
using And9.Service.Auth.Senders;
using And9.Service.Award.Senders;
using And9.Service.Core.Abstractions.Enums;
using And9.Service.Core.Senders;
using And9.Service.Election.Senders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Prometheus;
using StackExchange.Redis;

namespace And9.Gateway.Clan;

public class Startup
{
    public Startup(IConfiguration configuration) => Configuration = configuration;
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        AuthOptions.IssuerKey = Configuration["ISSUER_KEY"];

        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("and9.infra.redis"));

        services.AddTransient<IUserIdProvider, MemberIdProvider>();
        services.AddTransient<IAuthorizationHandler, ClanPolicyProvider>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = false,
                ValidIssuer = AuthOptions.ISSUER,
                ValidateAudience = false,
                ValidAudience = AuthOptions.AUDIENCE,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityIssuerKey(),
            };
            options.Events = new()
            {
                OnMessageReceived = context =>
                {
                    string accessToken = context.Request.Headers.Authorization.ToString().Split(' ', 2).Last();
                    if (string.IsNullOrEmpty(accessToken)) accessToken = context.Request.Query["access_token"];
                    if (string.IsNullOrEmpty(context.Token)) context.Token = accessToken;

                    if (!string.IsNullOrEmpty(context.Token))
                    {
                        JwtSecurityToken? token = new JwtSecurityTokenHandler().ReadJwtToken(context.Token);
                        context.HttpContext.User = new(new[] {new ClaimsIdentity(token.Claims)});
                    }

                    return Task.CompletedTask;
                },
            };
        });

        services.AddSignalR(options =>
        {
            options.EnableDetailedErrors = true;
            options.MaximumParallelInvocationsPerClient = 4;
        }).AddMessagePackProtocol();

        services.WithBroker(Configuration)
            .AppendConsumerWithoutResponse<RaiseElectionUpdateConsumerStrategy, int>()
            .AppendConsumerWithResponse<RaiseMemberUpdateConsumerStrategy, RaiseMemberUpdateArg, Rank>()
            .AddAuthSenders()
            .AddGatewaySenders()
            .AddAwardSenders()
            .AddCoreSenders()
            .AddElectionSenders()
            .AddDiscordSenders()
            .Build();

        services.AddHealthChecks()
            .AddRabbitMQ()
            .ForwardToPrometheus();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
        app.UseFileServer();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHttpMetrics();
        app.UseMetricServer();
        app.UseHealthChecks("/health");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<AuthHub>("hub/auth");
            endpoints.MapHub<CoreHub>("hub/core");
            endpoints.MapHub<DiscordHub>("hub/discord");
            endpoints.MapHub<ElectionHub>("hub/election");

            endpoints.MapHub<MemberHub>("hub/model/member");
            endpoints.MapHub<AwardHub>("hub/model/award");
            endpoints.MapHub<SquadHub>("hub/model/squad");
        });
    }
}