using RhonAyro.Common.Data.ScoreKeeping;

namespace RhonAyro.Infrastructure.Storage.Api
{
    /// <summary>
    /// Provides means to manage instances of <see cref="StartListEntry"/>.
    /// </summary>
    public interface IStartListEntryRepository : IRepository<StartListEntry>, IFileRepository
    {
    }
}
