using CommunityToolkit.Mvvm.ComponentModel;

using RhonAyro.Common.Data.ScoreKeeping;

namespace RhonAyro.Client.Desktop.Ui.ViewModels
{
    internal sealed class DisciplineItemViewModel : ObservableObject
    {
        public Discipline Discipline { get; }
        public string Name => Discipline.Name;

        public DisciplineItemViewModel(Discipline discipline)
        {
            Discipline = discipline;
        }
    }
}
