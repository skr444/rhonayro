using System;

using RhonAyro.Common.Data.ScoreKeeping;

namespace RhonAyro.Client.Desktop.Ui.Services
{
    internal interface ICompetitionService
    {
        Guid ActiveCompetitionId { get; }
        Competition ActiveCompetition { get; }
        void SetActiveCompetition(Guid id);
        void NewActiveCompetition();
        void RemoveActiveCompetition();
    }
}
