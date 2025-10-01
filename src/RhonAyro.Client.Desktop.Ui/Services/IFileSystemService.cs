namespace RhonAyro.Client.Desktop.Ui.Services
{
    /// <summary>
    /// Provides access to the file system.
    /// </summary>
    internal interface IFileSystemService
    {
        /// <summary>
        /// Opens the specified folder in Windows explorer.
        /// </summary>
        /// <param name="path">The directory to open in explorer. </param>
        void OpenFolderInExplorer(string path);

        /// <summary>
        /// Opens the application folder in Windows explorer.
        /// </summary>
        void OpenApplicationDataFolderInExplorer();

        /// <summary>
        /// Opens a file, reads its contents and closes it.
        /// </summary>
        /// <param name="path">The file to read.</param>
        /// <returns>The file contents as an array of bytes.</returns>
        byte[] ReadFileBytes(string path);
    }
}
