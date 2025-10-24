using System;
using System.Collections.Generic;

using RhonAyro.Common.Data.ScoreKeeping;

namespace RhonAyro.Client.Desktop.Ui.Services
{
    internal interface IDisciplineSessionService
    {
        IEnumerable<Discipline> EnlistedDisciplines { get; }
        IEnumerable<StartListEntry> Roster { get; }
        void SetActiveDiscipline(Guid id);
    }
}
