using System.Diagnostics;
using System.IO;

using RhonAyro.Infrastructure.Storage.Api;

namespace RhonAyro.Client.Desktop.Ui.Services.Implementation
{
    /// <inheritdoc/>
    internal sealed class FileSystemService : IFileSystemService
    {
        private const string ExplorerExe = "explorer.exe";

        private readonly IFileStorage storage;

        /// <summary>
        /// Creates a new instance of <see cref="FileSystemService"/>.
        /// </summary>
        /// <param name="fileStorage">File storage service.</param>
        public FileSystemService(IFileStorage fileStorage)
        {
            storage = fileStorage;
        }

        /// <inheritdoc/>
        public void OpenApplicationDataFolderInExplorer()
        {
            OpenFolderInExplorer(storage.StorageDirectory);
        }

        /// <inheritdoc/>
        public void OpenFolderInExplorer(string path)
        {
            if (Directory.Exists(path))
            {
                Process.Start(ExplorerExe, path);
            }
        }

        /// <inheritdoc/>
        public byte[] ReadFileBytes(string path)
        {
            return File.ReadAllBytes(path);
        }
    }
}
