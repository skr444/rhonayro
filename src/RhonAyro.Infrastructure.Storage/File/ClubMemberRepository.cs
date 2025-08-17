using RhonAyro.Common.Data.ScoreKeeping;
using RhonAyro.Infrastructure.Runtime.Api;
using RhonAyro.Infrastructure.Storage.Api;

namespace RhonAyro.Infrastructure.Storage.File
{
    /// <summary>
    /// Manages instances of <see cref="ClubMember"/>.
    /// </summary>
    internal sealed class ClubMemberRepository : FileRepository<ClubMember>, IClubMemberRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="ClubMemberRepository"/>.
        /// </summary>
        /// <param name="storageFilePath">Filesystem path pointing to the storage file.</param>
        /// <param name="lifecycleManagement">Central lifecycle management.</param>
        public ClubMemberRepository(string storageFilePath, ILifecycleManager lifecycleManagement)
            : base(storageFilePath, lifecycleManagement)
        {
        }
    }
}
