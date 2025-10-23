using System;
using System.Collections.Generic;
using System.Linq;

using RhonAyro.Common.Data.ScoreKeeping;
using RhonAyro.Infrastructure.Storage.Api;
using RhonAyro.Infrastructure.Storage.Extensions;

namespace RhonAyro.Client.Desktop.Ui.Services.Implementation
{
    internal sealed class DisciplineSessionService : IDisciplineSessionService
    {
        private const string ActiveDisciplineIdKey = "activeDisciplineId";
        private const string LockedSessionIdKey = "lockedSessionId";

        private readonly IViewStateRepository viewStateRepository;
        private readonly IDisciplineSessionRepository disciplineSessionRepository;
        private readonly IStartListService startListService;
        private readonly ICompetitionService competitionService;
        private Guid? activeDisciplineId;
        private DisciplineSession? activeDisciplineSession;
        private PerformanceEntry? activePerformanceEntry;

        public IEnumerable<StartListEntry> Roster => startListService.Roster.Where(x =>
            x.DisciplineId == activeDisciplineId);

        public Discipline ActiveDiscipline => startListService.GetEnlistedDiscipline(activeDisciplineId);

        public DisciplineSessionService(IFileStorage storage, IStartListService startLists,
            ICompetitionService competitions)
        {
            viewStateRepository = storage.GetRepository<IViewStateRepository>();
            disciplineSessionRepository = storage.GetRepository<IDisciplineSessionRepository>();
            startListService = startLists;
            competitionService = competitions;

            if (viewStateRepository.TryGetAs(LockedSessionIdKey, out Guid? disciplineId))
            {
                activeDisciplineId = disciplineId;
            }
            else
            {
                var discipline = startListService.EnlistedDisciplines.FirstOrDefault();
                if (discipline != null)
                {
                    activeDisciplineId = discipline.Id;
                }
                else
                {
                    activeDisciplineId = Guid.Parse("dddddddd-0000-0000-0000-d00000000001");
                }
            }
        }

        public void SetActiveDiscipline(Guid id)
        {
            var discipline = competitionService.GetDiscipline(id);
            if (discipline != null)
            {
                activeDisciplineId = discipline.Id;
                viewStateRepository.Save(LockedSessionIdKey, discipline.Id.ToString());
            }
        }
    }
}
