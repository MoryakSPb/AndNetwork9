﻿using And9.Service.Core.Abstractions.Enums;
using And9.Service.Core.Abstractions.Interfaces;
using MessagePack;

namespace And9.Service.Core.Abstractions.Models;

[MessagePackObject]
public class Specialization : ISpecialization
{
    [IgnoreMember]
    public IList<MemberSpecialization> MemberSpecializations { get; set; } = new List<MemberSpecialization>();
    [Key(0)]
    public int Id { get; set; }
    [Key(1)]
    public Direction Direction { get; set; }
    [Key(2)]
    public string Name { get; set; } = string.Empty;
    [Key(3)]
    public string Description { get; set; } = String.Empty;
    [Key(4)]
    public DateTime LastChanged { get; set; }
    [Key(5)]
    public Guid ConcurrencyToken { get; set; }
}