using RhonAyro.Common.Data.ScoreKeeping;

namespace RhonAyro.Infrastructure.Storage.Api
{
    /// <summary>
    /// Provides means to manage instances of <see cref="Competition"/>.
    /// </summary>
    public interface ICompetitionRepository : IRepository<Competition>, IFileRepository
    {
    }
}
