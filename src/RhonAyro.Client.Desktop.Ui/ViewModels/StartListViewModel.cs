using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using RhonAyro.Client.Desktop.Ui.Navigation;
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

        internal sealed class StartListEntryItem : ObservableObject
        {
            private readonly StartListEntry model;
            private readonly IClubMemberRepository clubMemberRepository;
            private readonly IDisciplineRepository disciplineRepository;
            private readonly IWheelRepository wheelRepository;

            public Guid StartListEntryId => model.Id;

            public int Position => model.StartPosition;

            public string Athlete
            {
                get
                {
                    if (clubMemberRepository.TryGet(model.AthleteId ?? Guid.Empty, out ClubMember? athlete))
                    {
                        return athlete!.FullName;
                    }

                    return String.Empty;
                }
            }

            public string Discipline
            {
                get
                {
                    if (disciplineRepository.TryGet(model.DisciplineId ?? Guid.Empty, out Discipline? discipline))
                    {
                        return discipline!.Name;
                    }

                    return String.Empty;
                }
            }

            public float WheelSize
            {
                get
                {
                    if (wheelRepository.TryGet(model.WheelId ?? Guid.Empty, out Wheel? wheel))
                    {
                        return wheel!.Size;
                    }

                    return 0;
                }
            }

            public string Coach
            {
                get
                {
                    if (clubMemberRepository.TryGet(model.CoachId ?? Guid.Empty, out ClubMember? coach))
                    {
                        return coach!.FullName;
                    }

                    return String.Empty;
                }
            }

            public StartListEntryItem(StartListEntry item, IClubMemberRepository clubMembers,
                IDisciplineRepository disciplines, IWheelRepository wheels)
            {
                model = item;
                clubMemberRepository = clubMembers;
                disciplineRepository = disciplines;
                wheelRepository = wheels;
            }
        }

        #endregion Types

        private readonly IStartListService startListService;
        private readonly IClubMemberRepository clubMemberRepository;
        private readonly IDisciplineRepository disciplineRepository;
        private readonly IWheelRepository wheelRepository;
        private readonly ICompetitionService competitionService;
        private ClubMember? selectedAthlete;
        private ClubMember? selectedCoach;
        private StartListEntryItem? selectedStartListEntryItem;
        private Guid? activeDisciplineFilter;

        public string AllDisciplinesLabel => "All";

        public bool AllDisciplinesChecked
        {
            set
            {
                if (value)
                {
                    activeDisciplineFilter = null;
                    OnPropertyChanged(nameof(Roster));
                }
            }
        }

        public string StraightBasicLabel =>
            competitionService.GetDiscipline(Guid.Parse("dddddddd-0000-0000-0000-d00000000001"))!.Name;

        public bool StraightBasicChecked
        {
            set
            {
                if (value)
                {
                    activeDisciplineFilter = Guid.Parse("dddddddd-0000-0000-0000-d00000000001");
                    OnPropertyChanged(nameof(Roster));
                }
            }
        }

        public string StraightAdvancedLabel =>
            competitionService.GetDiscipline(Guid.Parse("dddddddd-0000-0000-0000-d00000000002"))!.Name;

        public bool StraightAdvancedChecked
        {
            set
            {
                if (value)
                {
                    activeDisciplineFilter = Guid.Parse("dddddddd-0000-0000-0000-d00000000002");
                    OnPropertyChanged(nameof(Roster));
                }
            }
        }

        public string JumpLabel =>
            competitionService.GetDiscipline(Guid.Parse("dddddddd-0000-0000-0000-d00000000003"))!.Name;

        public bool JumpChecked
        {
            set
            {
                if (value)
                {
                    activeDisciplineFilter = Guid.Parse("dddddddd-0000-0000-0000-d00000000003");
                    OnPropertyChanged(nameof(Roster));
                }
            }
        }

        public string SpiralLabel =>
            competitionService.GetDiscipline(Guid.Parse("dddddddd-0000-0000-0000-d00000000004"))!.Name;

        public bool SpiralChecked
        {
            set
            {
                if (value)
                {
                    activeDisciplineFilter = Guid.Parse("dddddddd-0000-0000-0000-d00000000004");
                    OnPropertyChanged(nameof(Roster));
                }
            }
        }

        public string PairLabel =>
            competitionService.GetDiscipline(Guid.Parse("dddddddd-0000-0000-0000-d00000000005"))!.Name;

        public bool PairChecked
        {
            set
            {
                if (value)
                {
                    activeDisciplineFilter = Guid.Parse("dddddddd-0000-0000-0000-d00000000005");
                    OnPropertyChanged(nameof(Roster));
                }
            }
        }

        public IEnumerable<ClubMember> Athletes
        {
            get => clubMemberRepository.All(x => x.Roles.HasFlag(RoleType.Athlete));
        }

        public ClubMember? SelectedAthlete
        {
            get => selectedAthlete;
            set
            {
                if ((value != null) && SetProperty(ref selectedAthlete, value))
                {
                    startListService.SetActiveAthlete(selectedAthlete.Id);
                    OnPropertyChanged(nameof(SelectedAthleteName));

                    if (selectedCoach?.Id == value.Id)
                    {
                        startListService.SetActiveCoach(null);
                        selectedCoach = null;
                        OnPropertyChanged(nameof(SelectedCoach));
                        OnPropertyChanged(nameof(SelectedCoachName));
                    }

                    OnPropertyChanged(nameof(Coaches));
                    (AddToRosterCommand as RelayCommand)?.NotifyCanExecuteChanged();

                    selectedStartListEntryItem = null;
                    OnPropertyChanged(nameof(StartPosition));
                    (RemoveFromRosterCommand as RelayCommand)?.NotifyCanExecuteChanged();
                }
            }
        }

        public string SelectedAthleteName => selectedAthlete?.FullName ?? String.Empty;

        public IEnumerable<ClubMember> Coaches
        {
            get => clubMemberRepository.All(x =>
                   x.Roles.HasFlag(RoleType.Coach)
                && x.Id != selectedAthlete?.Id);
        }

        public ClubMember? SelectedCoach
        {
            get => selectedCoach;
            set
            {
                if ((value != null) && SetProperty(ref selectedCoach, value))
                {
                    startListService.SetActiveCoach(selectedCoach.Id);
                    OnPropertyChanged(nameof(SelectedCoachName));
                    (AddToRosterCommand as RelayCommand)?.NotifyCanExecuteChanged();

                    selectedStartListEntryItem = null;
                    OnPropertyChanged(nameof(StartPosition));
                    (RemoveFromRosterCommand as RelayCommand)?.NotifyCanExecuteChanged();
                }
            }
        }

        public string SelectedCoachName => selectedCoach?.FullName ?? String.Empty;

        public ObservableCollection<DisciplineSelectionItem> DisciplinesAndWheelSizes { get; }

        public IEnumerable<StartListEntryItem> Roster
        {
            get
            {
                if (activeDisciplineFilter == null)
                {
                    return startListService.Roster
                        .OrderBy(x => x.StartPosition)
                        .Select(x => new StartListEntryItem(x, clubMemberRepository, disciplineRepository,
                            wheelRepository));
                }

                return startListService.Roster
                    .Where(x => x.DisciplineId == activeDisciplineFilter)
                    .OrderBy(x => x.StartPosition)
                    .Select(x => new StartListEntryItem(x, clubMemberRepository, disciplineRepository, wheelRepository));
            }
        }

        public StartListEntryItem? SelectedStartListEntryItem
        {
            get => selectedStartListEntryItem;
            set
            {
                if ((value != null) && SetProperty(ref selectedStartListEntryItem, value))
                {
                    OnPropertyChanged(nameof(StartPosition));
                    (RemoveFromRosterCommand as RelayCommand)?.NotifyCanExecuteChanged();
                }
            }
        }

        public int? StartPosition => selectedStartListEntryItem?.Position;

        public ICommand AddToRosterCommand { get; }

        public ICommand RemoveFromRosterCommand { get; }

        public StartListViewModel(IFileStorage storage, ICompetitionService competitions,
            IStartListService startLists, INavigationService navigation)
        {
            clubMemberRepository = storage.GetRepository<IClubMemberRepository>();
            wheelRepository = storage.GetRepository<IWheelRepository>();
            disciplineRepository = storage.GetRepository<IDisciplineRepository>();
            startListService = startLists;
            competitionService = competitions;

            DisciplinesAndWheelSizes = new ObservableCollection<DisciplineSelectionItem>(
                competitions.Disciplines.Select(x => new DisciplineSelectionItem(x)));
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

                selectedStartListEntryItem = null;
                OnPropertyChanged(nameof(StartPosition));
                (RemoveFromRosterCommand as RelayCommand)?.NotifyCanExecuteChanged();
            };
            foreach (var item in DisciplinesAndWheelSizes)
            {
                item.PropertyChanged += OnDisciplineItemPropertyChanged;
            }

            selectedAthlete = startListService.ActiveAthlete;
            OnPropertyChanged(nameof(SelectedAthlete));
            selectedCoach = startListService.ActiveCoach;
            OnPropertyChanged(nameof(SelectedCoach));

            AddToRosterCommand = new RelayCommand(() =>
            {
                var disciplineAndWheelsSelection = DisciplinesAndWheelSizes
                    .Where(x => x.IsSelected && x.WheelSize.HasValue);

                try
                {
                    foreach (DisciplineSelectionItem disciplines in disciplineAndWheelsSelection)
                    {
                        startListService.AddToRoster(disciplines.Discipline.Id, disciplines.WheelSize!.Value);
                    }
                }
                catch (ArgumentNullException nullException)
                {
                    var message = new StringBuilder();
                    message.AppendLine(nullException.Message);
                    message.AppendLine();
                    message.AppendLine("Please select an athlete and a coach to make a new entry in the roster.");
                    navigation.ShowMessageBox(message.ToString(),
                        "Athlete, coach or both not selected",
                        MessageBoxButton.OK,
                        MessageBoxImage.Hand);
                }
                catch (InvalidOperationException forbiddenException)
                {
                    navigation.ShowMessageBox(forbiddenException.Message,
                        "Invalid operation",
                        MessageBoxButton.OK,
                        MessageBoxImage.Stop);
                }
                catch (Exception ex)
                {
                    var message = new StringBuilder();
                    message.AppendLine("An unexpected error occurred when making a new roster entry!");
                    message.AppendLine("Error message:");
                    message.AppendLine(ex.Message);
                    navigation.ShowMessageBox(message.ToString(),
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }

                OnPropertyChanged(nameof(Roster));
            }, () =>
            {
                return (SelectedAthlete != null)
                    && (SelectedCoach != null)
                    && DisciplinesAndWheelSizes.Any(x => x.IsSelected && x.WheelSize.HasValue);
            });

            RemoveFromRosterCommand = new RelayCommand(() =>
            {
                startListService.RemoveFromRoster(selectedStartListEntryItem!.StartListEntryId);
                OnPropertyChanged(nameof(Roster));

                selectedStartListEntryItem = null;
                OnPropertyChanged(nameof(StartPosition));
            }, () =>
            {
                return (selectedStartListEntryItem != null);
            });
        }

        private void OnDisciplineItemPropertyChanged(object? _, PropertyChangedEventArgs args)
        {
            if (args.PropertyName is nameof(DisciplineSelectionItem.IsSelected)
                or nameof(DisciplineSelectionItem.WheelSize))
            {
                OnPropertyChanged(nameof(Roster));
                (AddToRosterCommand as RelayCommand)?.NotifyCanExecuteChanged();

                selectedStartListEntryItem = null;
                OnPropertyChanged(nameof(StartPosition));
                (RemoveFromRosterCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }
    }
}
