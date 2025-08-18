using RhonAyro.Common Data ScoreKeeping;
using RhonAyro Infrastructure Runtime Api;
using RhonAyro Infrastructure Storage Api;

namespace RhonAyro.Infrastructure Storage File
{
    /// <summary>
    /// Manages instances of <see cref="Wheel"/>.
    /// </summary>
    internal sealed class WheelRepository : FileRepository<Wheel>, IWheelRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="WheelRepository"/>.
        /// </summary>
        /// <param name="storageFilePath">Filesystem path pointing to the storage file.</param>
        /// <param name="lifecycleManagement">Central lifecycle management.</param>
        public WheelRepository(string storageFilePath, ILifecycleManager lifecycleManagement)
            : base(storageFilePath, lifecycleManagement)
        {
        }
    }
}
