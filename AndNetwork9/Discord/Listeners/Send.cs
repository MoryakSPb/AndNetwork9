﻿using System;
using System.Net;
using AndNetwork9.Shared.Backend.Rabbit;
using AndNetwork9.Shared.Backend.Senders.Discord;
using Discord;
using Discord.Rest;
using IConnection = RabbitMQ.Client.IConnection;

namespace AndNetwork9.Discord.Listeners
{
    public class Send : BaseRabbitListenerWithoutResponse<SendArg>
    {
        private readonly DiscordBot _bot;

        public Send(IConnection connection, DiscordBot bot) : base(connection, SendSender.QUEUE_NAME)
        {
            _bot = bot;
        }

        public override async void Run(SendArg arg)
        {
            RestUser? user = await _bot.Rest.GetUserAsync(arg.DiscordId);
            if (user is null) throw new FailedCallException(HttpStatusCode.NotFound);
            try
            {
                await user.SendMessageAsync(arg.Message);
            }
            catch (Exception e)
            {
                throw new FailedCallException(HttpStatusCode.Locked)
                {
                    Description = e.Message,
                };
            }
        }
    }
}