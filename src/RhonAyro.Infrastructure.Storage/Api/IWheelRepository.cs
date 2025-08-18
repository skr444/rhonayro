using RhonAyro.Common.Data ScoreKeeping;

namespace RhonAyro.Infrastructure.Storage.Api
{
    /// <summary>
    /// Provides means to manage instances of <see cref="Wheel"/>.
    /// </summary>
    public interface IWheelRepository : IRepository<Wheel>, IFileRepository
    {
    }
}
