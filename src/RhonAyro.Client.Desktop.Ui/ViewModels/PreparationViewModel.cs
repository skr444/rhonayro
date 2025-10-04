using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using RhonAyro.Client.Desktop.Ui.Navigation;
using RhonAyro.Client.Desktop.Ui.Services;
using RhonAyro.Common.Data.ScoreKeeping;
using RhonAyro.Infrastructure.Storage.Api;

namespace RhonAyro.Client.Desktop.Ui.ViewModels
{
    internal sealed class PreparationViewModel : ObservableObject
    {
        private const string ActiveCompetitionIdKey = "activeCompetitionId";
        private const string HostLogoKey = "HostLogo";

        private readonly ICompetitionRepository competitionRepository;
        private readonly IViewStateRepository viewStateRepository;
        private readonly ILogoRepository logoRepository;
        private readonly ILogoService logoService;
        private readonly INavigationService navigation;
        private readonly IFileSystemService fileSystem;
        private readonly IResourceService resources;
        private DateTime? startDate;
        private int? startHour;
        private int? startMinute;
        private Competition activeCompetition;
        private Logo? hostLogo;
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
                    hostLogo = logoService.GetLogo(HostLogoKey, activeCompetition.Id, true);
                    OnPropertyChanged(nameof(HostLogoImage));
                    OnPropertyChanged(nameof(HostLogoPath));
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

        public string HostLogoPath => hostLogo?.Name ?? String.Empty;
        public byte[] HostLogoImage => logoService.ToLogoBytes(hostLogo?.ImageData);

        public ICommand ImportHostLogoCommand { get; }

        public ICommand ImportSponsorLogoCommand { get; }

        public ICommand RemoveSponsorLogoCommand { get; }

        public ICommand PreviousSponsorLogoCommand { get; }

        public ICommand NextSponsorLogoCommand { get; }

        public PreparationViewModel(IFileStorage fileStorage, INavigationService navigationService,
            IFileSystemService fileSystemService, IResourceService resourceService, ILogoService logoService)
        {
            competitionRepository = fileStorage.GetRepository<ICompetitionRepository>();
            viewStateRepository = fileStorage.GetRepository<IViewStateRepository>();
            logoRepository = fileStorage.GetRepository<ILogoRepository>();
            this.logoService = logoService;
            navigation = navigationService;
            fileSystem = fileSystemService;
            resources = resourceService;

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

            hostLogo = this.logoService.FirstOrDefault();
            OnPropertyChanged(nameof(HostLogoImage));
            OnPropertyChanged(nameof(HostLogoPath));

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

            ImportHostLogoCommand = new RelayCommand(() =>
            {
                var ofd = navigation.NewOfd();
                if (ofd.ShowDialog() ?? false)
                {
                    hostLogo = logoService.GetLogo(HostLogoKey, Path.GetFileName(ofd.FileName), activeCompetition.Id);
                    if (hostLogo == null)
                    {
                        hostLogo = new Logo
                        {
                            CompetitionId = activeCompetition.Id,
                            Name = Path.GetFileName(ofd.FileName),
                            Type = HostLogoKey,
                            ImageData = fileSystemService.ReadFileBytes(ofd.FileName)
                        };
                        logoRepository.AddOrUpdate(hostLogo);
                    }
                    OnPropertyChanged(nameof(HostLogoImage));
                    OnPropertyChanged(nameof(HostLogoPath));
                }
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
