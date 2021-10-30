﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AndNetwork9.Shared;
using AndNetwork9.Shared.Backend;
using AndNetwork9.Shared.Backend.Senders.Discord;
using AndNetwork9.Shared.Enums;
using AndNetwork9.Shared.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Task = System.Threading.Tasks.Task;

namespace AndNetwork9.AwardDispenser.Services;

public class RiseDispenser : ITimerService
{
    private readonly ILogger<RiseDispenser> _logger;
    private readonly PublishSender _publishSender;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly UpdateUserSender _updateUserSender;

    public RiseDispenser(IServiceScopeFactory scopeFactory, IConfiguration configuration,
        PublishSender publishSender, ILogger<RiseDispenser> logger, UpdateUserSender updateUserSender)
    {
        _scopeFactory = scopeFactory;
        _publishSender = publishSender;
        _logger = logger;
        _updateUserSender = updateUserSender;
        Interval = TimeSpan.Parse(configuration["RISE_DISPENSER_INTERVAL"]);
    }

    protected TimeSpan Interval { get; init; }

    CancellationTokenSource? ITimerService.CancellationTokenSource { get; set; }
    TimeSpan ITimerService.Interval => Interval;

    PeriodicTimer? ITimerService.Timer { get; set; }

    public async Task Process()
    {
        _logger.LogInformation("Triggering " + nameof(RiseDispenser));
        AsyncServiceScope serviceScope = _scopeFactory.CreateAsyncScope();
        await using ConfiguredAsyncDisposable _ = serviceScope.ConfigureAwait(false);
        ClanDataContext data = serviceScope.ServiceProvider.GetRequiredService<ClanDataContext>();
        ;
        Member[] members = data.Members.Where(x => x.Rank > Rank.None && x.Rank < Rank.Advisor).ToArray();
        Dictionary<Member, (Rank oldRank, Rank newRank)> changes = new();
        foreach (Member member in members)
        {
            Rank actualRank = member.Awards.GetRank();
            if (member.Rank == actualRank) continue;
            changes.Add(member, (member.Rank, actualRank));
            member.Rank = actualRank;
        }

        if (!changes.Any()) return;
        await data.SaveChangesAsync();
        StringBuilder text = new(256);
        foreach ((Member member, (Rank oldRank, Rank newRank)) in changes.OrderByDescending(x => x.Key))
        {
            string nickname = (member.DiscordId.HasValue ? member.GetDiscordMention() : member.Nickname)!;
            text.AppendLine(
                oldRank > newRank
                    ? $"📉: Игрок {nickname} разжалован в ранг «{newRank.GetRankName()}»"
                    : $"📈: Игроку {nickname} присвоен ранг «{newRank.GetRankName()}»");
            if (member.DiscordId.HasValue) await _updateUserSender.CallAsync(member.DiscordId!.Value);
        }

        if (text.Length > 0) await _publishSender.CallAsync(text.ToString());
        _logger.LogInformation("Triggered "
                               + nameof(RiseDispenser)
                               + Environment.NewLine
                               + $"Interval = {Interval.TotalSeconds}s");
    }
}