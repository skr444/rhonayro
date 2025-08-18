using RhonAyro.Common.Data.ScoreKeeping;
using RhonAyro.Infrastructure.Runtime.Api;
using RhonAyro.Infrastructure.Storage.Api;

namespace RhonAyro.Infrastructure.Storage.File
{
    /// <summary>
    /// Manages instances of <see cref="PerformanceEntry"/>.
    /// </summary>
    internal sealed class PerformanceEntryRepository : FileRepository<PerformanceEntry>, IPerformanceEntryRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="PerformanceEntryRepository"/>.
        /// </summary>
        /// <param name="storageFilePath">Filesystem path pointing to the storage file.</param>
        /// <param name="lifecycleManagement">Central lifecycle management.</param>
        public PerformanceEntryRepository(string storageFilePath, ILifecycleManager lifecycleManagement)
            : base(storageFilePath, lifecycleManagement)
        {
        }
    }
}
