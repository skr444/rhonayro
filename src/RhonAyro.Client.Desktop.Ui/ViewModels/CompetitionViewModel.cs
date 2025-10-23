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
        #region Types

        internal sealed class DisciplineItem : ObservableObject
        {
            public Discipline Discipline { get; }
            public string Name => Discipline.Name;

            public DisciplineItem(Discipline discipline)
            {
                Discipline = discipline;
            }
        }

        #endregion Types

        private readonly IClubMemberRepository clubMemberRepository;
        private readonly IDisciplineRepository disciplineRepository;
        private readonly IWheelRepository wheelRepository;
        private readonly ICompetitionService competitionService;
        private readonly IStartListService startListService;
        private readonly IDisciplineSessionService disciplineSessionService;
        private DisciplineItem selectedDiscipline;

        public IEnumerable<StartListEntryItemViewModel> Roster => disciplineSessionService.Roster.Select(x =>
            new StartListEntryItemViewModel(x, clubMemberRepository, disciplineRepository, wheelRepository));

        public ObservableCollection<DisciplineItem> DisciplineItems { get; } = new();

        public DisciplineItem SelectedDiscipline
        {
            get => selectedDiscipline;
            set
            {
                if ((value != null) && SetProperty(ref selectedDiscipline, value))
                {
                    OnPropertyChanged(nameof(Roster));
                }
            }
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
            var options = disciplineSessionService.Roster
                .GroupBy(r => r.DisciplineId)
                .Select(x => new { DiscplineId = x.Key, Count = x.Count() })
                .Where(x => x.Count > 0)
                .Select(x => GetDiscipline(x.DiscplineId))
                .Where(x => x != null)
                .Select(x => new DisciplineItem(x!))
                .ToList();

            // Update collection
            DisciplineItems.ReplaceWith(options); // helper extension: Clear + AddRange

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
