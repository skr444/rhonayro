using RhonAyro.Common.Data.ScoreKeeping;

namespace RhonAyro.Infrastructure.Storage.Api
{
    /// <summary>
    /// Provides means to manage instances of <see cref="PerformanceEntry"/>.
    /// </summary>
    public interface IPerformanceEntryRepository : IRepository<PerformanceEntry>, IFileRepository
    {
    }
}
