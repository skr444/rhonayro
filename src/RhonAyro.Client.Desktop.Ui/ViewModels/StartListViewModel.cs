using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using RhonAyro.Client.Desktop.Ui.Services;
using RhonAyro.Common.Data.ScoreKeeping;
using RhonAyro.Infrastructure.Storage.Api;

namespace RhonAyro.Client.Desktop.Ui.ViewModels
{
    internal sealed class StartListViewModel : ObservableObject
    {
        #region Types

        internal sealed class DisciplineSelectionItem : ObservableObject
        {
            private bool isSelected;
            private float? wheelSize;

            public Discipline Discipline { get; }

            public string DisciplineName => Discipline.Name;

            public bool IsSelected
            {
                get => isSelected;
                set
                {
                    if (SetProperty(ref isSelected, value) && !value)
                    {
                        WheelSize = null;
                    }
                }
            }

            public float? WheelSize
            {
                get => wheelSize;
                set => SetProperty(ref wheelSize, value);
            }

            public DisciplineSelectionItem(Discipline discipline)
            {
                Discipline = discipline;
            }
        }

        #endregion Types

        private readonly ICompetitionService competitionService;
        private readonly IStartListService startListService;
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
                    (AddToRosterCommand as RelayCommand)?.NotifyCanExecuteChanged();
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
                    (AddToRosterCommand as RelayCommand)?.NotifyCanExecuteChanged();
                }
            }
        }

        public string SelectedCoachName => selectedCoach?.FullName ?? String.Empty;

        public ObservableCollection<DisciplineSelectionItem> DisciplinesAndWheelSizes { get; }

        public ICommand AddToRosterCommand { get; }

        public StartListViewModel(IFileStorage fileStorage, ICompetitionService competitionService, IStartListService startListService)
        {
            clubMemberRepository = fileStorage.GetRepository<IClubMemberRepository>();
            startListEntryRepository = fileStorage.GetRepository<IStartListEntryRepository>();
            wheelRepository = fileStorage.GetRepository<IWheelRepository>();
            this.competitionService = competitionService;
            this.startListService = startListService;

            DisciplinesAndWheelSizes = new ObservableCollection<DisciplineSelectionItem>(
                competitionService.Disciplines.Select(x => new DisciplineSelectionItem(x)));
            DisciplinesAndWheelSizes.CollectionChanged += (_, e) =>
            {
                if (e.NewItems != null)
                {
                    foreach (DisciplineSelectionItem item in e.NewItems)
                    {
                        item.PropertyChanged += OnDisciplineItemPropertyChanged;
                    }
                }

                if (e.OldItems != null)
                {
                    foreach (DisciplineSelectionItem item in e.OldItems)
                    {
                        item.PropertyChanged -= OnDisciplineItemPropertyChanged;
                    }
                }
            };
            foreach (var item in DisciplinesAndWheelSizes)
            {
                item.PropertyChanged += OnDisciplineItemPropertyChanged;
            }
            
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

            AddToRosterCommand = new RelayCommand(() =>
            {
                var selections = DisciplinesAndWheelSizes
                    .Where(x => x.IsSelected && x.WheelSize.HasValue)
                    .Select(x => new { x.Discipline, Wheel = x.WheelSize!.Value })
                    .ToList();
                var t = selections;
            }, () =>
            {
                return (SelectedAthlete != null)
                    && (SelectedCoach != null)
                    && DisciplinesAndWheelSizes.Any(x => x.IsSelected && x.WheelSize.HasValue);
            });
        }

        private void OnDisciplineItemPropertyChanged(object? _, PropertyChangedEventArgs args)
        {
            if (args.PropertyName is nameof(DisciplineSelectionItem.IsSelected)
                or nameof(DisciplineSelectionItem.WheelSize))
            {
                (AddToRosterCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }
    }
}
