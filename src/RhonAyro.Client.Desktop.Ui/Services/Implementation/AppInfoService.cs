using System.Diagnostics;
using System.Reflection;

namespace RhonAyro.Client.Desktop.Ui.Services.Implementation
{
    /// <inheritdoc/>
    internal sealed class AppInfoService : IAppInfoService
    {
        private const string AppNameValue = "RhonAyro";
        private const string AppTitleValue = "Wheel Gymnastics Scorekeeping";

        private readonly AssemblyName assemblyName;
        private readonly Assembly assembly;
        private readonly FileVersionInfo fileVersionInfo;

        /// <inheritdoc/>
        public string AppName => fileVersionInfo.CompanyName ?? AppNameValue;

        /// <inheritdoc/>
        public string AppTitle => fileVersionInfo.LegalTrademarks ?? AppTitleValue;

        /// <inheritdoc/>
        public string Version => fileVersionInfo.FileVersion ?? assemblyName.Version?.ToString() ?? "n/a";

        /// <inheritdoc/>
        public string Copyright => fileVersionInfo.LegalCopyright ?? "skr444";

        /// <summary>
        /// Creates a new instance of <see cref="AppInfoService"/>.
        /// </summary>
        public AppInfoService()
        {
            assembly = Assembly.GetExecutingAssembly();
            assemblyName = assembly.GetName();
            fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
        }
    }
}
