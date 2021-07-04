﻿namespace AndNetwork9.Shared.Enums
{
    public enum Rank
    {
        Outcast = int.MinValue,
        Enemy = -5,
        Guest = -4,
        Diplomat = -3,
        Ally = -2,
        Candidate = -1,
        None = 0,
        Neophyte,
        Trainee,
        Assistant,
        JuniorEmployee,
        Employee,
        SeniorEmployee,
        Specialist,
        Defender,
        Advisor,
        FirstAdvisor = int.MaxValue,
    }
}