using RhonAyro.Common.Data.ScoreKeeping;

namespace RhonAyro.Infrastructure.Storage.Api
{
    /// <summary>
    /// Provides means to manage instances of <see cref="DisciplineSession"/>.
    /// </summary>
    public interface IDisciplineSessionRepository : IRepository<DisciplineSession>, IFileRepository
    {
    }
}
