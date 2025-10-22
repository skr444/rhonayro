using System;
using System.Collections.Generic;

using CommunityToolkit.Mvvm.ComponentModel;

using RhonAyro.Client.Desktop.Ui.Services;
using RhonAyro.Infrastructure.Storage.Api;

namespace RhonAyro.Client.Desktop.Ui.ViewModels
{
    internal sealed class CompetitionViewModel : ObservableObject
    {
        private readonly IClubMemberRepository clubMemberRepository;
        private readonly IDisciplineRepository disciplineRepository;
        private readonly IWheelRepository wheelRepository;
        private readonly ICompetitionService competitionService;
        private readonly IStartListService startListService;
        private Guid activeDisciplineId;

        public IEnumerable<StartListEntryItemViewModel> Roster
        {
            get
            {
                return null;
            }
        }

        public CompetitionViewModel(IFileStorage storage, ICompetitionService competitions, IStartListService startLists)
        {
            clubMemberRepository = storage.GetRepository<IClubMemberRepository>();
            disciplineRepository = storage.GetRepository<IDisciplineRepository>();
            wheelRepository = storage.GetRepository<IWheelRepository>();
            startListService = startLists;
            competitionService = competitions;

            activeDisciplineId = Guid.Parse("dddddddd-0000-0000-0000-d00000000001");
        }
    }
}
