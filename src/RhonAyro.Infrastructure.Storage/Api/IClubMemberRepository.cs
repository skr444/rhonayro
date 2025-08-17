using RhonAyro.Common.Data.ScoreKeeping;

namespace RhonAyro.Infrastructure.Storage.Api
{
    /// <summary>
    /// Provides means to manage instances of <see cref="ClubMember"/>.
    /// </summary>
    public interface IClubMemberRepository : IRepository<ClubMember>, IFileRepository
    {
    }
}
