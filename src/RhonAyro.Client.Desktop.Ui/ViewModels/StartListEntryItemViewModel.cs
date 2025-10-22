using System;

using CommunityToolkit.Mvvm.ComponentModel;

using RhonAyro.Common.Data.ScoreKeeping;
using RhonAyro.Infrastructure.Storage.Api;

namespace RhonAyro.Client.Desktop.Ui.ViewModels
{
    internal sealed class StartListEntryItemViewModel : ObservableObject
    {
        private readonly StartListEntry model;
        private readonly IClubMemberRepository clubMemberRepository;
        private readonly IDisciplineRepository disciplineRepository;
        private readonly IWheelRepository wheelRepository;

        public Guid StartListEntryId => model.Id;

        public int Position => model.StartPosition;

        public string Athlete
        {
            get
            {
                if (clubMemberRepository.TryGet(model.AthleteId ?? Guid.Empty, out ClubMember? athlete))
                {
                    return athlete!.FullName;
                }

                return String.Empty;
            }
        }

        public string Discipline
        {
            get
            {
                if (disciplineRepository.TryGet(model.DisciplineId ?? Guid.Empty, out Discipline? discipline))
                {
                    return discipline!.Name;
                }

                return String.Empty;
            }
        }

        public float WheelSize
        {
            get
            {
                if (wheelRepository.TryGet(model.WheelId ?? Guid.Empty, out Wheel? wheel))
                {
                    return wheel!.Size;
                }

                return 0;
            }
        }

        public string Coach
        {
            get
            {
                if (clubMemberRepository.TryGet(model.CoachId ?? Guid.Empty, out ClubMember? coach))
                {
                    return coach!.FullName;
                }

                return String.Empty;
            }
        }

        public StartListEntryItemViewModel(StartListEntry item, IClubMemberRepository clubMembers,
            IDisciplineRepository disciplines, IWheelRepository wheels)
        {
            model = item;
            clubMemberRepository = clubMembers;
            disciplineRepository = disciplines;
            wheelRepository = wheels;
        }
    }
}
