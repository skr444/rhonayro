using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RhonAyro.Common.Data.ScoreKeeping;
using RhonAyro.Infrastructure.Storage.Api;

namespace RhonAyro.Client.Desktop.Ui.Services.Implementation
{
    internal sealed class DisciplineSessionService : IDisciplineSessionService
    {
        private const string ActiveSessionIdKey = "activeDisciplineSessionId";

        private readonly IViewStateRepository viewStateRepository;
        private readonly IDisciplineSessionRepository disciplineSessionRepository;
        private readonly IStartListService startListService;
        private readonly ICompetitionService competitionService;
        private Guid activeDisciplineId;

        public IEnumerable<StartListEntry> Roster => startListService.Roster.Where(x =>
            x.DisciplineId == activeDisciplineId);

        public Discipline ActiveDiscipline => competitionService.GetDiscipline(activeDisciplineId)!;

        public DisciplineSessionService(IFileStorage storage, IStartListService startLists,
            ICompetitionService competitions)
        {
            viewStateRepository = storage.GetRepository<IViewStateRepository>();
            disciplineSessionRepository = storage.GetRepository<IDisciplineSessionRepository>();
            startListService = startLists;
            competitionService = competitions;

            if (viewStateRepository.TryGet(ActiveSessionIdKey, out string? disciplineIdValue)
                && Guid.TryParse(disciplineIdValue, out Guid disciplineId))
            {
                activeDisciplineId = disciplineId;
            }
            else
            {
                activeDisciplineId = Guid.Parse("dddddddd-0000-0000-0000-d00000000001");
            }
        }

        public void SetActiveDiscipline(Guid id)
        {
            var discipline = competitionService.GetDiscipline(id);
            if (discipline != null)
            {
                activeDisciplineId = discipline.Id;
                viewStateRepository.Save(ActiveSessionIdKey, activeDisciplineId.ToString());
            }
        }
    }
}
