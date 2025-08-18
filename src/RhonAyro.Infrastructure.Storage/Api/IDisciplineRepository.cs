using RhonAyro.Common.Data.ScoreKeeping;

namespace RhonAyro.Infrastructure.Storage.Api
{
    /// <summary>
    /// Provides means to manage instances of <see cref="Discipline"/>.
    /// </summary>
    public interface IDisciplineRepository : IRepository<Discipline>, IFileRepository
    {
    }
}
