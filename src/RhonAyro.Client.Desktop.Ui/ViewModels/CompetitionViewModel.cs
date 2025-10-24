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

namespace RhonAyro.Client.Desktop.Ui.ViewModels
{
    internal sealed class CompetitionViewModel : ObservableObject
    {
        private readonly IClubMemberRepository clubMemberRepository;
        private readonly IDisciplineRepository disciplineRepository;
        private readonly IWheelRepository wheelRepository;
        private readonly IDisciplineSessionService disciplineSessionService;
        private DisciplineItemViewModel? selectedDiscipline;
        private bool isSessionLocked;

        public ObservableCollection<DisciplineItemViewModel> DisciplineItems { get; } = [];

        public DisciplineItemViewModel? SelectedDiscipline
        {
            get => selectedDiscipline;
            set
            {
                if ((value != null) && SetProperty(ref selectedDiscipline, value))
                {
                    disciplineSessionService.SetActiveDiscipline(selectedDiscipline.Discipline.Id);
                    OnPropertyChanged(nameof(Roster));
                }
            }
        }

        public bool IsSessionLocked
        {
            get => isSessionLocked;
            set
            {
                if (value && SetProperty(ref isSessionLocked, value))
                {
                }
            }
        }

        public IEnumerable<StartListEntryItemViewModel> Roster => ConvertRoster(disciplineSessionService.Roster);

        public string StartNumber
        {
            get
            {
                return "1";
            }
        }

        public string Athlete
        {
            get => "Pirmin Zurbrügg";
        }

        public string Coach
        {
            get => "Gaby";
        }

        public string WheelSize
        {
            get => "230";
        }

        public string SessionControlButtonText { get; private set; }

        public ICommand StartSessionCommand { get; }

        public CompetitionViewModel(IFileStorage storage, IDisciplineSessionService disciplineSessions)
        {
            clubMemberRepository = storage.GetRepository<IClubMemberRepository>();
            disciplineRepository = storage.GetRepository<IDisciplineRepository>();
            wheelRepository = storage.GetRepository<IWheelRepository>();
            disciplineSessionService = disciplineSessions;

            SessionControlButtonText = "Start session";

            StartSessionCommand = new RelayCommand(() =>
            {

            });

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
    }
}
