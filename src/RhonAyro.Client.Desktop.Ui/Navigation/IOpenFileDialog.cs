using System;
using System.Windows;

using Microsoft.Win32;

namespace RhonAyro.Client.Desktop.Ui.Navigation
{
    /// <summary>
    /// Mirrors the <see cref="OpenFileDialog"/> API surface.
    /// </summary>
    internal interface IOpenFileDialog
    {
        /// <inheritdoc cref="FileDialog.AddExtension"/>
        bool AddExtension { get; set; }

        /// <inheritdoc cref="CommonItemDialog.AddToRecent"/>
        bool AddToRecent { get; set; }

        /// <inheritdoc cref="FileDialog.CheckFileExists"/>
        bool CheckFileExists { get; set; }

        /// <inheritdoc cref="FileDialog.CheckPathExists"/>
        bool CheckPathExists { get; set; }

        /// <inheritdoc cref="CommonItemDialog.ClientGuid"/>
        Guid? ClientGuid { get; set; }

        /// <inheritdoc cref="CommonItemDialog.DefaultDirectory"/>
        string DefaultDirectory { get; set; }

        /// <inheritdoc cref="FileDialog.DefaultExt"/>
        string DefaultExt { get; set; }

        /// <inheritdoc cref="CommonItemDialog.DereferenceLinks"/>
        bool DereferenceLinks { get; set; }

        /// <inheritdoc cref="FileDialog.FileName"/>
        string FileName { get; set; }

        /// <inheritdoc cref="FileDialog.FileNames"/>
        string[] FileNames { get; }

        /// <inheritdoc cref="FileDialog.Filter"/>
        string Filter { get; set; }

        /// <inheritdoc cref="FileDialog.FilterIndex"/>
        int FilterIndex { get; set; }

        /// <inheritdoc cref="OpenFileDialog.ForcePreviewPane"/>
        bool ForcePreviewPane { get; set; }

        /// <inheritdoc cref="CommonItemDialog.InitialDirectory"/>
        string InitialDirectory { get; set; }

        /// <inheritdoc cref="OpenFileDialog.Multiselect"/>
        bool Multiselect { get; set; }

        /// <inheritdoc cref="OpenFileDialog.ReadOnlyChecked"/>
        bool ReadOnlyChecked { get; set; }

        /// <inheritdoc cref="CommonItemDialog.RootDirectory"/>
        string RootDirectory { get; set; }

        /// <inheritdoc cref="FileDialog.SafeFileNames"/>
        string[] SafeFileNames { get; }

        /// <inheritdoc cref="CommonItemDialog.ShowHiddenItems"/>
        bool ShowHiddenItems { get; set; }

        /// <inheritdoc cref="OpenFileDialog.ShowReadOnly"/>
        bool ShowReadOnly { get; set; }

        /// <inheritdoc cref="CommonItemDialog.Title"/>
        string Title { get; set; }

        /// <inheritdoc cref="CommonItemDialog.ValidateNames"/>
        bool ValidateNames { get; set; }

        /// <inheritdoc cref="CommonDialog.ShowDialog()"/>
        bool? ShowDialog(Window? owner = null);
    }
}
