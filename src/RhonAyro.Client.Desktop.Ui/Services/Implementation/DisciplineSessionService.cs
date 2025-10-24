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
        #region Types

        private sealed class ActiveStartListEntries
        {
            private readonly Discipline discipline;
            private readonly StartListEntry[] entries;
            public int Index { get; private set; }
            public StartListEntry Current
            {
                get
                {
                    if ((Index < 0) && (Index >= entries.Length))
                    {
                        throw new ArgumentOutOfRangeException(nameof(Index), $"Current index '{Index}' is outside the range of current entries [{String.Join(", ", entries.Select(x => x.ToString()))}].");
                    }

                    return entries[Index];
                }
            }
            public Discipline Discipline => discipline;
            public IEnumerable<StartListEntry> Entries => entries;

            public ActiveStartListEntries(Discipline discipline, IEnumerable<StartListEntry> items)
            {
                ArgumentNullException.ThrowIfNull(discipline);
                ArgumentNullException.ThrowIfNull(items);

                this.discipline = discipline;
                entries = [.. items];
                Index = 0;
            }

            public bool MoveNext()
            {
                var index = Index + 1;

                if (index >= entries.Length)
                {
                    return false;
                }

                Index = index;
                return true;
            }

            public bool GoBack()
            {
                var index = Index - 1;

                if (index < 0)
                {
                    return false;
                }

                Index = index;
                return true;
            }
        }

        private sealed class ActiveSession
        {
            private readonly IPerformanceEntryRepository performanceEntryRepository;
            private ActiveStartListEntries roster;
            public DisciplineSession Session { get; }
            public ScoreBoard Score { get; }
            public PerformanceEntry? Performance { get; private set; }
            public ActiveSession(IPerformanceEntryRepository performanceEntries, ActiveStartListEntries roster, DisciplineSession session, ScoreBoard score)
            {
                performanceEntryRepository = performanceEntries;
                this.roster = roster;
                Session = session;
                Score = score;

                Performance = GetPerformanceEntryByStartListEntryId(performanceEntryRepository, roster.Current.Id);
            }

            public bool MoveNext()
            {
                if (roster.MoveNext())
                {
                    Performance = GetPerformanceEntryByStartListEntryId(performanceEntryRepository,
                        roster.Current.Id);

                    return true;
                }

                return false;
            }

            public bool GoBack()
            {
                if (roster.GoBack())
                {
                    Performance = GetPerformanceEntryByStartListEntryId(performanceEntryRepository,
                        roster.Current.Id);

                    return true;
                }

                return false;
            }

            private static PerformanceEntry GetPerformanceEntryByStartListEntryId(IPerformanceEntryRepository repo,
                Guid id)
            {
                var entries = repo.All(x => x.StartListEntryId == id);
                if (entries.Count == 1)
                {
                    return entries.First();
                }
                else if (entries.Count == 0)
                {
                    var entry = new PerformanceEntry
                    {
                        StartListEntryId = id
                    };
                    repo.AddOrUpdate(entry);
                    return entry;
                }
                else
                {
                    throw new InvalidOperationException($"Multiple performance entries available for current start list id '{id}'");
                }
            }
        }

        #endregion Types

        private const string ActiveDisciplineIdKey = "activeDisciplineId";
        private const string ActiveSessionIdKey = "activeSessionId";
        private const string ActiveStartListEntryIdKey = "ActiveStartListEntryId";
        private const string ActivePerformanceEntryIdKey = "activePerformanceEntryId";

        private readonly IViewStateRepository viewStateRepository;
        private readonly IDisciplineSessionRepository disciplineSessionRepository;
        private readonly IPerformanceEntryRepository performanceEntryRepository;
        private readonly IScoreEntryRepository scoreEntryRepository;
        private readonly IStartListService startListService;
        private readonly ICompetitionService competitionService;
        private ActiveSession? activeDisciplineSession;
        private ActiveStartListEntries? activeStartListEntries;
        private PerformanceEntry? activePerformanceEntry;
        private ScoreBoard? activeScoreBoard;
        private ScoreEntry? activeScoreEntry;

        public IEnumerable<Discipline> EnlistedDisciplines => startListService.EnlistedDisciplines;

        public IEnumerable<StartListEntry> Roster => (activeStartListEntries != null)
            ? startListService.Roster
                .Where(x => x.DisciplineId == activeStartListEntries.Discipline.Id)
                .OrderBy(x => x.StartPosition)
            : [];

        public bool HasActiveSession => (activeDisciplineSession != null);

        public DisciplineSessionService(IFileStorage storage, IStartListService startLists,
            ICompetitionService competitions)
        {
            viewStateRepository = storage.GetRepository<IViewStateRepository>();
            disciplineSessionRepository = storage.GetRepository<IDisciplineSessionRepository>();
            performanceEntryRepository = storage.GetRepository<IPerformanceEntryRepository>();
            startListService = startLists;
            competitionService = competitions;

            activeStartListEntries = null;
            if (viewStateRepository.TryGetAs(ActiveSessionIdKey, out Guid sessionId)
                && disciplineSessionRepository.TryGet(sessionId, out DisciplineSession? session))
            {
                activeDisciplineSession = new ActiveSession(performanceEntryRepository, Roster, session);
                activeStartListEntries = new ActiveStartListEntries(
                    competitionService.GetDiscipline(activeDisciplineSession!.DisciplineId)!, Roster);
            }
        }

        public void SetActiveDiscipline(Guid id)
        {
            if (HasActiveSession)
            {
                throw new InvalidOperationException("Cannot change discipline during active session.");
            }

            var discipline = competitionService.GetDiscipline(id);
            if (discipline != null)
            {
                activeStartListEntries = new ActiveStartListEntries(discipline, Roster);
            }
            else
            {
                throw new ArgumentException($"No discipline with id '{id}' exists.", nameof(id));
            }
        }

        public void StartSession()
        {
            if (HasActiveSession)
            {
                throw new InvalidOperationException("Session already active.");
            }
            if (activeStartListEntries == null)
            {
                throw new InvalidOperationException("No discipline selected.");
            }

            activeDisciplineSession = new DisciplineSession
            {
                DisciplineId = activeStartListEntries.Discipline.Id,
                
            };
        }

        public void NextStartNumber()
        {
            if (!HasActiveSession)
            {
                throw new InvalidOperationException("Session must be active to advance position.");
            }
        }

        public void PreviousStartNumber()
        {
            if (!HasActiveSession)
            {
                throw new InvalidOperationException("Session must be active to move back position.");
            }
        }

        public void CompleteSession()
        {
            if (!HasActiveSession)
            {
                throw new InvalidOperationException("No active session.");
            }
        }
    }
}
