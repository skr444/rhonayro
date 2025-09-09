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

            public string? FirstName
            {
                get => Model.FirstName;
                set
                {
                    Model.FirstName = value;
                    OnPropertyChanged();
                }
            }

            public string? LastName
            {
                get => Model.LastName;
                set
                {
                    Model.LastName = value;
                    OnPropertyChanged();
                }
            }

            public RoleType Roles
            {
                get => Model.Roles;
                set
                {
                    Model.Roles = value;
                    OnPropertyChanged();
                }
            }

            public bool IsAthlete
            {
                get => Model.Roles.HasFlag(RoleType.Athlete);
                set
                {
                    if (value)
                    {
                        Roles |= RoleType.Athlete;
                    }
                    else
                    {
                        Roles &= ~RoleType.Athlete;
                    }
                    OnPropertyChanged();
                }
            }

            public bool IsCoach
            {
                get => Model.Roles.HasFlag(RoleType.Coach);
                set
                {
                    if (value)
                    {
                        Roles |= RoleType.Coach;
                    }
                    else
                    {
                        Roles &= ~RoleType.Coach;
                    }
                    OnPropertyChanged();
                }
            }

            public bool IsJuror
            {
                get => Model.Roles.HasFlag(RoleType.Juror);
                set
                {
                    if (value)
                    {
                        Roles |= RoleType.Juror;
                    }
                    else
                    {
                        Roles &= ~RoleType.Juror;
                    }
                    OnPropertyChanged();
                }
            }

            public bool IsHeadJuror
            {
                get => Model.Roles.HasFlag(RoleType.HeadJuror);
                set
                {
                    if (value)
                    {
                        Roles |= RoleType.HeadJuror;
                    }
                    else
                    {
                        Roles &= ~RoleType.HeadJuror;
                    }
                    OnPropertyChanged();
                }
            }

            public string RolesDisplay => (Roles == RoleType.Unspecified)
                ? "-"
                : Roles.ToString();

            public ClubMemberViewModel() : this(new ClubMember())
            {
            }

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

        public ICommand DeleteCommand => new RelayCommand<object?>(row =>
        {
            if (row is not ClubMemberViewModel vm)
            {
                return;
            }

            Members.Remove(vm);
            clubMemberRepository.Delete(vm.Id);
        });
    }
}
