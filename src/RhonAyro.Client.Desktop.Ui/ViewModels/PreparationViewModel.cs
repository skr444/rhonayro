using System;
using System.Collections.Generic;
using System.Linq;

using CommunityToolkit.Mvvm.ComponentModel;

using RhonAyro.Common.Data.ScoreKeeping;
using RhonAyro.Infrastructure.Storage.Api;

namespace RhonAyro.Client.Desktop.Ui.ViewModels
{
    internal sealed class PreparationViewModel : ObservableObject
    {
        private DateTime? startAt;
        private DateTime? startDate;
        private int? startHour;
        private int? startMinute;
        private readonly ICompetitionRepository competitionRepository;
        private Competition activeCompetition;

        public Guid ActiveCompetitionId => activeCompetition.Id;
        public string EventStart => activeCompetition.EventStart.ToString();

        public DateTime? StartDate
        {
            get => startDate;
            set
            {
                if (SetProperty(ref startDate, value))
                {
                    Recompose();
                    activeCompetition.EventStart = Combine(startDate, value);
                    OnPropertyChanged(nameof(EventStart));
                }
            }
        }

        public DateTime? StartAt
        {
            get => startAt;
            set
            {
                if (SetProperty(ref startAt, value))
                {
                    // keep proxies in sync (use null-safety as you like)
                    StartDate = value?.Date;
                    StartHour = value?.Hour;
                    StartMinute = value?.Minute;
                    activeCompetition.EventStart = Combine(startDate, value);
                    OnPropertyChanged(nameof(EventStart));
                }
            }
        }

        public int? StartHour
        {
            get => startHour;
            set
            {
                if (SetProperty(ref startHour, value))
                {
                    Recompose();
                }
            }
        }

        public int? StartMinute
        {
            get => startMinute;
            set
            {
                if (SetProperty(ref startMinute, value))
                {
                    Recompose();
                }
            }
        }

        public IReadOnlyList<int> Hours { get; } = [.. Enumerable.Range(0, 24)];
        public IReadOnlyList<int> Minutes { get; } = [.. Enumerable.Range(0, 60).Where(m => m % 5 == 0)]; // step 5

        public PreparationViewModel(IFileStorage fileStorage)
        {
            competitionRepository = fileStorage.GetRepository<ICompetitionRepository>();
            var competition = competitionRepository.All().FirstOrDefault();
            if (competition == null)
            {
                competition = new Competition();
                competitionRepository.AddOrUpdate(competition);
            }
            activeCompetition = competition;
        }

        private void Recompose()
        {
            if (StartDate is DateTime d && StartHour is int h && StartMinute is int m)
            {
                StartAt = new DateTime(d.Year, d.Month, d.Day, h, m, 0, DateTimeKind.Local);
            }
            else
            {
                StartAt = null; // or keep previous; your choice
            }
        }

        private static DateTime Combine(DateTime? date, DateTime? time)
        {
            DateTime dateCandidate = date ?? DateTime.UtcNow;
            DateTime timeCandidate = time ?? DateTime.UtcNow;

            return new DateTime(
                dateCandidate.Year,
                dateCandidate.Month,
                dateCandidate.Day,
                timeCandidate.Hour,
                timeCandidate.Minute,
                timeCandidate.Second,
                timeCandidate.Millisecond);
        }
    }
}
