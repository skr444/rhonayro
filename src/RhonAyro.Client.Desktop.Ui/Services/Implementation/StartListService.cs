using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RhonAyro.Common.Data.ScoreKeeping;
using RhonAyro.Infrastructure.Storage.Api;
using RhonAyro.Infrastructure.Storage.File;

namespace RhonAyro.Client.Desktop.Ui.Services.Implementation
{
    internal sealed class StartListService : IStartListService
    {
        private readonly IStartListEntryRepository startListEntryRepository;
        private readonly IDisciplineSessionRepository disciplineSessionRepository;
        private readonly ICompetitionService competitionService;

        public IEnumerable<StartListEntry> Roster { get; }

        public StartListService(IFileStorage storage, ICompetitionService competitionService)
        {
            startListEntryRepository = storage.GetRepository<IStartListEntryRepository>();
            disciplineSessionRepository = storage.GetRepository<IDisciplineSessionRepository>();
            this.competitionService = competitionService;
        }
    }
}
