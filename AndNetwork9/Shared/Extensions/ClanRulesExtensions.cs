﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AndNetwork9.Shared.Enums;

namespace AndNetwork9.Shared.Extensions
{
    public static class ClanRulesExtensions
    {
        public const int ADVISOR_TERM_DAYS = 90;
        public const int VOTING_DAYS = 5;
        public const int ANNOUNCEMENT_DAYS = 5;
        public const int MIN_DIRECTION_CHANGE_DAYS = 5;

        public static readonly IReadOnlyDictionary<Rank, int> RankPoints = new ReadOnlyDictionary<Rank, int>(
            new Dictionary<Rank, int>(new[]
            {
                new KeyValuePair<Rank, int>(Rank.Neophyte, 0),
                new KeyValuePair<Rank, int>(Rank.Trainee, 5),
                new KeyValuePair<Rank, int>(Rank.Assistant, 10),
                new KeyValuePair<Rank, int>(Rank.JuniorEmployee, 15),
                new KeyValuePair<Rank, int>(Rank.Employee, 20),
                new KeyValuePair<Rank, int>(Rank.SeniorEmployee, 25),
                new KeyValuePair<Rank, int>(Rank.JuniorSpecialist, 30),
                new KeyValuePair<Rank, int>(Rank.Specialist, 35),
                new KeyValuePair<Rank, int>(Rank.SeniorSpecialist, 40),
                new KeyValuePair<Rank, int>(Rank.JuniorIntercessor, 45),
                new KeyValuePair<Rank, int>(Rank.Intercessor, 50),
                new KeyValuePair<Rank, int>(Rank.SeniorIntercessor, 55),
                new KeyValuePair<Rank, int>(Rank.JuniorSentinel, 60),
                new KeyValuePair<Rank, int>(Rank.Sentinel, 65),
                new KeyValuePair<Rank, int>(Rank.SeniorSentinel, 70),
            }));

        public static Rank GetRank(this IEnumerable<Award> awards)
        {
            double result = awards.Sum(x => x.Points);
            return RankPoints.Where(x => x.Value <= result).OrderByDescending(x => x.Value).First().Key;
        }

        public static string? GetRankIcon(this Rank rank)
        {
            return rank switch
            {
                Rank.Outcast => null,
                Rank.Enemy => null,
                Rank.Guest => null,
                Rank.Diplomat => null,
                Rank.Ally => null,
                Rank.Candidate => null,
                Rank.None => null,
                Rank.Neophyte => "⦁⦁⦁",
                Rank.Trainee => "⦁⦁",
                Rank.Assistant => "⦁",
                Rank.JuniorEmployee => "❮❮❮",
                Rank.Employee => "❮❮",
                Rank.SeniorEmployee => "❮",
                Rank.JuniorSpecialist => "❙❙❙",
                Rank.Specialist => "❙❙",
                Rank.SeniorSpecialist => "❙",
                Rank.JuniorIntercessor => "☆☆☆",
                Rank.Intercessor => "☆☆",
                Rank.SeniorIntercessor => "☆",
                Rank.JuniorSentinel => "★★★",
                Rank.Sentinel => "★★",
                Rank.SeniorSentinel => "★",
                Rank.Advisor => "△",
                Rank.FirstAdvisor => "▲",
                _ => throw new ArgumentOutOfRangeException(nameof(rank), rank, null),
            };
        }

        public static string GetName(this Direction department)
        {
            return department switch
            {
                Direction.Reserve => "Резерв",
                Direction.None => "Н/Д",
                Direction.Training => "Обучение и тренировка",
                Direction.Infrastructure => "Развитие и инфраструктура",
                Direction.Research => "Исследования и разработка",
                Direction.Military => "Атака и оборона",
                Direction.Agitation => "Агитация и внешние связи",
                _ => throw new ArgumentOutOfRangeException(nameof(department), department, null),
            };
        }

        public static string GetRankName(this Rank rank)
        {
            return rank switch
            {
                Rank.Outcast => "Изганнник",
                Rank.Enemy => "Противник",
                Rank.Guest => "Гость",
                Rank.Diplomat => "Дипломат",
                Rank.Ally => "Союзник",
                Rank.Candidate => "Кандидат",
                Rank.None => "",
                Rank.Neophyte => "Неофит",
                Rank.Trainee => "Стажёр",
                Rank.Assistant => "Ассистент",
                Rank.JuniorEmployee => "Младший сотрудник",
                Rank.Employee => "Сотрудник",
                Rank.SeniorEmployee => "Старший сорудник",
                Rank.JuniorSpecialist => "Младший специалист",
                Rank.Specialist => "Специалист",
                Rank.SeniorSpecialist => "Старший специалист",
                Rank.JuniorIntercessor => "Младший заступник",
                Rank.Intercessor => "Заступник",
                Rank.SeniorIntercessor => "Старший заступник",
                Rank.JuniorSentinel => "Младший страж",
                Rank.Sentinel => "Страж",
                Rank.SeniorSentinel => "Старший страж",
                Rank.Advisor => "Советник",
                Rank.FirstAdvisor => "Первый советник",
                _ => throw new ArgumentOutOfRangeException(nameof(rank), rank, null),
            };
        }

        public static string GetTypeName(this AwardType type)
        {
            return type switch
            {
                AwardType.None => string.Empty,
                AwardType.Copper => "Медь",
                AwardType.Bronze => "Бронза",
                AwardType.Silver => "Серебро",
                AwardType.Gold => "Золото",
                AwardType.Sapphire => "Сапфир",
                AwardType.Hero => "Герой",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
            };
        }

        public static string GetVoteName(this MemberVote vote)
        {
            return vote switch
            {
                MemberVote.Invalid => string.Empty,
                MemberVote.NoVote => "Голос не отдан",
                MemberVote.Accept => "За",
                MemberVote.Decline => "Против",
                MemberVote.Abstained => "Равнозначно",
                MemberVote.NeedMoreInformation => "Необходимо больше информации",
                _ => throw new ArgumentOutOfRangeException(nameof(vote), vote, null),
            };
        }

        public static string GetVoteResult(this MemberVote vote)
        {
            return vote switch
            {
                MemberVote.Invalid => string.Empty,
                MemberVote.NoVote => "Голосование идет",
                MemberVote.Accept => "Принято",
                MemberVote.Decline => "Отклюнено",
                MemberVote.Abstained => "Статус-кво",
                MemberVote.NeedMoreInformation => "Недостаточно информации",
                _ => throw new ArgumentOutOfRangeException(nameof(vote), vote, null),
            };
        }

        public static string GetAwardSymbol(this AwardType awardType)
        {
            return awardType switch
            {
                AwardType.None => string.Empty,
                AwardType.Copper => "\U0001f7e9",
                AwardType.Bronze => "\U0001F7EB",
                AwardType.Silver => "\U00002B1C",
                AwardType.Gold => "\U0001F7E8",
                AwardType.Sapphire => "\U0001f7e6",
                AwardType.Hero => "\U0001f7ea",
                _ => throw new ArgumentOutOfRangeException(nameof(awardType), awardType, null),
            };
        }

        public static string GetString(this RepoType type)
        {
            return type switch
            {
                RepoType.None => "Н/Д",
                RepoType.Blueprint => "Чертёж",
                RepoType.Script => "Скрипт",
                RepoType.World => "Мир",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
            };
        }
    }
}