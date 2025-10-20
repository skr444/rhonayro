using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly IViewStateRepository viewStateRepository;
        private readonly IClubMemberRepository clubMemberRepository;
        private Competition activeCompetition;

        public Guid ActiveCompetitionId => activeCompetition.Id;

        public Competition ActiveCompetition => new Competition
        {
            Id = activeCompetition.Id,
            Created = activeCompetition.Created,
            Modified = activeCompetition.Modified,
            EventStart = activeCompetition.EventStart,
            DisciplineSessions = [.. activeCompetition.DisciplineSessions],
            HeadJuror = activeCompetition.HeadJuror,
            CompetitionManager = activeCompetition.CompetitionManager
        };

        public ClubMember? ActiveHeadJuror
        {
            get
            {
                if (clubMemberRepository.TryGet(activeCompetition.HeadJuror, out ClubMember? headJuror))
                {
                    return headJuror;
                }

                return null;
            }
        }

        public ClubMember? ActiveManager
        {
            get
            {
                if (clubMemberRepository.TryGet(activeCompetition.CompetitionManager, out ClubMember? manager))
                {
                    return manager;
                }

                return null;
            }
        }

        public IEnumerable<Discipline> Disciplines { get; }

        public CompetitionService(IFileStorage storage)
        {
            competitionRepository = storage.GetRepository<ICompetitionRepository>();
            disciplineRepository = storage.GetRepository<IDisciplineRepository>();
            viewStateRepository = storage.GetRepository<IViewStateRepository>();
            clubMemberRepository = storage.GetRepository<IClubMemberRepository>();

            List<Discipline> disciplines = [.. disciplineRepository.All()];
            if (!disciplines.Any())
            {
                disciplines = new List<Discipline>
                {
                    new Discipline { Name = "Straight basic" },
                    new Discipline { Name = "Straight advanced" },
                    new Discipline { Name = "Jump" },
                    new Discipline { Name = "Spiral" },
                    new Discipline { Name = "Pair" },
                };

                foreach (Discipline discipline in disciplines)
                {
                    disciplineRepository.AddOrUpdate(discipline);
                }
            }
            Disciplines = new ReadOnlyCollection<Discipline>(disciplines);

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

        public void SetHeadJuror(Guid id)
        {
            activeCompetition.HeadJuror = id;
            competitionRepository.AddOrUpdate(activeCompetition);
        }

        public void SetManager(Guid id)
        {
            activeCompetition.CompetitionManager = id;
            competitionRepository.AddOrUpdate(activeCompetition);
        }
    }
}
