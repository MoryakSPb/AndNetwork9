﻿using AndNetwork9.Shared;
using Microsoft.AspNetCore.Authorization;

namespace AndNetwork9.Server.Auth.Attributes;

public class LieutenantAuthorizeAttribute : AuthorizeAttribute, IAuthorizationRequirement, IAuthPass
{
    public bool Pass(Member member)
    {
        return member.SquadCommander && member.SquadPart is not null && member.SquadPartNumber != 0;
    }
}