﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using AndNetwork9.Shared.Enums;
using AndNetwork9.Shared.Storage;
using AndNetwork9.Shared.Utility;

namespace AndNetwork9.Shared
{
    public record Task : IComparable<Task>
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public virtual string Description { get; set; } = string.Empty;
        public virtual IList<Comment> Comments { get; set; } = new List<Comment>();
        public virtual IList<Tag> Tags { get; set; } = new List<Tag>(0);

        public virtual IList<StaticFile> Files { get; set; } = new List<StaticFile>(0);

        public int ReadRuleId { get; set; }
        [JsonIgnore]
        public virtual AccessRule ReadRule { get; set; } = null!;
        public int WriteRuleId { get; set; }
        [JsonIgnore]
        public virtual AccessRule WriteRule { get; set; } = null!;

        public int? AssigneeId { get; set; }
        [JsonIgnore]
        public virtual Member? Assignee { get; set; } = null!;
        public int? SquadAssigneeId { get; set; }
        [JsonIgnore]
        public virtual Squad? SquadAssignee { get; set; } = null!;
        public Direction? DirectionAssignee { get; set; } = null!;
        public int? ReporterId { get; set; }
        [JsonIgnore]
        public virtual Member? Reporter { get; set; }

        public IEnumerable<int> WatchersId => Watchers.Select(x => x.Id);
        [JsonIgnore]
        public virtual IList<Member> Watchers { get; set; } = new List<Member>(0);

        public DateTime CreateTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? LastEditTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskLevel Level { get; set; }
        public AwardType? Award { get; set; }

        public bool AllowAssignByMember { get; set; }

        public int? ParentId { get; set; }
        [JsonIgnore]
        public virtual Task? Parent { get; set; }
        [JsonIgnore]
        public virtual IList<Task> Children { get; set; } = new List<Task>(0);

        public int CompareTo(Task? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (other is null) return 1;
            return Priority.CompareTo(other.Priority);
        }

        public virtual IEnumerable<Member> GetAllWatchers()
        {
            foreach (var watcher in Watchers.Where(x => x.DiscordNotificationsEnabled)) yield return watcher;
        }
    }
}