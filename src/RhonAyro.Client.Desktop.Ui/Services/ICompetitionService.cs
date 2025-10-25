using System;
using System.Collections.Generic;

using RhonAyro.Common.Data.ScoreKeeping;

namespace RhonAyro.Client.Desktop.Ui.Services
{
    internal interface ICompetitionService
    {
        Guid ActiveCompetitionId { get; }
        Competition ActiveCompetition { get; }
        ClubMember? ActiveHeadJuror { get; }
        ClubMember? ActiveManager { get; }
        IEnumerable<Discipline> Disciplines { get; }
        void SetActiveCompetition(Guid id);
        void NewActiveCompetition();
        void RemoveActiveCompetition();
        void SetHeadJuror(Guid id);
        void SetManager(Guid id);
        Discipline? GetDiscipline(Guid id);
        Discipline? GetDiscipline(string name);
        void AddDisciplineSession(DisciplineSession session);
    }
}
