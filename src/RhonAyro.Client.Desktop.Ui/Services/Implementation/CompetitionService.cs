using System;
using System.Linq;

using RhonAyro.Common.Data.ScoreKeeping;
using RhonAyro.Infrastructure.Storage.Api;

namespace RhonAyro.Client.Desktop.Ui.Services.Implementation
{
    internal sealed class CompetitionService : ICompetitionService
    {
        private const string ActiveCompetitionIdKey = "activeCompetitionId";

        private readonly ICompetitionRepository competitionRepository;
        private readonly IDisciplineRepository disciplineRepository;
        private readonly IStartListEntryRepository startListEntryRepository;
        private readonly IViewStateRepository viewStateRepository;
        private Competition activeCompetition;

        public Guid ActiveCompetitionId => activeCompetition.Id;

        public Competition ActiveCompetition => new Competition
        {
            Id = activeCompetition.Id,
            Created = activeCompetition.Created,
            Modified = activeCompetition.Modified,
            EventStart = activeCompetition.EventStart,
            DisciplineSessions = [.. activeCompetition.DisciplineSessions]
        };

        public CompetitionService(IFileStorage fileStorage)
        {
            competitionRepository = fileStorage.GetRepository<ICompetitionRepository>();
            disciplineRepository = fileStorage.GetRepository<IDisciplineRepository>();
            startListEntryRepository = fileStorage.GetRepository<IStartListEntryRepository>();
            viewStateRepository = fileStorage.GetRepository<IViewStateRepository>();

            if (   viewStateRepository.TryGet(ActiveCompetitionIdKey, out string? competitionIdValue)
                && Guid.TryParse(competitionIdValue, out Guid competitionId)
                && competitionRepository.TryGet(competitionId, out Competition? competition))
            {
                activeCompetition = competition!;
            }
            else
            {
                NewActiveCompetition();
            }
        }

        public void SetActiveCompetition(Guid id)
        {
            if (competitionRepository.TryGet(id, out Competition? competition))
            {
                activeCompetition = competition!;
                viewStateRepository.Save(ActiveCompetitionIdKey, activeCompetition.Id.ToString());
            }
        }

        public void NewActiveCompetition()
        {
            activeCompetition = new Competition();
            competitionRepository.AddOrUpdate(activeCompetition);
            viewStateRepository.Save(ActiveCompetitionIdKey, activeCompetition.Id.ToString());
        }

        public void RemoveActiveCompetition()
        {
            competitionRepository.Delete(activeCompetition.Id);
            if (competitionRepository.Count > 0)
            {
                activeCompetition = competitionRepository.All().OrderBy(x => Math.Abs(DateTime.Now.Subtract(x.EventStart).TotalSeconds)).First();
            }
            else
            {
                NewActiveCompetition();
            }
        }
    }
}
