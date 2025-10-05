using System;
using System.Collections.Generic;
using System.Linq;

using CommunityToolkit.Mvvm.ComponentModel;

using RhonAyro.Client.Desktop.Ui.Services;
using RhonAyro.Common.Data.ScoreKeeping;
using RhonAyro.Infrastructure.Storage.Api;
using RhonAyro.Infrastructure.Storage.File;

namespace RhonAyro.Client.Desktop.Ui.ViewModels
{
    internal sealed class StartListViewModel : ObservableObject
    {
        private readonly ICompetitionService competitionService;
        private readonly IClubMemberRepository clubMemberRepository;
        private readonly IStartListEntryRepository startListEntryRepository;
        private readonly IWheelRepository wheelRepository;
        private ClubMember? selectedAthlete;
        private ClubMember? selectedCoach;
        private StartListEntry activeStartListEntry;
        private Wheel activeWheel;

        public ICollection<ClubMember> Athletes
        {
            get => clubMemberRepository.All().Where(x => x.Roles.HasFlag(RoleType.Athlete)).ToList();
        }

        public ClubMember? SelectedAthlete
        {
            get => selectedAthlete;
            set
            {
                if ((value != null) && SetProperty(ref selectedAthlete, value))
                {
                    OnPropertyChanged(nameof(SelectedAthleteName));
                }
            }
        }

        public string SelectedAthleteName => selectedAthlete?.FullName ?? String.Empty;

        public ICollection<ClubMember> Coaches
        {
            get => clubMemberRepository.All().Where(x => x.Roles.HasFlag(RoleType.Coach)).ToList();
        }

        public ClubMember? SelectedCoach
        {
            get => selectedCoach;
            set
            {
                if ((value != null) && SetProperty(ref selectedCoach, value))
                {
                    OnPropertyChanged(nameof(SelectedCoachName));
                }
            }
        }

        public string SelectedCoachName => selectedCoach?.FullName ?? String.Empty;

        public StartListViewModel(IFileStorage fileStorage, ICompetitionService competitionService)
        {
            clubMemberRepository = fileStorage.GetRepository<IClubMemberRepository>();
            startListEntryRepository = fileStorage.GetRepository<IStartListEntryRepository>();
            wheelRepository = fileStorage.GetRepository<IWheelRepository>();
            
            var startListEntry = startListEntryRepository.All().FirstOrDefault();
            if (startListEntry == null)
            {
                startListEntry = new StartListEntry();
                startListEntryRepository.AddOrUpdate(startListEntry);
            }

            var wheelEntry = wheelRepository.All().FirstOrDefault();
            if (wheelEntry == null)
            {
                wheelEntry = new Wheel();
                wheelRepository.AddOrUpdate(wheelEntry);
            }
        }
    }
}
