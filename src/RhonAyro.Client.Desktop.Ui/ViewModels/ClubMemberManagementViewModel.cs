using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using RhonAyro.Common.Data.ScoreKeeping;
using RhonAyro.Infrastructure.Storage.Api;

namespace RhonAyro.Client.Desktop.Ui.ViewModels
{
    internal sealed class ClubMemberManagementViewModel : ObservableObject
    {
        #region Types

        internal sealed class ClubMemberViewModel : ObservableObject
        {
            public ClubMember Model { get; }

            public Guid Id => Model.Id;

            public DateTime Created => Model.Created;

            public DateTime Modified => Model.Modified;

            public string? FirstName => Model.FirstName;

            public string? LastName => Model.LastName;

            public ICollection<RoleType> Roles => Model.Roles;

            public ClubMemberViewModel(ClubMember member)
            {
                Model = member;
            }
        }

        #endregion Types

        private readonly IClubMemberRepository clubMemberRepository;

        public ObservableCollection<ClubMemberViewModel> Members { get; }

        public ClubMemberManagementViewModel(IFileStorage fileStorage)
        {
            clubMemberRepository = fileStorage.GetRepository<IClubMemberRepository>();
            Members = [];
        }

        public ICommand LoadCommand => new RelayCommand(() =>
        {
            Members.Clear();
            foreach (ClubMember member in clubMemberRepository.All())
            {
                Members.Add(new ClubMemberViewModel(member));
            }
        });

        public ICommand SaveRowCommand => new RelayCommand<object?>(row =>
        {
            if (row is ClubMemberViewModel member)
            {
                clubMemberRepository.AddOrUpdate(member.Model);
            }
        });

        public ICommand DeleteCommand => new RelayCommand<ClubMemberViewModel?>(vm =>
        {
            if (vm is null)
            {
                return;
            }

            Members.Remove(vm);
            clubMemberRepository.Delete(vm.Id);
        });
    }
}
