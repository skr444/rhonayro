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
        private const string HostLogoKey = "HostLogo";

        private readonly ICompetitionRepository competitionRepository;
        private readonly ICompetitionService competitionService;
        private readonly IViewStateRepository viewStateRepository;
        private readonly ILogoRepository logoRepository;
        private readonly ILogoService logoService;
        private readonly INavigationService navigation;
        private readonly IFileSystemService fileSystem;
        private readonly IResourceService resources;
        private DateTime? startDate;
        private int? startHour;
        private int? startMinute;
        private Competition selectedCompetition;
        private Logo? hostLogo;
        private IList<Logo> sponsorLogos;
        private int sponsorLogoIndex;

        public ICollection<Competition> Competitions
        {
            get => competitionRepository.All();
        }

        public Competition SelectedCompetition
        {
            get => competitionService.ActiveCompetition;
            set
            {
                if ((value != null) && SetProperty(ref selectedCompetition, value))
                {
                    competitionService.SetActiveCompetition(selectedCompetition.Id);
                    startDate = selectedCompetition.EventStart;
                    startHour = selectedCompetition.EventStart.Hour;
                    startMinute = (int)Quantize(selectedCompetition.EventStart.Minute);
                    OnPropertyChanged(nameof(ActiveCompetitionId));
                    OnPropertyChanged(nameof(EventStart));
                    OnPropertyChanged(nameof(StartDate));
                    OnPropertyChanged(nameof(StartHour));
                    OnPropertyChanged(nameof(StartMinute));
                    hostLogo = logoService.GetLogo(HostLogoKey, selectedCompetition.Id, true);
                    OnPropertyChanged(nameof(HostLogoImage));
                    OnPropertyChanged(nameof(HostLogoPath));
                }
            }
        }

        public Guid ActiveCompetitionId => competitionService.ActiveCompetition.Id;
        public string EventStart => competitionService.ActiveCompetition.EventStart.ToString();

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
            IFileSystemService fileSystemService, IResourceService resourceService, ILogoService logoService,
            ICompetitionService competitionService)
        {
            competitionRepository = fileStorage.GetRepository<ICompetitionRepository>();
            this.competitionService = competitionService;
            viewStateRepository = fileStorage.GetRepository<IViewStateRepository>();
            logoRepository = fileStorage.GetRepository<ILogoRepository>();
            this.logoService = logoService;
            navigation = navigationService;
            fileSystem = fileSystemService;
            resources = resourceService;

            selectedCompetition = competitionService.ActiveCompetition;
            startDate = selectedCompetition.EventStart;
            startHour = selectedCompetition.EventStart.Hour;
            startMinute = (int)Quantize(selectedCompetition.EventStart.Minute);
            OnPropertyChanged(nameof(SelectedCompetition));
            OnPropertyChanged(nameof(ActiveCompetitionId));
            OnPropertyChanged(nameof(EventStart));
            OnPropertyChanged(nameof(StartDate));
            OnPropertyChanged(nameof(StartHour));
            OnPropertyChanged(nameof(StartMinute));

            hostLogo = this.logoService.FirstOrDefault(x => x.CompetitionId == competitionService.ActiveCompetitionId, true);
            OnPropertyChanged(nameof(HostLogoImage));
            OnPropertyChanged(nameof(HostLogoPath));

            NewEventCommand = new RelayCommand(() =>
            {
                competitionService.NewActiveCompetition();
                selectedCompetition = competitionService.ActiveCompetition;
                startDate = selectedCompetition.EventStart;
                startHour = selectedCompetition.EventStart.Hour;
                startMinute = (int)Quantize(selectedCompetition.EventStart.Minute);
                OnPropertyChanged(nameof(SelectedCompetition));
                OnPropertyChanged(nameof(ActiveCompetitionId));
                OnPropertyChanged(nameof(EventStart));
                OnPropertyChanged(nameof(StartDate));
                OnPropertyChanged(nameof(StartHour));
                OnPropertyChanged(nameof(StartMinute));
                OnPropertyChanged(nameof(Competitions));
                hostLogo = logoService.GetLogo(HostLogoKey, selectedCompetition.Id, true);
                OnPropertyChanged(nameof(HostLogoImage));
                OnPropertyChanged(nameof(HostLogoPath));
            });

            DeleteCompetitionCommand = new RelayCommand(() =>
            {
                competitionService.RemoveActiveCompetition();
                selectedCompetition = competitionService.ActiveCompetition;
                startDate = selectedCompetition.EventStart;
                startHour = selectedCompetition.EventStart.Hour;
                startMinute = (int)Quantize(selectedCompetition.EventStart.Minute);
                OnPropertyChanged(nameof(SelectedCompetition));
                OnPropertyChanged(nameof(ActiveCompetitionId));
                OnPropertyChanged(nameof(EventStart));
                OnPropertyChanged(nameof(StartDate));
                OnPropertyChanged(nameof(StartHour));
                OnPropertyChanged(nameof(StartMinute));
                OnPropertyChanged(nameof(Competitions));
                hostLogo = logoService.GetLogo(HostLogoKey, selectedCompetition.Id, true);
                OnPropertyChanged(nameof(HostLogoImage));
                OnPropertyChanged(nameof(HostLogoPath));
            });

            ImportHostLogoCommand = new RelayCommand(() =>
            {
                var ofd = navigation.NewOfd();
                if (ofd.ShowDialog() ?? false)
                {
                    hostLogo = logoService.GetLogo(HostLogoKey, Path.GetFileName(ofd.FileName), selectedCompetition.Id);
                    if (hostLogo == null)
                    {
                        hostLogo = new Logo
                        {
                            CompetitionId = selectedCompetition.Id,
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
                selectedCompetition.EventStart = dt;
                OnPropertyChanged(nameof(EventStart));
                competitionRepository.AddOrUpdate(selectedCompetition);
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
