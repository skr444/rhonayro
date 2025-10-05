using System;

using RhonAyro.Common.Data.ScoreKeeping;

namespace RhonAyro.Client.Desktop.Ui.Services
{
    internal interface ICompetitionService
    {
        Guid ActiveCompetitionId { get; }
        Competition ActiveCompetition { get; }
        ClubMember? ActiveHeadJuror { get; }
        ClubMember? ActiveManager { get; }
        void SetActiveCompetition(Guid id);
        void NewActiveCompetition();
        void RemoveActiveCompetition();
        void SetHeadJuror(Guid id);
        void SetManager(Guid id);
    }
}
