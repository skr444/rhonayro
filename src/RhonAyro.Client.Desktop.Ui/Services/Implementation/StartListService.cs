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
        private const string ActiveAthleteIdKey = "activeAthleteId";
        private const string ActiveCoachIdKey = "activeCoachId";

        private readonly IStartListEntryRepository startListEntryRepository;
        private readonly IDisciplineSessionRepository disciplineSessionRepository;
        private readonly IViewStateRepository viewStateRepository;
        private readonly IClubMemberRepository clubMemberRepository;
        private readonly ICompetitionService competitionService;
        private IList<DisciplineSession> disciplineSessions;
        private IList<StartListEntry> startListEntries;
        private ClubMember? activeAthlete;
        private ClubMember? activeCoach;

        public IEnumerable<StartListEntry> Roster { get; }

        public ClubMember? ActiveAthlete
        {
            get
            {
                if (   viewStateRepository.TryGet(ActiveAthleteIdKey, out string? athleteIdValue)
                    && Guid.TryParse(athleteIdValue, out Guid athleteId)
                    && clubMemberRepository.TryGet(athleteId, out ClubMember? athlete))
                {
                    return athlete;
                }

                return null;
            }
        }

        public ClubMember? ActiveCoach
        {
            get
            {
                if (viewStateRepository.TryGet(ActiveCoachIdKey, out string? coachIdValue)
                    && Guid.TryParse(coachIdValue, out Guid coachId)
                    && clubMemberRepository.TryGet(coachId, out ClubMember? coach))
                {
                    return coach;
                }

                return null;
            }
        }

        public IEnumerable<StartListEntry> StartListEntries
        {
            get
            {
                return startListEntryRepository.All(x => x.CompetitionId == competitionService.ActiveCompetitionId);
            }
        }

        public StartListService(IFileStorage storage, ICompetitionService competitionService)
        {
            startListEntryRepository = storage.GetRepository<IStartListEntryRepository>();
            disciplineSessionRepository = storage.GetRepository<IDisciplineSessionRepository>();
            viewStateRepository = storage.GetRepository<IViewStateRepository>();
            clubMemberRepository = storage.GetRepository<IClubMemberRepository>();
            this.competitionService = competitionService;

            disciplineSessions = new List<DisciplineSession>();
            startListEntries = new List<StartListEntry>();
        }

        public void SetActiveAthlete(Guid? id)
        {
            if (id == null)
            {
                viewStateRepository.Remove(ActiveAthleteIdKey);
            }
            else
            {
                viewStateRepository.Save(ActiveAthleteIdKey, id!.ToString());
            }
        }

        public void SetActiveCoach(Guid? id)
        {
            if (id == null)
            {
                viewStateRepository.Remove(ActiveCoachIdKey);
            }
            else
            {
                viewStateRepository.Save(ActiveCoachIdKey, id!.ToString());
            }
        }
    }
}
