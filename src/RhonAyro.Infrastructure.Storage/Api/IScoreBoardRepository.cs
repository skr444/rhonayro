using RhonAyro.Common.Data.ScoreKeeping;

namespace RhonAyro.Infrastructure.Storage.Api
{
    /// <summary>
    /// Provides means to manage instances of <see cref="ScoreBoard"/>.
    /// </summary>
    public interface IScoreBoardRepository : IRepository<ScoreBoard>, IFileRepository
    {
    }
}
