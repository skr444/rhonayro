using RhonAyro.Common.Data ScoreKeeping;

namespace RhonAyro.Infrastructure.Storage.Api
{
    /// <summary>
    /// Provides means to manage instances of <see cref="ScoreEntry"/>.
    /// </summary>
    public interface IScoreEntryRepository : IRepository<ScoreEntry>, IFileRepository
    {
    }
}
