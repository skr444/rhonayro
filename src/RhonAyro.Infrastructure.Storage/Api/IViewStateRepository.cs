using RhonAyro.Common.Data.Ui;

namespace RhonAyro.Infrastructure.Storage.Api
{
    /// <summary>
    /// Manages and persists view state information.
    /// </summary>
    public interface IViewStateRepository : IRepository<ViewStateEntry>, IFileRepository
    {
        bool TryGet(string key, out string value);

        bool TryGet(string view, string key, out string value);
    }
}
