using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RhonAyro.Client.Desktop.Ui.Navigation
{
    internal interface IOpenFileDialog
    {
        bool AddExtension { get; set; }
        bool AddToRecent { get; set; }
        bool CheckFileExists { get; set; }
        bool CheckPathExists { get; set; }
        Guid? ClientGuid { get; set; }
        string DefaultDirectory { get; set; }
        string DefaultExt { get; set; }
        bool DereferenceLinks { get; set; }
        string FileName { get; set; }
        string[] FileNames { get; }
        string Filter { get; set; }
        int FilterIndex { get; set; }
        bool ForcePreviewPane { get; set; }
        string InitialDirectory { get; set; }
        bool Multiselect { get; set; }
        bool ReadOnlyChecked { get; set; }
        string RootDirectory { get; set; }
        string[] SafeFileNames { get; }
        bool ShowHiddenItems { get; set; }
        bool ShowReadOnly { get; set; }
        string Title { get; set; }
        bool ValidateNames { get; set; }

        bool? ShowDialog(Window? owner = null);
    }
}
