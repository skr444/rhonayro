using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using RhonAyro.Client.Desktop.Ui.Services;
using RhonAyro.Common.Data.ScoreKeeping;
using RhonAyro.Infrastructure.Storage.Api;
using RhonAyro.Client.Desktop.Ui.Extensions;

namespace RhonAyro.Client.Desktop.Ui.ViewModels
{
    internal sealed class CompetitionViewModel : ObservableObject
    {
        private readonly IClubMemberRepository clubMemberRepository;
        private readonly IDisciplineRepository disciplineRepository;
        private readonly IWheelRepository wheelRepository;
        private readonly ICompetitionService competitionService;
        private readonly IStartListService startListService;
        private readonly IDisciplineSessionService disciplineSessionService;
        private DisciplineItemViewModel selectedDiscipline;
        private bool isSessionLocked;
        private Guid? lockedSession;

        public ObservableCollection<DisciplineItemViewModel> DisciplineItems { get; } = [];

        public DisciplineItemViewModel SelectedDiscipline
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
                    lockedSession = selectedDiscipline.Discipline.Id;
                }
            }
        }

        public IEnumerable<StartListEntryItemViewModel> Roster => disciplineSessionService.Roster.Select(x =>
            new StartListEntryItemViewModel(x, clubMemberRepository, disciplineRepository, wheelRepository));

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

        public CompetitionViewModel(IFileStorage storage, ICompetitionService competitions, IStartListService startLists, IDisciplineSessionService disciplineSessions)
        {
            clubMemberRepository = storage.GetRepository<IClubMemberRepository>();
            disciplineRepository = storage.GetRepository<IDisciplineRepository>();
            wheelRepository = storage.GetRepository<IWheelRepository>();
            startListService = startLists;
            competitionService = competitions;
            disciplineSessionService = disciplineSessions;

            RefreshDisciplineSelections();
        }

        private void RefreshDisciplineSelections()
        {
            // Update collection
            DisciplineItems.ReplaceWith(startListService.EnlistedDisciplines
                .Select(x => new DisciplineItemViewModel(GetDiscipline(x.Id)!)));

            // Preserve selection if still valid
            if (selectedDiscipline is null ||
                !DisciplineItems.Any(o => o.Discipline == selectedDiscipline.Discipline))
            {
                SelectedDiscipline = DisciplineItems.FirstOrDefault();
            }

            Discipline? GetDiscipline(Guid? id)
            {
                if (disciplineRepository.TryGet(id ?? Guid.Empty, out Discipline? discipline))
                {
                    return discipline;
                }
                return null;
            }
        }
    }
}
