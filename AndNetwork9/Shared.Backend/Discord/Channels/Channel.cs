﻿using System;
using System.Collections.Generic;
using AndNetwork9.Shared.Backend.Discord.Enums;

namespace AndNetwork9.Shared.Backend.Discord.Channels
{
    public class Channel : IEquatable<Channel>, IComparable<Channel>, IComparable
    {
        public ulong DiscordId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ChannelType Type { get; set; }

        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public int ChannelPosition { get; set; }

        public Permissions EveryonePermissions { get; set; }
        public Permissions MemberPermissions { get; set; }
        public Permissions AdvisorPermissions { get; set; }

        public virtual Squad? Squad { get; set; }
        public Permissions SquadPermissions { get; set; }
        public Permissions SquadCommanderPermissions { get; set; }

        public ChannelFlags ChannelFlags { get; set; }

        public int CompareTo(object? obj)
        {
            if (obj is null) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            return obj is Channel other
                ? CompareTo(other)
                : throw new ArgumentException($"Object must be of type {nameof(Channel)}");
        }

        public int CompareTo(Channel? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (other is null) return 1;
            if (CategoryId is null)
                return other.CategoryId is null ? ChannelPosition.CompareTo(other.ChannelPosition) : -1;
            if (other.CategoryId is null) return 1;
            int categoryCompare = CategoryId.Value.CompareTo(other.CategoryId.Value);
            return categoryCompare == 0 ? ChannelPosition.CompareTo(other.ChannelPosition) : categoryCompare;
        }

        public bool Equals(Channel? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return DiscordId == other.DiscordId;
        }

        public static bool operator <(Channel? left, Channel? right)
        {
            return Comparer<Channel>.Default.Compare(left, right) < 0;
        }

        public static bool operator >(Channel? left, Channel? right)
        {
            return Comparer<Channel>.Default.Compare(left, right) > 0;
        }

        public static bool operator <=(Channel? left, Channel? right)
        {
            return Comparer<Channel>.Default.Compare(left, right) <= 0;
        }

        public static bool operator >=(Channel? left, Channel? right)
        {
            return Comparer<Channel>.Default.Compare(left, right) >= 0;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Channel)obj);
        }

        public override int GetHashCode()
        {
            return DiscordId.GetHashCode();
        }

        public static bool operator ==(Channel? left, Channel? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Channel? left, Channel? right)
        {
            return !Equals(left, right);
        }
    }
}