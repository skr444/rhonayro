using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using RhonAyro.Common.Data.ScoreKeeping;
using RhonAyro.Infrastructure.Storage.Api;

namespace RhonAyro.Client.Desktop.Ui.ViewModels
{
    internal sealed class PreparationViewModel : ObservableObject
    {
        private const string ActiveCompetitionIdKey = "activeCompetitionId";

        private readonly ICompetitionRepository competitionRepository;
        private readonly IViewStateRepository viewStateRepository;
        private readonly ILogoRepository logoRepository;
        private DateTime? startDate;
        private int? startHour;
        private int? startMinute;
        private Competition activeCompetition;
        private Logo hostLogo;
        private IList<Logo> sponsorLogos;
        private int sponsorLogoIndex;

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
                    startMinute = (int)Quantize(activeCompetition.EventStart.Minute);
                    OnPropertyChanged(nameof(ActiveCompetitionId));
                    OnPropertyChanged(nameof(EventStart));
                    OnPropertyChanged(nameof(StartDate));
                    OnPropertyChanged(nameof(StartHour));
                    OnPropertyChanged(nameof(StartMinute));
                    viewStateRepository.Save<PreparationViewModel>(ActiveCompetitionIdKey, activeCompetition.Id.ToString());
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

        // logos

        public string HostLogoPath => hostLogo?.Path ?? String.Empty;
        public byte[] HostLogoImage => hostLogo?.ImageData ?? Array.Empty<byte>();

        public ICommand ImportHostLogoCommand { get; }

        public ICommand ImportSponsorLogoCommand { get; }

        public ICommand RemoveSponsorLogoCommand { get; }

        public ICommand PreviousSponsorLogoCommand { get; }

        public ICommand NextSponsorLogoCommand { get; }

        public PreparationViewModel(IFileStorage fileStorage)
        {
            competitionRepository = fileStorage.GetRepository<ICompetitionRepository>();
            viewStateRepository = fileStorage.GetRepository<IViewStateRepository>();
            logoRepository = fileStorage.GetRepository<ILogoRepository>();

            var competition = competitionRepository.All().FirstOrDefault();
            if (competition == null)
            {
                competition = new Competition();
                competitionRepository.AddOrUpdate(competition);
            }
            activeCompetition = competition;
            startDate = activeCompetition.EventStart;
            startHour = activeCompetition.EventStart.Hour;
            startMinute = (int)Quantize(activeCompetition.EventStart.Minute);

            NewEventCommand = new RelayCommand(() =>
            {
                activeCompetition = new Competition();
                competitionRepository.AddOrUpdate(activeCompetition);
                startDate = activeCompetition.EventStart;
                startHour = activeCompetition.EventStart.Hour;
                startMinute = (int)Quantize(activeCompetition.EventStart.Minute);
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
                startMinute = (int)Quantize(activeCompetition.EventStart.Minute);
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

        private static double Quantize(double n, double resolution = 5)
        {
            var q = n / resolution;
            q = Math.Round(q, MidpointRounding.AwayFromZero);
            return q * resolution;
        }
    }
}
