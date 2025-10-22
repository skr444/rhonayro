using System;
using System.Collections.Generic;

using RhonAyro.Common.Data.ScoreKeeping;

namespace RhonAyro.Client.Desktop.Ui.Services
{
    internal interface IStartListService
    {
        IEnumerable<StartListEntry> Roster { get; }
        ClubMember? ActiveAthlete { get; }
        ClubMember? ActiveCoach { get; }
        void SetActiveAthlete(Guid? id);
        void SetActiveCoach(Guid? id);
        void AddToRoster(Guid disciplineId, float wheelSize);
        void RemoveFromRoster(Guid id);
        StartListEntry? MoveRosterEntryUp(Guid entryId, Guid disciplineId);
        StartListEntry? MoveRosterEntryDown(Guid entryId, Guid disciplineId);
    }
}
