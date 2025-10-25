using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using RhonAyro.Client.Desktop.Ui.Services;
using RhonAyro.Common.Data.ScoreKeeping;
using RhonAyro.Infrastructure.Storage.Api;
using RhonAyro.Client.Desktop.Ui.Extensions;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using RhonAyro.Client.Desktop.Ui.Navigation;

namespace RhonAyro.Client.Desktop.Ui.ViewModels
{
    internal sealed class CompetitionViewModel : ObservableObject
    {
        private readonly IClubMemberRepository clubMemberRepository;
        private readonly IDisciplineRepository disciplineRepository;
        private readonly IWheelRepository wheelRepository;
        private readonly IDisciplineSessionService disciplineSessionService;
        private DisciplineItemViewModel? selectedDiscipline;
        private bool hasNext;
        private bool hasPrevious;
        private int difficulty;
        private float scoreOne;
        private float scoreTwo;
        private float scoreThree;
        private float scoreFour;

        public ObservableCollection<DisciplineItemViewModel> DisciplineItems { get; } = [];

        public DisciplineItemViewModel? SelectedDiscipline
        {
            get => selectedDiscipline;
            set
            {
                if (   (value != null)
                    && SetProperty(ref selectedDiscipline, value)
                    && !disciplineSessionService.HasActiveSession)
                {
                    disciplineSessionService.SetActiveDiscipline(selectedDiscipline.Discipline.Id);
                    OnPropertyChanged(nameof(Roster));
                    OnPropertyChanged(nameof(IsSessionLocked));
                    OnPropertyChanged(nameof(StartNumber));
                    OnPropertyChanged(nameof(Athlete));
                    OnPropertyChanged(nameof(Coach));
                    OnPropertyChanged(nameof(WheelSize));
                    hasNext = true;
                    hasPrevious = false;
                    (NextPerformanceCommand as RelayCommand)?.NotifyCanExecuteChanged();
                    (PreviousPerformanceCommand as RelayCommand)?.NotifyCanExecuteChanged();
                }
            }
        }

        public bool IsSessionLocked => disciplineSessionService.HasActiveSession;

        public IEnumerable<StartListEntryItemViewModel> Roster => ConvertRoster(disciplineSessionService.Roster);

        public string StartNumber => disciplineSessionService.Current?.StartPosition.ToString() ?? String.Empty;

        public string Athlete
        {
            get
            {
                if (clubMemberRepository.TryGet(disciplineSessionService.Current?.AthleteId ?? Guid.Empty,
                    out ClubMember? member))
                {
                    return member!.FullName;
                }

                return String.Empty;
            }
        }

        public string Coach
        {
            get
            {
                if (clubMemberRepository.TryGet(disciplineSessionService.Current?.CoachId ?? Guid.Empty,
                    out ClubMember? member))
                {
                    return member!.FullName;
                }

                return String.Empty;
            }
        }

        public string WheelSize
        {
            get
            {
                if (wheelRepository.TryGet(disciplineSessionService.Current?.WheelId ?? Guid.Empty,
                    out Wheel? wheel))
                {
                    return wheel!.Size.ToString();
                }

                return String.Empty;
            }
        }

        public int Difficulty
        {
            get => difficulty;
            set
            {
                if (SetProperty(ref difficulty, value))
                {
                    OnPropertyChanged(nameof(FinalScore));
                }
            }
        }

        public float ScoreOne
        {
            get => scoreOne;
            set
            {
                if (SetProperty(ref scoreOne, value))
                {
                    OnPropertyChanged(nameof(FinalScore));
                }
            }
        }

        public float ScoreTwo
        {
            get => scoreTwo;
            set
            {
                if (SetProperty(ref scoreTwo, value))
                {
                    OnPropertyChanged(nameof(FinalScore));
                }
            }
        }

        public float ScoreThree
        {
            get => scoreThree;
            set
            {
                if (SetProperty(ref scoreThree, value))
                {
                    OnPropertyChanged(nameof(FinalScore));
                }
            }
        }

        public float ScoreFour
        {
            get => scoreFour;
            set
            {
                if (SetProperty(ref scoreFour, value))
                {
                    OnPropertyChanged(nameof(FinalScore));
                }
            }
        }

        public float FinalScore => CalculateScore(difficulty, scoreOne, scoreTwo, scoreThree, scoreFour);

        public string SessionControlButtonText => IsSessionLocked ? "Complete session" : "Start session";

        public ICommand StartSessionCommand { get; }
        public ICommand NextPerformanceCommand { get; }
        public ICommand PreviousPerformanceCommand { get; }

        public CompetitionViewModel(IFileStorage storage, IDisciplineSessionService disciplineSessions,
            INavigationService navigation)
        {
            clubMemberRepository = storage.GetRepository<IClubMemberRepository>();
            disciplineRepository = storage.GetRepository<IDisciplineRepository>();
            wheelRepository = storage.GetRepository<IWheelRepository>();
            disciplineSessionService = disciplineSessions;
            hasNext = true;
            hasPrevious = false;

            StartSessionCommand = new RelayCommand(() =>
            {
                try
                {
                    if (disciplineSessionService.HasActiveSession)
                    {
                        disciplineSessionService.CompleteSession();
                    }
                    else
                    {
                        disciplineSessionService.StartSession();
                    }

                    OnPropertyChanged(nameof(IsSessionLocked));
                    OnPropertyChanged(nameof(SessionControlButtonText));
                    OnPropertyChanged(nameof(StartNumber));
                    OnPropertyChanged(nameof(Athlete));
                    OnPropertyChanged(nameof(Coach));
                    OnPropertyChanged(nameof(WheelSize));
                    (NextPerformanceCommand as RelayCommand)?.NotifyCanExecuteChanged();
                    (PreviousPerformanceCommand as RelayCommand)?.NotifyCanExecuteChanged();
                }
                catch (Exception ex)
                {
                    navigation.ShowMessageBox(ex.Message, "Invalid operation",
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            });

            NextPerformanceCommand = new RelayCommand(() =>
            {
                hasNext = disciplineSessionService.NextStartNumber();
                hasPrevious = true;
                OnPropertyChanged(nameof(StartNumber));
                OnPropertyChanged(nameof(Athlete));
                OnPropertyChanged(nameof(Coach));
                OnPropertyChanged(nameof(WheelSize));
                (NextPerformanceCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (PreviousPerformanceCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }, () => IsSessionLocked && hasNext);

            PreviousPerformanceCommand = new RelayCommand(() =>
            {
                hasPrevious = disciplineSessionService.PreviousStartNumber();
                hasNext = true;
                OnPropertyChanged(nameof(StartNumber));
                OnPropertyChanged(nameof(Athlete));
                OnPropertyChanged(nameof(Coach));
                OnPropertyChanged(nameof(WheelSize));
                (NextPerformanceCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (PreviousPerformanceCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }, () => IsSessionLocked && hasPrevious);

            RefreshDisciplineSelections();
        }

        private void RefreshDisciplineSelections()
        {
            // Update collection
            DisciplineItems.ReplaceWith(ConvertDisciplines(disciplineSessionService.EnlistedDisciplines));

            // Preserve selection if still valid
            if (selectedDiscipline is null ||
                !DisciplineItems.Any(o => o.Discipline == selectedDiscipline.Discipline))
            {
                SelectedDiscipline = DisciplineItems.FirstOrDefault();
            }
        }

        private IEnumerable<StartListEntryItemViewModel> ConvertRoster(IEnumerable<StartListEntry> items)
        {
            foreach (StartListEntry item in items)
            {
                yield return new StartListEntryItemViewModel(item,
                    clubMemberRepository,
                    disciplineRepository,
                    wheelRepository);
            }
        }

        private static IEnumerable<DisciplineItemViewModel> ConvertDisciplines(IEnumerable<Discipline> items)
        {
            foreach (Discipline item in items)
            {
                yield return new DisciplineItemViewModel(item);
            }
        }

        private static float CalculateScore(int difficulty, params float[] scoreValues)
        {
            ArgumentNullException.ThrowIfNull(scoreValues);
            if (scoreValues.Length != 4)
            {
                throw new ArgumentException($"Expecting exactly four score values, but got {scoreValues.Length}.",
                    nameof(scoreValues));
            }

            float lowest = scoreValues.Min();
            float highest = scoreValues.Max();

            var eligibleScores = scoreValues.Except([lowest, highest]);
            var average = eligibleScores.Average();
            return average * difficulty;
        }
    }
}
