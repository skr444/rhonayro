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

            public ActiveStartListEntries(Discipline discipline, IStartListService startListService)
            {
                ArgumentNullException.ThrowIfNull(discipline);
                ArgumentNullException.ThrowIfNull(startListService);

                this.discipline = discipline;
                entries = [.. startListService.Roster
                    .Where(x => x.DisciplineId == discipline.Id)
                    .OrderBy(x => x.StartPosition)];
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
            private readonly Func<StartListEntry> currentStartListEntry;
            private readonly Func<bool> moveNext;
            private readonly Func<bool> goBack;
            public DisciplineSession Session { get; private set; }
            public ScoreBoard Score { get; private set; }
            public PerformanceEntry? Performance { get; private set; }
            public ActiveSession(IPerformanceEntryRepository performanceEntries, DisciplineSession session,
                Func<StartListEntry> getCurrentStartListEntry, Func<bool> nextStartListEntry,
                Func<bool> previousStartListEntry)
            {
                performanceEntryRepository = performanceEntries;
                Session = session;
                currentStartListEntry = getCurrentStartListEntry;
                moveNext = nextStartListEntry;
                goBack = previousStartListEntry;
                Score = new ScoreBoard();

                Performance = GetPerformanceEntryByStartListEntryId(performanceEntryRepository,
                    currentStartListEntry().Id);
            }

            public bool MoveNext()
            {
                if (moveNext())
                {
                    Performance = GetPerformanceEntryByStartListEntryId(performanceEntryRepository,
                        currentStartListEntry().Id);

                    return true;
                }

                return false;
            }

            public bool GoBack()
            {
                if (goBack())
                {
                    Performance = GetPerformanceEntryByStartListEntryId(performanceEntryRepository,
                        currentStartListEntry().Id);

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
        private const string ActiveStartListEntryIdKey = "activeStartListEntryId";
        private const string ActivePerformanceEntryIdKey = "activePerformanceEntryId";

        private readonly IViewStateRepository viewStateRepository;
        private readonly IDisciplineSessionRepository disciplineSessionRepository;
        private readonly IPerformanceEntryRepository performanceEntryRepository;
        private readonly IScoreEntryRepository scoreEntryRepository;
        private readonly IScoreBoardRepository scoreBoardRepository;
        private readonly IStartListService startListService;
        private readonly ICompetitionService competitionService;
        private ActiveStartListEntries? activeStartListEntries;
        private ActiveSession? activeDisciplineSession;
        private PerformanceEntry? activePerformanceEntry;
        private ScoreBoard? activeScoreBoard;
        private ScoreEntry? activeScoreEntry;

        public IEnumerable<Discipline> EnlistedDisciplines => startListService.EnlistedDisciplines;

        public IEnumerable<StartListEntry> Roster => (activeStartListEntries != null)
            ? activeStartListEntries.Entries
            : [];

        public StartListEntry? Current => activeStartListEntries?.Current;

        public bool HasActiveSession => (activeDisciplineSession != null);

        public DisciplineSessionService(IFileStorage storage, IStartListService startLists,
            ICompetitionService competitions)
        {
            viewStateRepository = storage.GetRepository<IViewStateRepository>();
            disciplineSessionRepository = storage.GetRepository<IDisciplineSessionRepository>();
            performanceEntryRepository = storage.GetRepository<IPerformanceEntryRepository>();
            scoreBoardRepository = storage.GetRepository<IScoreBoardRepository>();
            startListService = startLists;
            competitionService = competitions;

            activeStartListEntries = null;
            activeDisciplineSession = null;
            if (viewStateRepository.TryGetAs(ActiveSessionIdKey, out Guid sessionId)
                && disciplineSessionRepository.TryGet(sessionId, out DisciplineSession? session))
            {
                activeStartListEntries = new ActiveStartListEntries(
                    competitionService.GetDiscipline(session!.DisciplineId)!, startListService);
                activeDisciplineSession = new ActiveSession(performanceEntryRepository, session,
                    () => activeStartListEntries.Current, activeStartListEntries.MoveNext,
                    activeStartListEntries.GoBack);
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
                activeStartListEntries = new ActiveStartListEntries(discipline, startListService);
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

            activeDisciplineSession = new ActiveSession(performanceEntryRepository,
                GetDisciplineSession(competitionService, disciplineSessionRepository,
                    activeStartListEntries.Discipline.Id),
                () => activeStartListEntries.Current, activeStartListEntries.MoveNext, activeStartListEntries.GoBack);
            viewStateRepository.Save(ActiveSessionIdKey, activeDisciplineSession.Session.Id.ToString());
        }

        public bool NextStartNumber()
        {
            if (!HasActiveSession)
            {
                throw new InvalidOperationException("Session must be active to advance position.");
            }

            return activeDisciplineSession!.MoveNext();
        }

        public bool PreviousStartNumber()
        {
            if (!HasActiveSession)
            {
                throw new InvalidOperationException("Session must be active to move back position.");
            }

            return activeDisciplineSession!.GoBack();
        }

        public void CompleteSession()
        {
            if (!HasActiveSession)
            {
                throw new InvalidOperationException("No active session.");
            }

            activeDisciplineSession = null;
            viewStateRepository.Remove(ActiveSessionIdKey);
        }

        private static DisciplineSession GetDisciplineSession(ICompetitionService competitionService,
            IDisciplineSessionRepository repo, Guid disciplineId)
        {
            var sessions = new List<DisciplineSession>();
            foreach (var sessionId in competitionService.ActiveCompetition.DisciplineSessions)
            {
                if (repo.TryGet(sessionId, out DisciplineSession? session))
                {
                    sessions.Add(session!);
                }
            }

            var sessionCandidate = sessions.FirstOrDefault(x => x.DisciplineId == disciplineId);
            if (sessionCandidate == null)
            {
                sessionCandidate = new DisciplineSession
                {
                    DisciplineId = disciplineId
                };
                repo.AddOrUpdate(sessionCandidate);
                competitionService.AddDisciplineSession(sessionCandidate);
            }

            return sessionCandidate;
        }
    }
}
