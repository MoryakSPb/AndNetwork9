﻿namespace AndNetwork9.Shared.Enums;

public enum Rank
{
    Outcast = int.MinValue,
    Enemy = -7,
    Guest = -6,
    Diplomat = -5,
    Ally = -4,
    Candidate = -3,
    Auxiliary = -2,
    SeniorAuxiliary = -1,
    None = 0,
    Neophyte,
    Trainee,
    Assistant,
    JuniorEmployee,
    Employee,
    SeniorEmployee,
    JuniorSpecialist,
    Specialist,
    SeniorSpecialist,
    JuniorIntercessor,
    Intercessor,
    SeniorIntercessor,
    JuniorSentinel,
    Sentinel,
    SeniorSentinel,
    Advisor,
    FirstAdvisor = int.MaxValue,
}