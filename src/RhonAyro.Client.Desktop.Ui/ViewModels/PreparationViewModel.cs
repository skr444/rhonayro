using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using RhonAyro.Common.Data.ScoreKeeping;
using RhonAyro.Infrastructure.Storage.Api;

namespace RhonAyro.Client.Desktop.Ui.ViewModels
{
    internal sealed class PreparationViewModel : ObservableObject
    {
        private DateTime? startDate;
        private int? startHour;
        private int? startMinute;
        private readonly ICompetitionRepository competitionRepository;
        private Competition activeCompetition;

        public ICollection<Competition> Competitions
        {
            get => competitionRepository.All();
        }

        public Competition SelectedCompetition
        {
            get => activeCompetition;
            set
            {
                if ((value != null) && SetProperty(ref activeCompetition, value))
                {
                    startDate = activeCompetition.EventStart;
                    startHour = activeCompetition.EventStart.Hour;
                    startMinute = QuantizeMinute(activeCompetition.EventStart.Minute);
                    OnPropertyChanged(nameof(ActiveCompetitionId));
                    OnPropertyChanged(nameof(EventStart));
                    OnPropertyChanged(nameof(StartDate));
                    OnPropertyChanged(nameof(StartHour));
                    OnPropertyChanged(nameof(StartMinute));
                }
            }
        }

        public Guid ActiveCompetitionId => activeCompetition.Id;
        public string EventStart => activeCompetition.EventStart.ToString();

        public DateTime StartDate
        {
            get => startDate ?? DateTime.Today;
            set
            {
                if (SetProperty(ref startDate, value))
                {
                    Recompose();
                }
            }
        }

        public int StartHour
        {
            get => startHour ?? 0;
            set
            {
                if (SetProperty(ref startHour, value))
                {
                    Recompose();
                }
            }
        }

        public int StartMinute
        {
            get => startMinute ?? 0;
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

        public ICommand NewEventCommand { get; }
        public ICommand DeleteCompetitionCommand { get; }

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
            startDate = activeCompetition.EventStart;
            startHour = activeCompetition.EventStart.Hour;
            startMinute = activeCompetition.EventStart.Minute;

            NewEventCommand = new RelayCommand(() =>
            {
                activeCompetition = new Competition();
                competitionRepository.AddOrUpdate(activeCompetition);
                startDate = activeCompetition.EventStart;
                startHour = activeCompetition.EventStart.Hour;
                startMinute = activeCompetition.EventStart.Minute;
                OnPropertyChanged(nameof(SelectedCompetition));
                OnPropertyChanged(nameof(ActiveCompetitionId));
                OnPropertyChanged(nameof(EventStart));
                OnPropertyChanged(nameof(StartDate));
                OnPropertyChanged(nameof(StartHour));
                OnPropertyChanged(nameof(StartMinute));
                OnPropertyChanged(nameof(Competitions));
            });

            DeleteCompetitionCommand = new RelayCommand(() =>
            {
                competitionRepository.Delete(activeCompetition.Id);
                var next = competitionRepository.All().FirstOrDefault();
                if (next == null)
                {
                    next = new Competition();
                    competitionRepository.AddOrUpdate(next);
                }
                activeCompetition = next;
                startDate = activeCompetition.EventStart;
                startHour = activeCompetition.EventStart.Hour;
                startMinute = activeCompetition.EventStart.Minute;
                OnPropertyChanged(nameof(SelectedCompetition));
                OnPropertyChanged(nameof(ActiveCompetitionId));
                OnPropertyChanged(nameof(EventStart));
                OnPropertyChanged(nameof(StartDate));
                OnPropertyChanged(nameof(StartHour));
                OnPropertyChanged(nameof(StartMinute));
                OnPropertyChanged(nameof(Competitions));
            });
        }

        private void Recompose()
        {
            if (startDate is DateTime d && startHour is int h && startMinute is int m)
            {
                var dt = new DateTime(d.Year, d.Month, d.Day, h, m, 0, DateTimeKind.Local);
                activeCompetition.EventStart = dt;
                OnPropertyChanged(nameof(EventStart));
                competitionRepository.AddOrUpdate(activeCompetition);
                OnPropertyChanged(nameof(Competitions));
            }
        }

        private static int QuantizeMinute(int minute, int resolution = 5)
        {
            return (int)Math.Round((double)minute, 0) % 5;
        }
    }
}
