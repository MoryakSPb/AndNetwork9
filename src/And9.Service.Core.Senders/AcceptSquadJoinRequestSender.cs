﻿using And9.Lib.Broker;
using And9.Lib.Broker.Senders;

namespace And9.Service.Core.Senders;

[QueueName(QUEUE_NAME)]
public class AcceptSquadJoinRequestSender : BrokerSenderWithoutResponse<(short number, int memberId)>
{
    public const string QUEUE_NAME = "And9.Service.Core.AcceptSquadJoinRequest";
    public AcceptSquadJoinRequestSender(BrokerManager brokerManager) : base(brokerManager) { }
}