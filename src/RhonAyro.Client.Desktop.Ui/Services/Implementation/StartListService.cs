using System;
using System.Collections.Generic;
using System.Linq;

using RhonAyro.Common.Data.ScoreKeeping;
using RhonAyro.Infrastructure.Storage.Api;

namespace RhonAyro.Client.Desktop.Ui.Services.Implementation
{
    internal sealed class StartListService : IStartListService
    {
        private const string ActiveAthleteIdKey = "activeAthleteId";
        private const string ActiveCoachIdKey = "activeCoachId";

        private readonly IStartListEntryRepository startListEntryRepository;
        private readonly IViewStateRepository viewStateRepository;
        private readonly IClubMemberRepository clubMemberRepository;
        private readonly IWheelRepository wheelRepository;
        private readonly ICompetitionService competitionService;

        public IEnumerable<StartListEntry> Roster
        {
            get
            {
                return startListEntryRepository.All(x => x.CompetitionId == competitionService.ActiveCompetitionId);
            }
        }

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

        public StartListService(IFileStorage storage, ICompetitionService competitionService)
        {
            startListEntryRepository = storage.GetRepository<IStartListEntryRepository>();
            viewStateRepository = storage.GetRepository<IViewStateRepository>();
            clubMemberRepository = storage.GetRepository<IClubMemberRepository>();
            wheelRepository = storage.GetRepository<IWheelRepository>();
            this.competitionService = competitionService;
        }

        public void SetActiveAthlete(Guid? id)
        {
            if (id == null)
            {
                viewStateRepository.Remove(ActiveAthleteIdKey);
            }
            else
            {
                viewStateRepository.Save(ActiveAthleteIdKey, id.ToString()!);
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
                viewStateRepository.Save(ActiveCoachIdKey, id.ToString()!);
            }
        }

        public void AddToRoster(Guid disciplineId, float wheelSize)
        {
            var entry = new StartListEntry
            {
                CompetitionId = competitionService.ActiveCompetitionId,
                DisciplineId = disciplineId,
                StartPosition = startListEntryRepository.All(x => x.DisciplineId == disciplineId).Count + 1
            };

            ArgumentNullException.ThrowIfNull(ActiveAthlete);
            entry.AthleteId = ActiveAthlete.Id;

            ArgumentNullException.ThrowIfNull(ActiveCoach);
            entry.CoachId = ActiveCoach.Id;

            Wheel? wheel = wheelRepository.All(x => x.Size == wheelSize).FirstOrDefault();
            if (wheel == null)
            {
                wheel = new Wheel { Size = wheelSize };
                wheelRepository.AddOrUpdate(wheel);
            }

            entry.WheelId = wheel.Id;

            if (startListEntryRepository
                    .All(x => (x.AthleteId == entry.AthleteId) && (x.DisciplineId == entry.DisciplineId)).Count > 0)
            {
                string discipline = competitionService.GetDiscipline(entry.DisciplineId.Value)!.Name;
                throw new InvalidOperationException(
                    $"Athlete '{ActiveAthlete.FullName}' is already listed for discipline '{discipline}'!");
            }

            startListEntryRepository.AddOrUpdate(entry);
        }

        public void RemoveFromRoster(Guid id)
        {
            startListEntryRepository.Delete(id);
        }
    }
}
