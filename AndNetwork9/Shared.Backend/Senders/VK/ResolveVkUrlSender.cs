﻿using AndNetwork9.Shared.Backend.Rabbit;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
namespace AndNetwork9.Shared.Backend.Senders.VK
{
    public class ResolveVkUrlSender : BaseRabbitSenderWithoutResponse<string>
    {
        public const string QUEUE_NAME = "Vk.WallPublish";
        protected ResolveVkUrlSender(IConnection connection, ILogger<BaseRabbitSenderWithoutResponse<string>> logger) : base(connection, QUEUE_NAME, logger)
        {
        }
    }
}