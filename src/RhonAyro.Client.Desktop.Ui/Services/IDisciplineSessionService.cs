using System;
using System.Collections.Generic;

using RhonAyro.Common.Data.ScoreKeeping;

namespace RhonAyro.Client.Desktop.Ui.Services
{
    internal interface IDisciplineSessionService
    {
        IEnumerable<Discipline> EnlistedDisciplines { get; }
        IEnumerable<StartListEntry> Roster { get; }
        StartListEntry? Current { get; }
        bool HasActiveSession { get; }
        void SetActiveDiscipline(Guid id);
        void StartSession();
        bool NextStartNumber();
        bool PreviousStartNumber();
        void CompleteSession();
    }
}
